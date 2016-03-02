using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpPcap;
using System.Threading;

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
                    // Add packet in OCurrentPacketList

                    int nNextNo = lstPackets.Items.Count + 1;
                    ListViewItem item = new ListViewItem(nNextNo.ToString());
                    item.SubItems.Add(r.Timeval.ToString());
                    item.SubItems.Add("Src:Need parsing");
                    item.SubItems.Add("Dest:Need parsing");
                    item.SubItems.Add("Protocols");
                    item.SubItems.Add(r.Data.Length.ToString());

                    // We need to show captured packet in the UI
                    // but, this code is running in different thread from the UI thread
                    // So, we have to use BeginInvoke to transfer request to UI thread
                    this.Invoke(new Action(delegate ()
                    {
                        lstPackets.Items.Add(item);
                    }
                    ));

                }
                curPacket.Clear();
            }
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
            // Show statistics form
            frmStatistics f = new frmStatistics();
            f.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Add column in the list control
            lstPackets.Columns.Add("No", 50, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Time", 100, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Source", 150, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Destination", 150, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Protocol", 150, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Length", 60, HorizontalAlignment.Center);
            lstPackets.Columns.Add("Info", 80, HorizontalAlignment.Center);

//            lstPackets.Items.Add("1");
//            lstPackets.Items.Add("2");
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmQuery query = new frmQuery();
            if (DialogResult.OK != query.ShowDialog())
            {
                return;
            }
            // 
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
            frmSearchPacket f = new frmSearchPacket();
            f.ShowDialog();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if packet is being captured now, clean up
            _StopCapture();
        }
    }
}
