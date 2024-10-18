using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая_работа
{
    public partial class EngineerForm : Form
    {
        private Engineer engineer;
        //private DataManager<Order> orderManager; // Экземпляр DataManager для управления пользователями
        //private DataManager<Service> serviceManager; // Экземпляр DataManager для управления услугами
        public EngineerForm(Engineer engineer)
        {
            InitializeComponent();
            this.engineer = engineer;
            label1.Text = $"{engineer.Surname} {engineer.Name}";
            LoadOrdersIntoGrid();
            LoadServicesIntoGrid();
        }
        public void LoadOrdersIntoGrid()
        {
            dataGridView1.DataSource = null; // Очищаем текущие данные
            dataGridView1.DataSource = engineer.GetAssignedOrders(); // Задаем новый источник данных для DataGridView

            // Скрыть некоторые колонки
            dataGridView1.Columns["ManagerId"].Visible = false;
            dataGridView1.Columns["MasterId"].Visible = false;
            dataGridView1.Columns["OrderId"].Visible = false;
            dataGridView1.Columns["LastName"].Visible = false;
            dataGridView1.Columns["FirstName"].Visible = false;
            dataGridView1.Columns["Patronymic"].Visible = false;
            dataGridView1.Columns["ContactPhone"].Visible = false;

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
        public void LoadServicesIntoGrid()
        {
            dataGridView2.DataSource = null; // Очищаем текущие данные
            //serviceManager = new DataManager<Service>("services.json"); // Инициализация DataManager для управления пользователями
            dataGridView2.DataSource = engineer.GetAllServices(); // Задаем новый источник данных для DataGridView

            dataGridView2.Columns["ServiceId"].Visible = false;
            dataGridView2.Columns["DeviceType"].HeaderText = "Тип устройства";
            dataGridView2.Columns["ServiceName"].HeaderText = "Ремонтная услуга";
            dataGridView2.Columns["Price"].HeaderText = "Стоимость";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Открываем форму для добавления нового пользователя и передаем текущего администратора
            AddService newForm = new AddService(engineer);
            newForm.Owner = this;
            newForm.ShowDialog(); // Используем ShowDialog, чтобы ожидать закрытия новой формы
        }

        private void button4_Click(object sender, EventArgs e) // Удалить услугу
        {
            // Проверяем, что выбрана строка в dataGridView2
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Получаем объект Service из выделенной строки через DataBoundItem
                var selectedService = (Service)dataGridView2.SelectedRows[0].DataBoundItem;

                if (selectedService != null)
                {
                    try
                    {
                        // Удаляем услугу через объект Engineer
                        engineer.RemoveService(selectedService.ServiceId);

                        // Обновляем DataGridView
                        LoadServicesIntoGrid();
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите услугу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбрана строка в обоих DataGridView (заказы и услуги)
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView2.SelectedRows.Count > 0)
            {
                // Получаем выбранный заказ
                var selectedOrder = (Order)dataGridView1.SelectedRows[0].DataBoundItem;

                // Получаем выбранную услугу
                var selectedService = (Service)dataGridView2.SelectedRows[0].DataBoundItem;

                if (selectedOrder != null && selectedService != null)
                {
                    try
                    {
                        // Проверка на соответствие типа устройства между заказом и услугой
                        if (selectedOrder.DeviceType != selectedService.DeviceType)
                        {
                            throw new InvalidOperationException("Тип устройства заказа и услуги не совпадают. Диагностику невозможно добавить.");
                        }

                        // Проверка, что строка стоимости заказа не пуста. Если пуста - присваиваем 0
                        float orderPrice = 0;
                        if (!string.IsNullOrWhiteSpace(selectedOrder.Price))
                        {
                            // Замена запятой на точку для корректного преобразования
                            string orderPriceStr = selectedOrder.Price.Replace(",", ".");

                            // Пробуем преобразовать строку стоимости заказа в float, используя CultureInfo.InvariantCulture
                            if (!float.TryParse(orderPriceStr, NumberStyles.Float, CultureInfo.InvariantCulture, out orderPrice))
                            {
                                throw new FormatException("Неверный формат стоимости заказа.");
                            }
                        }

                        // Пробуем преобразовать строку стоимости услуги в float, заменяя запятую на точку
                        string servicePriceStr = selectedService.Price.Replace(",", ".");
                        if (!float.TryParse(servicePriceStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float servicePrice))
                        {
                            throw new FormatException("Неверный формат стоимости услуги.");
                        }

                        // Суммируем стоимость заказа и услуги
                        float updatedPrice = orderPrice + servicePrice;

                        // Обновляем стоимость заказа, преобразуя её обратно в строку
                        selectedOrder.Price = updatedPrice.ToString("F2", CultureInfo.InvariantCulture);

                        // Добавляем услугу к заказу через объект Engineer
                        engineer.AddServiceToOrder(selectedOrder.OrderId, selectedService.ServiceId);

                        // Устанавливаем флаг диагностики
                        engineer.MarkOrderAsDiagnosed(selectedOrder.OrderId);

                        MessageBox.Show("Диагностика успешно добавлена к заказу и статус диагностики обновлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ и услугу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Предположим, что колонка с OrderId скрыта, но она присутствует в dataGridView
                // Получаем значение OrderId из выбранной строки
                int selectedOrderId = (int)dataGridView1.SelectedRows[0].Cells["OrderId"].Value;

                // Находим заказ с этим OrderId
                var selectedOrder = engineer.GetAssignedOrders().FirstOrDefault(order => order.OrderId == selectedOrderId);

                if (selectedOrder != null)
                {
                    // Очищаем предыдущие данные в dataGridView3
                    dataGridView3.DataSource = null;

                    // Устанавливаем список диагнозов в качестве источника данных для dataGridView3
                    dataGridView3.DataSource = selectedOrder.Diagnoses.Select(d => new
                    {
                        //d.ServiceId,
                        d.ServiceName,
                        d.Price
                    }).ToList();
                    dataGridView3.Columns["ServiceName"].HeaderText = "Ремонтная услуга";
                    dataGridView3.Columns["Price"].HeaderText = "Стоимость";
                }
                else
                {
                    MessageBox.Show("Заказ с данным ID не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для отображения диагнозов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
