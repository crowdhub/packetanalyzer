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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        // Start capture
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        // Pause capture
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Stop capture
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

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

            lstPackets.Items.Add("1");
            lstPackets.Items.Add("2");
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
    }
}
