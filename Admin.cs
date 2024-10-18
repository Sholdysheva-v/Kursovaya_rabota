using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая_работа
{
    public class Admin : User 
    {
        private DataManager<User> userManager; // Экземпляр DataManager для управления пользователями
        private DataManager<AccessData> accessManager; // Экземпляр DataManager для работы с данными о доступах

        // Конструктор класса Admin
        public Admin(int id, string login, string password, string surname, string name, string patronymic, string contactPhone, string email)
            : base(id, login, password, surname, name, patronymic, contactPhone, email)
        {
            userManager = new DataManager<User>("users.json"); // Инициализация для работы с сотрудниками
            accessManager = new DataManager<AccessData>("access.json"); // Инициализация  для работы с ролями сотрудников
        }

        // Конструктор Admin, который принимает объект User
        public Admin(User user)
            : base(user.Id, user.Login, user.Password, user.Surname, user.Name, user.Patronymic, user.ContactPhone, user.Email) // Вызываем конструктор базового класса
        {
            userManager = new DataManager<User>("users.json"); // Инициализация для работы с сотрудниками
            accessManager = new DataManager<AccessData>("access.json"); // Инициализация  для работы с ролями сотрудников
        }

        public void AddNewUser(string login, string password, string surname, string name, string patronymic, string contactPhone, string email, int accessCode)
        {
            // Проверка на уникальность логина, чтобы избежать дублирования пользователей
            if (userManager.FindItem(user => user.Login == login) != null)
            {
                throw new ArgumentException("Пользователь с таким логином уже существует.");
            }
            int id = GenerateUniqueId();

            // Создание нового объекта User
            var newUser = new User (id, login, password, surname, name, patronymic, contactPhone, email);

            // Добавление нового пользователя в список с ролями сотрудников
            AddUserAccess(accessCode, id);

            // Добавление нового пользователя в список через DataManager
            userManager.AddItem(newUser);
        }

        // Метод для генерации уникального ID
        private int GenerateUniqueId()
        {
            // Получение списка всех существующих пользователей
            var existingUsers = userManager.Items;
            // Генерация нового ID
            int newId;
            Random random = new Random();
            do
            {
                newId = random.Next(1, int.MaxValue); // Генерация случайного числа в диапазоне от 1 до int.MaxValue
            } while (existingUsers.Any(user => user.Id == newId)); // Проверка на уникальность ID
            return newId;
        }

        // метод для получения списка пользователей с указанным кодом доступа
        public List<int> GetUsersWithAccess(int accessCode) //этот спсиок пойдет если администартор выберет фильтр о том какой сколько каких сотрудников работает
        {
            var accessData = accessManager.FindItem(ad => ad.AccessCode == accessCode); // поиск данных о доступе по коду
            return accessData?.UserIds ?? new List<int>(); // возвращение списка идентификаторов пользователей
        }

        // метод для добавления пользователя с указанным кодом доступа //при добавлении нового пользователя указать его роль и его айди
        public void AddUserAccess(int accessCode, int userId)
        {
            var accessData = accessManager.FindItem(ad => ad.AccessCode == accessCode); // поиск данных о доступе по коду
            if (accessData == null)
            {
                accessData = new AccessData { AccessCode = accessCode }; // создание новых данных о доступе, если не найдены
                accessManager.AddItem(accessData); // добавление новых данных в список
            }
            if (!accessData.UserIds.Contains(userId))
            {
                accessData.UserIds.Add(userId); // Добавление идентификатора пользователя в список
                accessManager.SaveItems(); // Сохранение изменений в файл
            }
        }

        // Метод для удаления пользователя из данных о доступе. При удалении пользователя удалить его и из таблиц с кодом
        public void RemoveUserAccess(int accessCode, int userId)
        {
            var accessData = accessManager.FindItem(ad => ad.AccessCode == accessCode); // Поиск данных о доступе по коду
            if (accessData != null)
            {
                accessData.UserIds.Remove(userId); // Удаление идентификатора пользователя из списка
                if (!accessData.UserIds.Any())
                {
                    accessManager.RemoveItem(accessData); // Удаление данных о доступе, если список пользователей пуст
                }
                else
                {
                    accessManager.SaveItems(); // Сохранение изменений в файл
                }
            }
        }
        // Метод для удаления пользователя
        // Метод для удаления пользователя
        public void RemoveUser(int userId)
        {
            // Проверяем, есть ли у пользователя незавершённые заказы
            var ordersManager = new DataManager<Order>("orders.json");
            var incompleteOrders = ordersManager.Items.Any(order => order.MasterId == userId && !order.IsCompleted);

            if (incompleteOrders)
            {
                throw new InvalidOperationException("У сотрудника есть незавершённые заказы. Удаление невозможно.");
            }

            // Получаем пользователя по ID
            var user = userManager.FindItem(u => u.Id == userId);
            if (user != null)
            {
                // Удаляем пользователя из всех данных о доступе
                foreach (var accessData in accessManager.Items)
                {
                    if (accessData.UserIds.Contains(userId))
                    {
                        RemoveUserAccess(accessData.AccessCode, userId);
                    }
                }

                // Удаляем пользователя из списка пользователей
                userManager.RemoveItem(user);
                userManager.SaveItems(); // Сохранение изменений в файл
            }
            else
            {
                throw new ArgumentException($"Пользователь с ID {userId} не найден.");
            }
        }


    }
}
