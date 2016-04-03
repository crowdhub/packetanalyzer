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
    public partial class frmSearchPacket : Form
    {
        private string _hexcode = "";

        public frmSearchPacket()
        {
            InitializeComponent();
        }

        public string GetSearchString()
        {
            return _hexcode;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _hexcode = txtHexcode.Text;
            if (_hexcode.Length <= 0)
            {
                MessageBox.Show("You have to enter searching text(Hex value)", "Error");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
