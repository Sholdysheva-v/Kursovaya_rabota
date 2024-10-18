using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая_работа
{
    // Класс для хранения данных о доступах
    public class AccessData
    {
        public int AccessCode { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }

    
}
