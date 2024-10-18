using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Курсовая_работа
{
    public partial class AddService : Form
    {
        private Engineer engineer;
        public AddService(Engineer engineer)
        {
            InitializeComponent();
            this.engineer = engineer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Считываем данные из полей формы 
                // Получение значений из TextBox для заполнения данных о заказе
                string serviceName = textBox1.Text; //услуга
                string price = textBox2.Text; //цена
                DeviceType deviceType = (DeviceType)comboBox1.SelectedIndex;

                engineer.AddNewService(deviceType, serviceName, price);
                ((EngineerForm)Owner).LoadServicesIntoGrid();
                this.Close(); // Скрыть текущую форму 

            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }
    }
}
