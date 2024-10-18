using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая_работа
{
    public partial class receipt_2 : Form
    {
        public receipt_2(Order ord)
        {
            InitializeComponent();

            label1.Text = ord.DateOfInquiry.ToString("");
            label2.Text = ord.LastName + " " + ord.FirstName + " " + ord.Patronymic;
            label3.Text = ord.ContactPhone;
            label4.Text = ord.Model + "   " + ord.SerialNumber;
            label5.Text = ord.Issue;
            label6.Text = ord.ExternalConditionComment;
            label7.Text = ord.Price;
            DisplayDiagnoses(ord.Diagnoses); // Отображаем диагностики

            pictureBox1.Controls.Add(label1);
            pictureBox1.Controls.Add(label2);
            pictureBox1.Controls.Add(label3);
            pictureBox1.Controls.Add(label4);
            pictureBox1.Controls.Add(label5);
            pictureBox1.Controls.Add(label6);
            pictureBox1.Controls.Add(label7);
            pictureBox1.Controls.Add(listBox1);
            pictureBox1.Controls.Add(listBox2);

            void DisplayDiagnoses(List<Service> diagnoses)
            {
                // Отображение списка диагностик на форме, например, в ListBox или другом элементе управления
                listBox1.Items.Clear(); // Предполагается наличие ListBox с именем listBoxDiagnoses
                listBox2.Items.Clear(); // Предполагается наличие ListBox с именем listBoxDiagnoses

                foreach (var diagnosis in diagnoses)
                {
                    listBox1.Items.Add($"{diagnosis.ServiceName}");
                    listBox2.Items.Add($"{diagnosis.Price}"); 
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bm, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            bm.Save("Чек " + label2.Text + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            bm.Dispose();
            this.Close();
        }
    }
}
