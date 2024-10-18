using System;
using System.Windows.Forms;

namespace Курсовая_работа
{
    public partial class AddUser : Form
    {
        private Admin admin;
        public AddUser(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Считываем данные из полей формы 
                // Получение значений из TextBox для заполнения данных о заказе
                string login = textBox1.Text; //login
                string password = textBox2.Text; //пароль
                string surname = textBox3.Text; //фамилия
                string name = textBox4.Text; //имя
                string patronymic = textBox5.Text;//отчество
                string contactPhone = textBox6.Text;//телефон
                string email = textBox7.Text;//почта
                int accessCode;
                if (comboBox1.Text == "Менеджер пункта приема")
                {
                    accessCode = 1;
                }
                else if (comboBox1.Text == "Мастер")
                {
                    accessCode = 2;
                }
                else if (comboBox1.Text == "Администратор")
                {
                    accessCode = 0;
                }
                else
                {
                    throw new ArgumentException("Выберите правильную роль из списка.");
                }

                admin.AddNewUser(login, password, surname, name, patronymic, contactPhone, email, accessCode);
                ((AdminForm)Owner).LoadUsersIntoGrid();
                this.Close(); // Скрыть текущую форму 

            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }
    }
}
