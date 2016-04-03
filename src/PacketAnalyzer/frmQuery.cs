using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketAnalyzer
{
    public partial class frmQuery : Form
    {
        private ulong _startTime;                       // query: start time
        private ulong _endTime;                         // query: end time
        private List<int> _protocols = new List<int>(); // query: protocol

        public frmQuery()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _startTime = TimeConvert.DateTimeToUnixTimestamp(startDateTime.Value);
            _endTime = TimeConvert.DateTimeToUnixTimestamp(endDateTime.Value);

            // Add List
            _protocols.Clear();
            for (int i=0; i<chkListBox.CheckedItems.Count; i++)
            {
                int port = ProtocolInfo.instanceOf().ConvertToPort((string)chkListBox.CheckedItems[i]);
                _protocols.Add(port);
            }

            if (_startTime > _endTime)
            {
                MessageBox.Show("Start time should be equal to or earlier than the end time", "Error");
                return;
            }
            if (_protocols.Count <= 0)
            {
                MessageBox.Show("You have to select at least one protocol", "Error");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmQuery_Load(object sender, EventArgs e)
        {
            int nCount = ProtocolInfo.instanceOf().GetPortCount();
            for (int i=0; i<nCount; i++)
            {
                int port = ProtocolInfo.instanceOf().GetPortAt(i);
                chkListBox.Items.Add(ProtocolInfo.instanceOf().ConvertToString(port));
            }
            _selectAll(true);
        }

        private void _selectAll(bool bSelect)
        {
            int nCount = chkListBox.Items.Count;
            for (int i = 0; i < nCount; i++)
            {
                chkListBox.SetItemChecked(i, bSelect);
            }
        }
        
        public ulong GetStartTime()
        {
            return _startTime;
        }
        public ulong GetEndTime()
        {
            return _endTime;
        }
        public List<int> GetProtocolList()
        {
            return _protocols;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            _selectAll(true);
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            _selectAll(false);
        }
    }
}
