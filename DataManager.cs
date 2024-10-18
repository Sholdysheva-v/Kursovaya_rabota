using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Курсовая_работа
{
    // Универсальный класс для работы с любыми типами объектов
    public class DataManager<T> where T : class // T - это любой ссылочный тип
    {
        private List<T> items; // Приватное поле для хранения списка объектов
        private string jsonFilePath; // Приватное поле для хранения пути к JSON файлу

        public DataManager(string filePath) // Публичный конструктор класса, принимающий путь к JSON файлу
        {
            jsonFilePath = filePath; // Присваивание пути к JSON файлу в поле jsonFilePath
            items = DeserializeItems(); // Вызов метода DeserializeItems для загрузки списка объектов из файла
        }

        // Приватный метод для десериализации списка объектов из JSON файла
        private List<T> DeserializeItems()
        {
            try
            {
                if (File.Exists(jsonFilePath)) // Проверка существования файла
                {
                    string json = File.ReadAllText(jsonFilePath); // Чтение содержимого файла

                    // Проверяем, что содержимое файла не пустое
                    if (!string.IsNullOrEmpty(json))
                    {
                        return JsonConvert.DeserializeObject<List<T>>(json); // Десериализация JSON строки в список объектов
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки (можно заменить на нужную вам реализацию)
                Console.WriteLine($"Ошибка при десериализации: {ex.Message}");
            }

            // Возвращаем пустой список в случае любой ошибки
            return new List<T>();
        }

        public List<T> Items // Публичное свойство, возвращающее список объектов ???
        {
            get { return items; }
        }

        // Метод для добавления нового объекта в список и сохранения в файл
        public void AddItem(T newItem)
        {
            items.Add(newItem); // Добавляем новый объект в список
            SaveItems(); // Сохраняем изменения в файл
        }

        // Метод для сохранения списка объектов в JSON файл
        public void SaveItems()
        {
            string json = JsonConvert.SerializeObject(items, Formatting.Indented); // Преобразование списка объектов в JSON строку
            File.WriteAllText(jsonFilePath, json); // Запись JSON строки в файл
        }

        // Метод для поиска объекта по предикату
        public T FindItem(Predicate<T> match)
        {
            return items.Find(match); // Поиск объекта, удовлетворяющего предикату
        }

        // Метод для удаления объекта из списка
        public void RemoveItem(T itemToRemove)
        {
            items.Remove(itemToRemove); // Удаление объекта из списка
            SaveItems(); // Сохранение изменений в файл
        }

    }
}
