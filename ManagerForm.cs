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
    public partial class ManagerForm : Form
    {
        private Manager manager;
        private DataManager<Order> orderManager; // Экземпляр DataManager для управления пользователями

        public ManagerForm(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
            label1.Text = $"{manager.Surname} {manager.Name}";
            LoadOrdersIntoGrid();
        }

        // Метод для загрузки заказов в DataGridView
        public void LoadOrdersIntoGrid()
        {
            dataGridView1.DataSource = null; // Очищаем текущие данные
            orderManager = new DataManager<Order>("orders.json"); // Инициализация DataManager для управления пользователями
            dataGridView1.DataSource = orderManager.Items; // Задаем новый источник данных для DataGridView

            // Скрыть некоторые колонки
            dataGridView1.Columns["ManagerId"].Visible = false; 
            dataGridView1.Columns["MasterId"].Visible = false; 
            dataGridView1.Columns["OrderId"].Visible = false; 

            // Переименовать заголовки столбцов
            dataGridView1.Columns["LastName"].HeaderText = "Фамилия";
            dataGridView1.Columns["FirstName"].HeaderText = "Имя";
            dataGridView1.Columns["Patronymic"].HeaderText = "Отчество";
            dataGridView1.Columns["ContactPhone"].HeaderText = "Контактный телефон";
            dataGridView1.Columns["DeviceType"].HeaderText = "Тип устройства";
            dataGridView1.Columns["Model"].HeaderText = "Модель устройства";
            dataGridView1.Columns["SerialNumber"].HeaderText = "Серийный номер";
            dataGridView1.Columns["ExternalConditionComment"].HeaderText = "Комментарий о внешнем состоянии";
            dataGridView1.Columns["Issue"].HeaderText = "Проблема";
            dataGridView1.Columns["IsCompleted"].HeaderText = "Завершен";
            dataGridView1.Columns["DateOfInquiry"].HeaderText = "Дата запроса";
            dataGridView1.Columns["IsDiagnosed"].HeaderText = "Диагностирован";
            dataGridView1.Columns["Price"].HeaderText = "Стоимость";
        }

        private void button1_Click(object sender, EventArgs e) //добавить заказ
        {
            // Открываем форму для добавления нового пользователя и передаем текущего администратора
            AddOrder newForm = new AddOrder(manager);
            newForm.Owner = this;
            newForm.ShowDialog(); // Используем ShowDialog, чтобы ожидать закрытия новой формы
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Проверяем, что выделена строка в DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем объект Order из выделенной строки
                Order selectedOrder = (Order)dataGridView1.SelectedRows[0].DataBoundItem;

                if (selectedOrder != null)
                {
                    try
                    {
                        // Проверяем, завершён ли заказ
                        if (!selectedOrder.IsCompleted)
                        {
                            throw new InvalidOperationException("Невозможно удалить заказ, так как он не завершён.");
                        }

                        // Удаляем заказ через orderManager
                        orderManager.RemoveItem(selectedOrder);

                        // Обновляем данные в DataGridView после удаления заказа
                        LoadOrdersIntoGrid();

                        MessageBox.Show("Заказ успешно удалён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex) // На случай других возможных ошибок
                    {
                        MessageBox.Show("Произошла ошибка при удалении заказа: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления, выделив всю строку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Обновляем данные в DataGridView
            LoadOrdersIntoGrid();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    
                    // Получаем индекс выбранной строки и объект Order
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    Order resOrder = dataGridView1.SelectedRows[0].DataBoundItem as Order; //перевод строки в объект заказ
                    if (resOrder.IsDiagnosed==true) 
                    { 
                        // Открываем новое окно и передаем объект Order
                        receipt_2 recForm = new receipt_2(resOrder);
                        recForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Нельзя выписать чек, пока устройство не прошло диагностику.");
                    }
                }
                else
                {
                    MessageBox.Show("Выберите заказ для выписки квитанции.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }

        private void button2_Click(object sender, EventArgs e) //выписать квитанцию
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Получаем индекс выбранной строки и объект Order
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    Order resOrder = dataGridView1.SelectedRows[0].DataBoundItem as Order; //перевод строки в объект заказ

                    // Открываем новое окно и передаем объект Order
                    receipt recForm = new receipt(resOrder);
                    recForm.Show();
                }
                else
                {
                    MessageBox.Show("Выберите заказ для выписки квитанции.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка, что выделена строка в DataGridView
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Получаем объект Order из выделенной строки
                    Order selectedOrder = (Order)dataGridView1.SelectedRows[0].DataBoundItem as Order;

                    if (selectedOrder != null )
                    {
                        if (selectedOrder.IsDiagnosed == true)
                        {

                            // Изменяем статус заказа на true
                            // Устанавливаем флаг диагностики
                            manager.MarkOrderAsCompleted(selectedOrder.OrderId);

                            // Обновляем данные в DataGridView после изменения статуса
                            LoadOrdersIntoGrid();
                            MessageBox.Show("Статус заказа успешно изменен на 'Завершен'.");
                        }
                        else
                        {
                            MessageBox.Show("Нельзя завершить заказ, пока мастер не провёл диагностику.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите заказ для завершения.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }
    }
}
