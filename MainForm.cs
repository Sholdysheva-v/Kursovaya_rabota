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
    public partial class MainForm : Form
    {
        private DataManager<User> userManager;
        private DataManager<AccessData> accessManager;
        public MainForm()
        {
            InitializeComponent();
            userManager = new DataManager<User>("users.json");
            accessManager = new DataManager<AccessData>("access.json");
        }

        private int GetAccessCode(User user) //получение кода доступа
        {
            var accessData = accessManager.FindItem(ad => ad.UserIds.Contains(user.Id));
            return accessData?.AccessCode ?? 3; // Возвращаем код доступа или 3 (default) , если не найден
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;

            User user = userManager.FindItem(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                // Получаем код доступа
                int accessCode = GetAccessCode(user);

                // Открываем форму на основе кода доступа
                switch (accessCode)
                {
                    case 0:
                        Admin admin = new Admin(user);
                        AdminForm adminForm = new AdminForm(admin);
                        adminForm.Show();
                        this.Hide();
                        adminForm.FormClosed += delegate
                        {
                            this.Close();
                        };
                        break;
                    case 1:
                        Manager manager = new Manager(user);
                        ManagerForm managerForm = new ManagerForm(manager);
                        managerForm.Show();
                        this.Hide();
                        managerForm.FormClosed += delegate
                        {
                            this.Close();
                        };
                        break;
                    case 2:
                        Engineer engineer = new Engineer(user);
                        EngineerForm engineerForm = new EngineerForm(engineer);
                        engineerForm.Show();
                        this.Hide();
                        engineerForm.FormClosed += delegate
                        {
                            this.Close();
                        };
                        break;
                    default:
                        MessageBox.Show("Нет доступа для этого пользователя.");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }
    }
}
