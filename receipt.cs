using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая_работа
{
    public partial class receipt : Form
    {
        public receipt(Order ord)
        {
            InitializeComponent();

            label1.Text = ord.DateOfInquiry.ToString("");
            label2.Text = ord.LastName + " " + ord.FirstName + " " + ord.Patronymic;
            label3.Text = ord.ContactPhone;
            label4.Text = ord.Model + "   " + ord.SerialNumber;
            label5.Text = ord.Issue;
            label6.Text = ord.ExternalConditionComment;
            
            pictureBox1.Controls.Add(label1);
            pictureBox1.Controls.Add(label2);
            pictureBox1.Controls.Add(label3);
            pictureBox1.Controls.Add(label4);
            pictureBox1.Controls.Add(label5);
            pictureBox1.Controls.Add(label6);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bm, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            bm.Save("Квитанция_" + label2.Text + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            bm.Dispose();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
