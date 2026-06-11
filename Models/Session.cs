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
        public int Id { get; set; } // Используется для поиска и удаления конкретного сеанса
        public int FilmId { get; set; } // Связывает сеанс с конкретным фильмом
        public string FilmName { get; set; } // Для отображения названия фильма 
        public DateTime Date { get; set; } // Дата проведения сеанса
        public TimeSpan StartTime { get; set; } //Время начала сеанса
        public string Hall { get; set; } // Название кинозала
        public int FreeSeats { get; set; } //Количество свободных мест в зале на этот сеанс
    }
}
