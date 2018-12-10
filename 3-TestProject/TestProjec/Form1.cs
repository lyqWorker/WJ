using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace TestProjec
{
    public partial class Form1 : Form
    {
        public string ImgPath = "C:\\VCodeImg\\";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int num = random.Next(0, 60);
            string imgFileName = num.ToString() + ".jpg";
            string imgUrl = Path.Combine(ImgPath, imgFileName);
            richTextBox1.Text = CommonUtils.ImgToBase64String(new Bitmap(imgUrl));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = CommonUtils.Base64StringToImg(richTextBox1.Text);
        }
    }
}
