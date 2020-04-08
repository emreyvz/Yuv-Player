using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YazLab_III
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
             Form1.width = Convert.ToInt32(bunifuTextbox1.text);
             Form1.height = Convert.ToInt32(bunifuTextbox2.text);
             Form1.type = Convert.ToInt32(bunifuDropdown1.selectedIndex);
            this.Close();
        }
    }
}
