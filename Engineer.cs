using System;
using System.Collections.Generic;
using System.Linq;

namespace Курсовая_работа
{
    public class Engineer : User
    {
        private DataManager<Order> orderManager; // Экземпляр DataManager для управления заказами
        private DataManager<Service> serviceManager; // Экземпляр DataManager для управления пользователями

        // Конструктор класса Engineer
        public Engineer(int id, string login, string password, string surname, string name, string patronymic, string contactPhone, string email)
            : base(id, login, password, surname, name, patronymic, contactPhone, email)
        {
            orderManager = new DataManager<Order>("orders.json"); // Инициализация для работы с заказами
            serviceManager = new DataManager<Service>("services.json"); // Инициализация для работы с заказами
        }

        // Конструктор Engineer, который принимает объект User
        public Engineer(User user)
            : base(user.Id, user.Login, user.Password, user.Surname, user.Name, user.Patronymic, user.ContactPhone, user.Email)
        {
            orderManager = new DataManager<Order>("orders.json");
            serviceManager = new DataManager<Service>("services.json"); // Инициализация для работы с заказами

        }

        // Метод для получения списка заказов, назначенных инженеру
        public List<Order> GetAssignedOrders()
        {
            return orderManager.Items.FindAll(order => order.MasterId == this.Id); // Возвращаем заказы, назначенные текущему инженеру
        }
        // Добавить услугу в список услуг
        public void AddNewService(DeviceType deviceType, string serviceName, string price)
        {
            int serviceId = GenerateUniqueServiceId(); 

            // Создаем новый объект класса Service
            var newService = new Service(serviceId, deviceType, serviceName, price);

            // Добавляем новую услугу в DataManager и сохраняем изменения
            serviceManager.AddItem(newService);

        }
        // Метод для удаления услуги в списке услуг
        public void RemoveService(int serviceId)
        {
            var serviceToRemove = serviceManager.FindItem(service => service.ServiceId == serviceId);
            if (serviceToRemove != null)
            {
                serviceManager.RemoveItem(serviceToRemove);
            }
            else
            {
                throw new ArgumentException("Услуга с указанным ID не найдена.");
            }
        }

        // добавить услугу в список
        public void AddServiceToOrder(int orderId, int serviceId)
        {
            // Находим заказ по его ID
            var order = orderManager.FindItem(o => o.OrderId == orderId);

            if (order != null && order.MasterId == this.Id)
            {
                // Находим услугу по её ID
                var service = serviceManager.FindItem(s => s.ServiceId == serviceId);

                if (service != null)
                {
                    // Добавляем услугу диагностики к заказу
                    order.AddService(service);
                    orderManager.SaveItems(); // Сохраняем изменения в заказах
                }
                else
                {
                    throw new ArgumentException("Услуга не найдена.");
                }
            }
            else
            {
                throw new ArgumentException("Заказ не найден или не принадлежит этому инженеру.");
            }
        }




        // Метод для получения списка всех услуг
        public List<Service> GetAllServices()
        {
            return serviceManager.Items; // Возвращаем все услуги, хранящиеся в DataManager
        }

        // Метод для генерации уникального ID для услуги
        private int GenerateUniqueServiceId()
        {
            // Получение списка всех существующих услуг
            var existingServices = serviceManager.Items;

            // Генерация нового ID
            int newId;
            Random random = new Random();

            do
            {
                newId = random.Next(1, int.MaxValue); // Генерация случайного числа в диапазоне от 1 до int.MaxValue
            } while (existingServices.Any(service => service.ServiceId == newId)); // Проверка на уникальность ID
            return newId;
        }

        public void MarkOrderAsDiagnosed(int orderId)
        {
            Order order = orderManager.FindItem(o => o.OrderId == orderId); // Поиск заказа по ID
            if (order != null && order.MasterId == this.Id)
            {
                order.MarkAsDiagnosed(); // Отмечаем заказ как завершенный
                                         
                orderManager.SaveItems();// Сохраняем изменения в файл
            }
            else
            {
                throw new ArgumentException("Заказ не найден или не принадлежит этому инженеру.");
            }
        }
    }
}
