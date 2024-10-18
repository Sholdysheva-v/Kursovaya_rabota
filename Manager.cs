using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая_работа
{
    public class Manager : User
    {
        private DataManager<Order> orderManager; // Экземпляр DataManager для управления пользователями
        private DataManager<AccessData> accessManager; // Экземпляр DataManager для управления пользователями
        public Manager(User user)
        : base(user.Id, user.Login, user.Password, user.Surname, user.Name, user.Patronymic, user.ContactPhone, user.Email) // Вызываем конструктор базового класса
        {
            orderManager = new DataManager<Order>("orders.json"); // для работы с зазказами
            accessManager = new DataManager<AccessData>("access.json"); // для работы с зазказами
        }

        // Метод для добавления нового заказа
        public void AddOrder(string surname, string name, string patronymic,
                             string contactPhone, DeviceType deviceType, string model, string serialNumber,
                             string externalConditionComment, string issue)
        {
            // Создание нового заказа
            int newOrderId = GenerateUniqueId(); // Генерация нового OrderId
            DateTime currentDate = DateTime.Now;
            // Получение списка всех заказов и доступа
            var orders = orderManager.Items;            // Получаем список всех заказов
            var accessData = accessManager.Items;       // Получаем список всех данных о доступах

            // Назначение мастера с минимальным количеством незавершенных заказов
            int masterId = AssignMaster(orders, accessData);

            Order newOrder = new Order(
                newOrderId,           // Генерация нового ID заказа
                this.Id,              // Присваивание ID текущего менеджера
                masterId,             // ID мастера
                surname,
                name,
                patronymic,
                contactPhone,
                deviceType,
                model,
                serialNumber,
                externalConditionComment,
                issue,
                false,                // Статус заказа не завершен
                currentDate,          // Дата подачи заявки
                false,                // Диагностика не завершена        
                string.Empty          // Цена пока не указана
            );

            // Добавление нового заказа в список через DataManager
            orderManager.AddItem(newOrder);
        }

        // Метод для генерации уникального ID
        private int GenerateUniqueId()
        {
            // Получение списка всех существующих пользователей
            var existingUsers = orderManager.Items;
            // Генерация нового ID
            int newId;
            Random random = new Random();
            do
            {
                newId = random.Next(1, int.MaxValue); // Генерация случайного числа в диапазоне от 1 до int.MaxValue
            } while (existingUsers.Any(user => user.OrderId == newId)); // Проверка на уникальность ID
            return newId;
        }
        // Функция для назначения мастера с минимальным количеством незавершенных заказов
        public int AssignMaster(List<Order> orders, List<AccessData> accessData)
        {
            // Находим мастеров (код доступа = 2)
            var masters = accessData
                .Where(ad => ad.AccessCode == 2)
                .SelectMany(ad => ad.UserIds)
                .ToList();

            if (!masters.Any())
                throw new Exception("Нет доступных мастеров.");

            // Получаем количество незавершенных заказов для каждого мастера
            var masterOrdersCount = masters
                .Select(masterId => new
                {
                    MasterId = masterId,
                    IncompleteOrdersCount = orders.Count(order =>
                        order.MasterId == masterId &&
                        !order.IsCompleted) // Считаем заказы, которые не завершены
                });

            // Находим минимальное количество незавершенных заказов
            int minOrders = masterOrdersCount.Min(m => m.IncompleteOrdersCount);

            // Получаем всех мастеров с минимальным количеством незавершенных заказов
            var mastersWithMinOrders = masterOrdersCount
                .Where(m => m.IncompleteOrdersCount == minOrders)
                .Select(m => m.MasterId)
                .ToList();

            // Выбираем случайного мастера из списка
            Random random = new Random();
            return mastersWithMinOrders[random.Next(mastersWithMinOrders.Count)];
        }

        public void MarkOrderAsCompleted(int orderId)
        {
            Order order = orderManager.FindItem(o => o.OrderId == orderId); // Поиск заказа по ID
            if (order != null)
            {
                order.MarkAsCompleted(); // Отмечаем заказ как завершенный

                orderManager.SaveItems();// Сохраняем изменения в файл
            }
            else
            {
                throw new ArgumentException("Заказ не найден или не принадлежит этому инженеру.");
            }
        }
    }
}   
