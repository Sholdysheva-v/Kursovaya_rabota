using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Курсовая_работа
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Admin admin = new Admin(123, "Admin_1", "Admin_1", "Шолдышева", "Виктория", "Денисовна", "89138942103", "viktoria220604@gmail.com");
            //// Добавление нового пользователя в список через DataManager
            //admin.AddNewUser("Admin_2", "Admin_2", "Иванова", "Екатерина", "Андреевна", "89234356708", "lisa@gmail.com", 0);
            Application.Run(new MainForm());
        }
    }
}
