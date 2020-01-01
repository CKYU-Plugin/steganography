using Steganography.Property;
using Steganography.Robot.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steganography
{
    public partial class frmSet : Form
    {
        public frmSet()
        {
            InitializeComponent();
        }


        private void frmSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            HandlerProperty.SProperty.admin = ToNum(txtAdminQq.Text);
            Config.SaveProperty();
            MessageBox.Show("已储存", "储存");
        }

        private string ToNum(string str)
        {
            return new String(str.Where(Char.IsDigit).ToArray());
        }

        private void frmSet_Shown(object sender, EventArgs e)
        {
            Config.LoadProperty();
            txtAdminQq.Text = HandlerProperty.SProperty.admin;
        }
    }
}
