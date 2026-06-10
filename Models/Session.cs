using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Models
{
    // Модель сеанса
    public class Session
    {
        public int Id { get; set; }
        public int FilmId { get; set; } // Связь с фильмом
        public string FilmName { get; set; } // Для отображения
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public string Hall { get; set; }
        public int FreeSeats { get; set; }
    }
}
