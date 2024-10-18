using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая_работа
{
    public partial class AddOrder : Form
    {
        private Manager manager;
        public AddOrder(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Считываем данные из полей формы 
                // Получение значений из TextBox для заполнения данных о заказе
                string surname = textBox1.Text; //login
                string name = textBox2.Text; //пароль
                string patronymic = textBox3.Text; //фамилия
                string contactPhone = textBox4.Text; //имя
                DeviceType deviceType = (DeviceType)comboBox1.SelectedIndex;
                string model = textBox5.Text;
                string serialNumber = textBox6.Text;
                string externalConditionComment = textBox7.Text;
                string issue = textBox8.Text;
                

                manager.AddOrder(surname, name, patronymic, contactPhone, deviceType, model, serialNumber, externalConditionComment, issue);
                ((ManagerForm)Owner).LoadOrdersIntoGrid();
                this.Close(); // Скрыть текущую форму 

            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }

        private void AddOrder_Load(object sender, EventArgs e)
        {

        }
    }
}
