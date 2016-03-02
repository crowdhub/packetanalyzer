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

namespace PacketAnalyzer
{
    public partial class frmSelectNIC : Form
    {
        private int _selectedNIC = -1;

        public frmSelectNIC()
        {
            InitializeComponent();
        }

        private void frmSelectNIC_Load(object sender, EventArgs e)
        {
            // Obtain the interface lists and add them to the list box
            foreach (var device in CaptureDeviceList.Instance)
            {
                String str;
                str = device.Description;
                str += " (";
                str += device.Name;
                str += ")";
                lstDevice.Items.Add(str);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _selectedNIC = lstDevice.SelectedIndex;
            if (_selectedNIC < 0)
            {
                MessageBox.Show("There is no Interface selected");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int GetSelectedNIC()
        {
            return _selectedNIC;
        }
    }
}
