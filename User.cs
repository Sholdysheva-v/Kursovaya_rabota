using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Курсовая_работа
{
    //  класс User — базовый класс для всех пользователей системы
    public class User
    {
        // Свойства пользователя
        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string Surname { get; private set; }
        public string Name { get; private set; }
        public string Patronymic { get; private set; }
        public string ContactPhone { get; private set; }
        public string Email { get; private set; }


        // Конструктор класса User
        public User(int id, string login, string password, string surname, string name, string patronymic, string contactPhone, string email)
        {
            Id = id;
            Login = login;
            Password = password;

            if (name.Length >= 40)
            {
                throw new ArgumentOutOfRangeException("ФИО не может быть больше 40 символов.");
            }
            foreach (char c in name)
            {
                if ((char.IsDigit(c) || char.IsPunctuation(c)) && c != ' ')
                {
                    throw new ArgumentException("Поле Фамилия содержит недопустимые символы.");
                }
            }
            if (name.Length == 0)
            {
                throw new ArgumentOutOfRangeException("Поле Фамилия не может быть пустым.");
            }
            Name = name;

            foreach (char c in surname)
            {
                if ((char.IsDigit(c) || char.IsPunctuation(c)) && c != ' ')
                {
                    throw new ArgumentException("Поле Имя содержит недопустимые символы.");
                }
            }
            if (surname.Length == 0)
            {
                throw new ArgumentOutOfRangeException("Поле Имя не может быть пустым.");
            }
            Surname = surname;

            foreach (char c in patronymic)
            {
                if ((char.IsDigit(c) || char.IsPunctuation(c)) && c != ' ')
                {
                    throw new ArgumentException("Поле Отчество содержит недопустимые символы.");
                }
            }
            if (patronymic.Length == 0)
            {
                throw new ArgumentOutOfRangeException("Поле Отчество не может быть пустым.");
            }
            Patronymic = patronymic;

            foreach (char c in contactPhone)
            {
                if (!char.IsDigit(c) && c != ' ')
                {
                    throw new ArgumentException("Поле Контактный номер содержит недопустимые символы.");
                }
            }
            if (contactPhone.Length == 0)
            {
                throw new ArgumentOutOfRangeException("Поле Контактный номер не может быть пустым.");
            }
            ContactPhone = contactPhone;

            Email = email;

        }
    }
}