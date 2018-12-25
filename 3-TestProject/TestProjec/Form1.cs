using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace TestProjec
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string a = "NBjrHWgBtFw3wrxvFUyc+XY7QqBCLH59L701OXF5JuENo3T3UusjX0baCO9Dp/lzghnx2NBkShKh/okJFrE+9LutCyfpuTqdjowNpbcYob+mu+8OORo7VEoYIG5+ttfhK3EtquWcR2rdK16lhpZDeS/v/M6BnlSW03feTAxx72c=";
            
            textBox1.Text = RSAUtils.DecryptData(a, RSAUtils.GetPrivateKey(), "UTF-8");
        }
    }
}
