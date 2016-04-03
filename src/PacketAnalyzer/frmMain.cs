using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using SharpPcap;
using PacketDotNet;
using PacketDotNet.Utils;
using System.Threading;
using Be.Windows.Forms;

namespace PacketAnalyzer
{
    public partial class frmMain : Form
    {
        // synchronization object
        private object _sync = new object();

        private bool _requestStop = false;
        private Thread _threadObject = null;
        private ICaptureDevice _curNIC = null;
        // event handler
        private PacketArrivalEventHandler _eventPacketArrival = null;
        private CaptureStoppedEventHandler _eventCaptureStopped = null;

        // manage all captured packets
        private List<RawCapture> _capturedPackets = new List<RawCapture>();

        // Database
        OArchive _db = new OArchive();
        // current packet list
        OCurrentPacketList _curPacketList = new OCurrentPacketList();

        private void _ThreadProc()
        {
            while (false == _requestStop)
            {
                bool _sleep = false;
                // Get packet from packet queue
                lock (_sync)
                {
                    // If there is no packets
                    if (_capturedPackets.Count <= 0)
                    {
                        _sleep = true;
                    }
                }
                // If there is no packet captured
                if (true == _sleep)
                {
                    Thread.Sleep(100);
                    continue;
                }

                // At this time, we have packets captured
                // We have to handle this packet;
                List<RawCapture> curPacket = null;
                lock(_sync)
                {
                    curPacket = _capturedPackets;
                    _capturedPackets = new List<RawCapture>();
                }
                foreach(RawCapture r in curPacket)
                {
                    // 1. Parse packet
                    OPacket packet = new OPacket();
                    if (false == _ParsePacket(r, packet))
                    {
                        continue;
                    }

                    // 2. Save packet to database
                    lock (_sync)
                    {
                        _db.AddPacket(packet, r.Data);
                    }

                    int nNextNo = lstPackets.Items.Count + 1;
                    ListViewItem item = new ListViewItem(nNextNo.ToString());
                    item.SubItems.Add(packet.GetCollectedTime().ToString());
                    item.SubItems.Add(packet.GetSourceIP());
                    item.SubItems.Add(packet.GetDestinationIP());
                    item.SubItems.Add(ProtocolInfo.instanceOf().ConvertToString(packet.GetProtocol()));
                    item.SubItems.Add(r.Data.Length.ToString());

                    // We need to show captured packet in the UI
                    // but, this code is running in different thread from the UI thread
                    // So, we have to use BeginInvoke to transfer request to UI thread
                    this.Invoke(new Action(delegate ()
                    {
                        lstPackets.Items.Add(item);
                    }
                    ));

                    // Add OPacket object in the list
                    _curPacketList.Add(packet);
                }
                curPacket.Clear();
            }
        }

        private bool _ParsePacket(RawCapture r, OPacket p)
        {
            Packet packet = PacketDotNet.Packet.ParsePacket(r.LinkLayerType, r.Data);
            IpPacket ip = PacketDotNet.IpPacket.GetEncapsulated(packet);
            if (null == ip)
            {
                return false;
            }
            p.SetSourceIP(ip.SourceAddress.ToString());
            p.SetDestinationIP(ip.DestinationAddress.ToString());
            p.SetLength(r.Data.Length);
            p.SetCollectedTime(r.Timeval.Seconds);
            TcpPacket tcp = PacketDotNet.TcpPacket.GetEncapsulated(packet);
            if (null != tcp)
            {
                p.SetProtocol(tcp.DestinationPort);
                return true;
            }
//            UdpPacket udp = PacketDotNet.UdpPacket.GetEncapsulated(packet);
//            if (null != udp)
//            {
//                p.SetProtocol(udp.DestinationPort);
//                return true;
//            }

            return false;
        }

        private void _OnPacketArrival(object sender, CaptureEventArgs e)
        {
            // synchronize..
            lock (_sync)
            {
                _capturedPackets.Add(e.Packet);
            }
        }
        private void _OnCaptureStopped(object sender, CaptureStoppedEventStatus status)
        {
        }

        private bool _StartCapture()
        {
            // if capturing is already started, just return true
            if (null != _curNIC)
            {
                return true;
            }

            int nSelectedNIC = OConfiguration.instanceOf().GetSelectedNIC();
            if (nSelectedNIC < 0)
            {
                MessageBox.Show("There is no device selected.", "Error");
                return false;
            }
            if (nSelectedNIC >= CaptureDeviceList.Instance.Count)
            {
                MessageBox.Show("The selected device is not valid. Select device again", "Error");
                return false;
            }

            // clear all packet before starting capture
            _capturedPackets.Clear();
            // clear all current packet list
            _curPacketList.Clear();
            // clear ui
            lstPackets.Items.Clear();
            hexEdit.ByteProvider = null;

            _curNIC = CaptureDeviceList.Instance[nSelectedNIC];
            _threadObject = new Thread(_ThreadProc);
            _threadObject.Start();

            // Set up capture
            _eventPacketArrival = new PacketArrivalEventHandler(_OnPacketArrival); ;
            _eventCaptureStopped = new CaptureStoppedEventHandler(_OnCaptureStopped);
            _curNIC.OnPacketArrival += _eventPacketArrival;
            _curNIC.OnCaptureStopped += _eventCaptureStopped;
            _curNIC.Open();
            // start capture
            _curNIC.StartCapture();

            return true;
        }
        private void _StopCapture()
        {
            // if capturing was not started, just return
            if (null == _curNIC)
            {
                return;
            }

            _requestStop = true;

            _threadObject.Join();
            _threadObject = null;

            _curNIC.OnPacketArrival -= _eventPacketArrival;
            _curNIC.OnCaptureStopped -= _eventCaptureStopped;

            _eventPacketArrival = null;
            _eventCaptureStopped = null;
            _curNIC = null;
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private int _SelectNIC()
        {
            frmSelectNIC f = new frmSelectNIC();
            if (DialogResult.OK != f.ShowDialog())
            {
                return -1;
            }
            return f.GetSelectedNIC();
        }

        private byte[] _StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray();
        }

