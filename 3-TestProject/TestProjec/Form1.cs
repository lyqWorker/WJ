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
            //Random random = new Random();
            //int num = random.Next(0, 60);
            //string imgFileName = num.ToString() + ".jpg";
            //string imgUrl = Path.Combine(ImgPath, imgFileName);
            //richTextBox1.Text = CommonUtils.ImgToBase64String(new Bitmap(imgUrl));
            //string url = "http://localhost:8001/api/Img/GetBitmap";
            //string str = HttpUtils.GetData(url);
            //richTextBox1.Text = str;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string result = richTextBox1.Text.Trim('"');
            //var bytes = Convert.FromBase64String(result);
            //using (MemoryStream stream = new MemoryStream(bytes))
            //{
            //    pictureBox1.Image = Image.FromStream(stream);
            //}
            pictureBox1.Image = CommonUtils.Base64StringToImg(result);
        }
    }
}
