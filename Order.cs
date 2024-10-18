using System;
using System.Collections.Generic;
using System.Linq;

namespace Курсовая_работа
{
    // Перечисление для типа устройства
    public enum DeviceType
    {
        Смартфон,
        Ноутбук,
        Планшет,
        Электронная_книга,
    }

    // Класс Order
    public class Order
    {
        public int OrderId { get; private set; }
        public int ManagerId { get; private set; }
        public int MasterId { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string Patronymic { get; private set; }
        public string ContactPhone { get; private set; }
        public DeviceType DeviceType { get; private set; }
        public string Model { get; private set; }
        public string SerialNumber { get; private set; }
        public string ExternalConditionComment { get; private set; }
        public string Issue { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime DateOfInquiry { get; private set; }
        public bool IsDiagnosed { get; private set; }
        public List<Service> Diagnoses { get; private set; } // Список диагностик
        public string Price { get; set; }

        // Конструктор класса Order
        public Order(int orderId, int managerId, int masterId, string lastName, string firstName, string patronymic,
                     string contactPhone, DeviceType deviceType, string model, string serialNumber,
                     string externalConditionComment, string issue, bool isCompleted, DateTime dateOfInquiry,
                     bool isDiagnosed, string price)
        {
            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId), "Идентификатор заказа должен быть положительным числом.");
            OrderId = orderId;

            if (managerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(managerId), "Идентификатор менеджера должен быть положительным числом.");
            ManagerId = managerId;

            if (masterId <= 0)
                throw new ArgumentOutOfRangeException(nameof(masterId), "Идентификатор мастера должен быть положительным числом.");
            MasterId = masterId;

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length >= 40)
                throw new ArgumentOutOfRangeException(nameof(lastName), "Поле Фамилия не может быть пустым или содержать больше 40 символов.");
            LastName = lastName;

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentOutOfRangeException(nameof(firstName), "Поле Имя не может быть пустым.");
            FirstName = firstName;

            if (string.IsNullOrWhiteSpace(patronymic))
                throw new ArgumentOutOfRangeException(nameof(patronymic), "Поле Отчество не может быть пустым.");
            Patronymic = patronymic;

            if (string.IsNullOrWhiteSpace(contactPhone) || contactPhone.Any(c => !char.IsDigit(c)))
                throw new ArgumentException("Поле Контактный номер содержит недопустимые символы.");
            ContactPhone = contactPhone;

            if (!Enum.IsDefined(typeof(DeviceType), deviceType))
                throw new ArgumentOutOfRangeException(nameof(deviceType), "Поле Устройство содержит недопустимое значение.");
            DeviceType = deviceType;

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentOutOfRangeException(nameof(model), "Поле Модель не может быть пустым.");
            Model = model;

            if (string.IsNullOrWhiteSpace(serialNumber))
                throw new ArgumentOutOfRangeException(nameof(serialNumber), "Поле Серийный номер не может быть пустым.");
            SerialNumber = serialNumber;

            if (string.IsNullOrWhiteSpace(externalConditionComment))
                throw new ArgumentOutOfRangeException(nameof(externalConditionComment), "Поле 'Комментарий о внешнем состоянии' не может быть пустым.");
            ExternalConditionComment = externalConditionComment;

            if (string.IsNullOrWhiteSpace(issue))
                throw new ArgumentOutOfRangeException(nameof(issue), "Поле 'Проблема заявленная заказчиком' не может быть пустым.");
            Issue = issue;

            IsCompleted = isCompleted;
            DateOfInquiry = dateOfInquiry;
            IsDiagnosed = isDiagnosed;
            Diagnoses = new List<Service>(); // Инициализация списка диагностик
            Price = price;
        }

        // Метод для добавления услуги к заказу
        public void AddService(Service service)
        {
            // Проверяем, что услуга еще не добавлена
            if (!Diagnoses.Any(s => s.ServiceId == service.ServiceId))
            {
                Diagnoses.Add(service); // Добавляем услугу в список
            }
            else
            {
                throw new ArgumentException("Услуга уже добавлена к заказу.");
            }
        }

      


        //// Метод для добавления диагностики к заказу
        //public void AddDiagnosis(Service diagnosis)
        //{
        //    if (!Diagnoses.Any(d => d.ServiceId == diagnosis.ServiceId))
        //    {
        //        Diagnoses.Add(diagnosis);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Диагностика уже добавлена к заказу.");
        //    }
        //}

        // Метод для установки флага завершенности заказа
        public void MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
            }
            else
            {
                throw new InvalidOperationException("Заказ уже завершен.");
            }
        }

        // Метод для установки флага диагностики
        public void MarkAsDiagnosed()
        {
            if (!IsDiagnosed)
            {
                IsDiagnosed = true;

            }
            //else
            //{
            //    throw new InvalidOperationException("Диагностика уже выполнена.");
            //}
        }
        public void UpdatePrice(float additionalAmount)
        {
            // Конвертируем строку Price в float
            if (float.TryParse(Price, out float currentPrice))
            {
                // Суммируем текущее значение с переданным аргументом
                float newPrice = currentPrice + additionalAmount;

                // Преобразуем обратно в строку и сохраняем
                Price = newPrice.ToString("F2"); // Сохранение с двумя знаками после запятой
            }
            else
            {
                throw new InvalidOperationException("Не удалось преобразовать поле Price в число.");
            }
        }

    }

    // Класс Service для представления услуг
    public class Service
    {
        public int ServiceId { get; private set; }
        public DeviceType DeviceType { get; private set; }
        public string ServiceName { get; private set; }
        public string Price { get; private set; }

        public Service(int serviceId, DeviceType deviceType, string serviceName, string price)
        {
            if (serviceId <= 0)
                throw new ArgumentOutOfRangeException(nameof(serviceId), "Идентификатор услуги должен быть положительным числом.");
            ServiceId = serviceId;

            if (!Enum.IsDefined(typeof(DeviceType), deviceType))
                throw new ArgumentOutOfRangeException(nameof(deviceType), "Недопустимый тип устройства.");
            DeviceType = deviceType;

            if (string.IsNullOrWhiteSpace(serviceName))
                throw new ArgumentOutOfRangeException(nameof(serviceName), "Поле Наименование услуги не может быть пустым.");
            ServiceName = serviceName;

            if (string.IsNullOrWhiteSpace(price) || !float.TryParse(price, out float parsedPrice))
                throw new ArgumentException("Поле Стоимость содержит недопустимые символы или не является числом.");

            // Сохраняем цену в виде строки с двумя знаками после запятой
            Price = parsedPrice.ToString("F2");
        }
    }
}