        private bool _ContainsSearchBytes(byte[] searchBytes, string path)
        {
            // Not fast method.
            byte[] fileBytes = File.ReadAllBytes(path);
            if (fileBytes.Length <= 0)
            {
                return false;
            }

            for (int i = 0; i < fileBytes.Length - searchBytes.Length; i++)
            {
                bool match = true;
                for (int k = 0; k < searchBytes.Length; k++)
                {
                    if (fileBytes[i + k] != searchBytes[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    int pos = i + searchBytes.Length;
                    return match;
                }
            }
            return false;
        }

        private void _ShowCurrentSelectedPacketContents()
        {
            if (lstPackets.SelectedItems.Count <= 0)
            {
                return;
            }
            int nIndex = lstPackets.SelectedItems[0].Index;
            if (nIndex < 0)
            {
                return;
            }
            OPacket selectedPacket = _curPacketList.GetAt(nIndex);
            if (null == selectedPacket)
            {
                return;
            }

            try
            {
                DynamicFileByteProvider dynamicFileByteProvider = new DynamicFileByteProvider(selectedPacket.GetPacketStoredPath(), true);
                hexEdit.ByteProvider = dynamicFileByteProvider;
            }
            catch (IOException)
            {
            }
        }

        // Start capture
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check whether interface is selected first
            int nCurNIC = OConfiguration.instanceOf().GetSelectedNIC();
            // if there is no NIC selected, ask user
            if (nCurNIC < 0)
            {
                nCurNIC = _SelectNIC();
                if (nCurNIC < 0)
                {
                    return;
                }
                OConfiguration.instanceOf().SetSelectedNIC(nCurNIC);
            }
            // Start packet capturing
            _StartCapture();

            // Add code: Disable Start button, Enable Stop button
        }

        // Pause capture
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Add code: Disable Stop button, Enale Start button
        }

        // Stop capture
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _StopCapture();

            // Add code: enable Start button
        }

        // Exit program
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void statisticsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_curPacketList.GetCount() <= 0)
            {
                MessageBox.Show("There is no packets to build statistics. Make packet list first by capturing or querying", "Error");
                return;
            }

            // Show statistics form
            frmStatistics f = new frmStatistics();
            f.SetPacketList(_curPacketList);
            f.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Create folder for saving packets
            Directory.CreateDirectory(OArchive.GetArchiveFolder());

            // Add column in the list control
            lstPackets.Columns.Add("No", 50, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Time", 100, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Source", 150, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Destination", 150, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Protocol", 150, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Length", 60, HorizontalAlignment.Center);

            // Load database
            if (false == _db.OpenDatabase(OArchive.DBName(), true))
            {
                MessageBox.Show("Failed to open database. Please check database first before running program", "Error");
                this.Close();
                return;
            }

//            lstPackets.Items.Add("1");
//            lstPackets.Items.Add("2");
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmQuery fQuery = new frmQuery();
            if (DialogResult.OK != fQuery.ShowDialog())
            {
                return;
            }
            // 1. Clear current packet list
            _curPacketList.Clear();
            // 2. Clear UI
            lstPackets.Items.Clear();
            hexEdit.ByteProvider = null;
            // 3. Query packet list from the database
            _db.Query(fQuery.GetStartTime(), fQuery.GetEndTime(), fQuery.GetProtocolList(), _curPacketList);

            if (_curPacketList.GetCount() <= 0)
            {
                MessageBox.Show("There is no data object that corresponds to search criteria", "Information");
                return;
            }

            // Add packet data in the UI
            for (int i = 0; i < _curPacketList.GetCount(); i++)
            {
                OPacket packet = _curPacketList.GetAt(i);
                int nNextNo = lstPackets.Items.Count + 1;
                ListViewItem item = new ListViewItem(nNextNo.ToString());
                item.SubItems.Add(packet.GetCollectedTime().ToString());
                item.SubItems.Add(packet.GetSourceIP());
                item.SubItems.Add(packet.GetDestinationIP());
                item.SubItems.Add(ProtocolInfo.instanceOf().ConvertToString(packet.GetProtocol()));
                item.SubItems.Add(packet.GetLength().ToString());

                lstPackets.Items.Add(item);
            }
        }

        // Select Interface
        private void setInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int nCurNIC = _SelectNIC();
            if (nCurNIC >= 0)
            {
                // Set current Selected NIC
                OConfiguration.instanceOf().SetSelectedNIC(nCurNIC);
            }
        }

        private void findPacketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_curPacketList.GetCount() <= 0)
            {
                MessageBox.Show("There is no packet lists to search", "Error");
                return;
            }

            frmSearchPacket f = new frmSearchPacket();
            if (DialogResult.OK != f.ShowDialog())
            {
                return;
            }

            // At this time, we have to search..
            // Build search hex value from the string
            byte[] searchBytes = _StringToByteArray(f.GetSearchString());

            for (int i=0; i<_curPacketList.GetCount(); i++)
            {
                OPacket packet = _curPacketList.GetAt(i);
                if (true == _ContainsSearchBytes(searchBytes, packet.GetPacketStoredPath()))
                {
                    lstPackets.Items[i].Selected = true;
                    _ShowCurrentSelectedPacketContents();
                    break;
                }
            }

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if packet is being captured now, clean up
            _StopCapture();
        }

        private void lstPackets_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ShowCurrentSelectedPacketContents();
        }
    }
}
