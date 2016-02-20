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
        private DateTime _startTime;    // query: start time
        private DateTime _endTime;      // query: end time
        private Array _protocols;       // query: protocol

        public frmQuery()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
