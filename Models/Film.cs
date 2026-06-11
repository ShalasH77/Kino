using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Models
{
    // Модель фильма
    namespace Kino.Models  // Пространство имён для моделей данных
    {
        // Модель фильма - представляет один фильм в базе данных
        // Реализует INotifyPropertyChanged для обновления UI при изменении свойств
        public class Film : INotifyPropertyChanged  // Наследуем интерфейс уведомлений
        {
            // Приватные поля (хранят реальные значения свойств)

            // Уникальный идентификатор фильма
            private int _id;
            // Название фильма
            private string _name;
            // Жанр фильма (комедия, драма, фантастика и т.д.)
            private string _genre;
            // Имя режиссёра фильма
            private string _director;
            // Продолжительность фильма в минутах
            private int _duration;
            // Возрастное ограничение (0+, 6+, 12+, 16+, 18+)
            private string _ageRating;
            // Цена билета на этот фильм в рублях
            private decimal _price;


            // Публичные свойства (доступны извне)


            // Уникальный идентификатор фильма
            // Используется для поиска и связывания с сеансами
  
            public int Id
            {
                get => _id;  // get - возвращает значение поля _id
                set          // set - устанавливает новое значение
                {
                    _id = value;  // value - ключевое слово с новым значением
                    OnPropertyChanged(nameof(Id));  // Уведомляем UI об изменении
                }
            }

            // Название фильма
            public string Name
            {
                get => _name;
                set
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }

            // Жанр фильма
            public string Genre
            {
                get => _genre;
                set
                {
                    _genre = value;
                    OnPropertyChanged(nameof(Genre));
                }
            }

            // Имя режиссёра фильма
            public string Director
            {
                get => _director;
                set
                {
                    _director = value;
                    OnPropertyChanged(nameof(Director));
                }
            }

            // Продолжительность фильма в минутах
            // Валидация: от 30 до 300 минут
            public int Duration
            {
                get => _duration;
                set
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }

            // Возрастное ограничение
            // Определяет, кому можно смотреть фильм
            public string AgeRating
            {
                get => _ageRating;
                set
                {
                    _ageRating = value;
                    OnPropertyChanged(nameof(AgeRating));
                }
            }

            // Валидация: от 100 до 2000 рублей

            public decimal Price
            {
                get => _price;
                set
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }

            // Реализация INotifyPropertyChanged

            // Событие, которое срабатывает при изменении любого свойства
            // WPF подписывается на это событие и обновляет UI автоматически
            public event PropertyChangedEventHandler PropertyChanged;

            // Метод для вызова события PropertyChanged
            // Уведомляет UI о том, что свойство изменилось и нужно обновить отображение
            protected void OnPropertyChanged(string propertyName)
            {
                // PropertyChanged?.Invoke - вызывает событие, если есть подписчики
                // ?. - оператор условного доступа (не вызывает, если PropertyChanged == null)
                // this - текущий объект (экземпляр Film)
                // new PropertyChangedEventArgs(propertyName) - аргументы с именем свойства
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }