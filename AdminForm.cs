using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Курсовая_работа
{
    public partial class AdminForm : Form
    {
        private DataManager<User> userManager; // Экземпляр DataManager для управления пользователями
        private Admin admin; // Объект Admin для работы с администрацией

        public AdminForm(Admin userAdmin) // Передаем объект Admin через конструктор
        {
            InitializeComponent();
            this.admin = userAdmin; // Сохраняем ссылку на объект Admin

            //userManager = new DataManager<User>("users.json"); // Инициализация DataManager для управления пользователями

            // Отображение имени администратора
            label1.Text = $"{admin.Surname} {admin.Name}";

            // Загрузка пользователей в DataGridView при загрузке формы
            LoadUsersIntoGrid();
        }

        // Метод для загрузки пользователей в DataGridView
        public void LoadUsersIntoGrid()
        {
            dataGridView1.DataSource = null; // Очищаем текущие данные
            userManager = new DataManager<User>("users.json"); // Инициализация DataManager для управления пользователями
            dataGridView1.DataSource = userManager.Items; // Задаем новый источник данных для DataGridView
        }

        private void button1_Click(object sender, EventArgs e) //добавить сотрудника
        {
            // Открываем форму для добавления нового пользователя и передаем текущего администратора
            AddUser newForm = new AddUser(admin);
            newForm.Owner = this;
            newForm.ShowDialog(); // Используем ShowDialog, чтобы ожидать закрытия новой формы

        }

        private void button2_Click(object sender, EventArgs e) // удалить сотрудника
        {
            try
            {
                // Проверка, что выделена строка в DataGridView
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Получаем индекс выделенной строки
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;

                    // Получаем объект User из выделенной строки
                    User selectedUser = (User)dataGridView1.SelectedRows[0].DataBoundItem;

                    if (selectedUser != null)
                    {
                        // Удаляем пользователя через Admin
                        admin.RemoveUser(selectedUser.Id);

                        // Обновляем данные в DataGridView после удаления пользователя
                        LoadUsersIntoGrid();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите пользователя для удаления.");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Показываем сообщение, если у пользователя есть незавершённые заказы
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Показываем сообщение для любых других ошибок
                MessageBox.Show("Произошла ошибка при удалении пользователя: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
