using Kino.Models;
using Kino.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kino
{

    // Главное окно приложения
    // Содержит две вкладки: Фильмы и Сеансы
    public partial class MainWindow : Window
    {

        // Хранилища данных (работают как база данных в памяти)

        // Список всех фильмов
        private List<Film> _films = new List<Film>();

        // Список всех сеансов
        private List<Session> _sessions = new List<Session>();

        // Следующий доступный ID для нового фильма
        // Начинается с 1 и увеличивается при добавлении
        private int _nextFilmId = 1;

        // Следующий доступный ID для нового сеанса
        private int _nextSessionId = 1;

        // Кнструктор 

        // Конструктор главного окна
        // Вызывается при создании окна
        public MainWindow()
        {
            InitializeComponent();      // Загружаем интерфейс из XAML
            LoadSampleData();           // Загружаем тестовые данные для демонстрации
            RefreshFilms();             // Отображаем фильмы в таблице
            RefreshSessions();          // Отображаем сеансы в таблице
        }

        // Загрузка текстовых данных 

        // Загружает тестовые данные для демонстрации работы приложения
        // Создаёт несколько фильмов и сеансов по умолчанию
        private void LoadSampleData()
        {
            // СОЗДАНИЕ ТЕСТОВЫХ ФИЛЬМОВ 
            _films = new List<Film>
            {
                new Film {
                    Id = _nextFilmId++,           // ID = 1, затем увеличиваем
                    Name = "1+1",                 // Название
                    Genre = "Комедия",            // Жанр
                    Director = "Оливье Накаш",    // Режиссёр
                    Duration = 120,               // Длительность 120 минут
                    AgeRating = "18+",            // Возрастное ограничение
                    Price = 350                   // Цена 350 рублей
                },
                new Film {
                    Id = _nextFilmId++,           // ID = 2
                    Name = "Волк с Уолл-стрит",
                    Genre = "Биография",
                    Director = "Мартин Скорсезе",
                    Duration = 180,
                    AgeRating = "18+",
                    Price = 400
                },
                new Film {
                    Id = _nextFilmId++,           // ID = 3
                    Name = "Король Лев",
                    Genre = "Мультфильм",
                    Director = "Джон Фавро",
                    Duration = 118,
                    AgeRating = "6+",
                    Price = 250
                }
            };

            // Создание текстовых сеансов
            _sessions = new List<Session>
            {
                new Session {
                    Id = _nextSessionId++,        // ID = 1
                    FilmId = 1,                  // Ссылка на фильм с ID=1 (Король Лев)
                    FilmName = "Король Лев",     // Название (дублируется для отображения)
                    Date = DateTime.Today.AddDays(1),  // Завтра
                    StartTime = new TimeSpan(9, 0, 0), // 09:00 утра
                    Hall = "Зал 1",
                    FreeSeats = 45
                },
                new Session {
                    Id = _nextSessionId++,        // ID = 2
                    FilmId = 1,                  // Тоже фильм "1+1"
                    FilmName = "1+1",
                    Date = DateTime.Today.AddDays(1),
                    StartTime = new TimeSpan(19, 0, 0), // 19:00 вечера
                    Hall = "Зал 1",
                    FreeSeats = 50
                },
                new Session {
                    Id = _nextSessionId++,        // ID = 3
                    FilmId = 1,
                    FilmName = "1+1",
                    Date = DateTime.Today.AddDays(1),
                    StartTime = new TimeSpan(21, 30, 0), // 21:30
                    Hall = "Зал 1",
                    FreeSeats = 45
                },
                new Session {
                    Id = _nextSessionId++,        // ID = 4
                    FilmId = 2,                  // Фильм "Волк с Уолл-стрит"
                    FilmName = "Волк с Уолл-стрит",
                    Date = DateTime.Today.AddDays(2), // Послезавтра
                    StartTime = new TimeSpan(18, 0, 0), // 18:00
                    Hall = "Зал 2",
                    FreeSeats = 60
                }
            };
        }

        // Раздел: Управление фильмами

        #region Фильмы
        /// Обновляет таблицу фильмов
        /// Перезагружает источник данных DataGrid
        private void RefreshFilms()
        {
            dgFilms.ItemsSource = null;       // Сбрасываем старый источник
            dgFilms.ItemsSource = _films;     // Устанавливаем новый (список фильмов)
            // WPF автоматически перерисует таблицу
        }

        // Обработчик кнопки "Добавить фильм"
        // Открывает диалог создания нового фильма
        private void BtnAddFilm_Click(object sender, RoutedEventArgs e)
        {
            // Создаём диалоговое окно для добавления фильма
            var dialog = new FilmDialog();

            // ShowDialog() - показывает окно модально (блокирует родительское окно)
            // == true - пользователь нажал "Сохранить"
            if (dialog.ShowDialog() == true)
            {
                // Получаем созданный фильм из диалога
                var film = dialog.GetFilm();

                // Присваиваем новый уникальный ID
                film.Id = _nextFilmId++;

                // Добавляем в список
                _films.Add(film);

                // Обновляем таблицу
                RefreshFilms();
            }
        }

        // Обработчик кнопки "Редактировать фильм"
        // Открывает диалог редактирования выбранного фильма
        private void BtnEditFilm_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный в таблице фильм
            var selectedFilm = dgFilms.SelectedItem as Film;

            // Если ничего не выбрано - показываем предупреждение
            if (selectedFilm == null)
            {
                MessageBox.Show("Выберите фильм для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;  // Выходим из метода
            }

            // Создаём диалог редактирования, передавая выбранный фильм
            var dialog = new FilmDialog(selectedFilm);

            if (dialog.ShowDialog() == true)
            {
                // Получаем отредактированный фильм
                var updatedFilm = dialog.GetFilm();

                // Сохраняем оригинальный ID
                updatedFilm.Id = selectedFilm.Id;

                // Находим индекс фильма в списке по ID
                var index = _films.FindIndex(f => f.Id == selectedFilm.Id);

                // Заменяем старый фильм новым
                _films[index] = updatedFilm;

                // Обновляем таблицу
                RefreshFilms();
            }
        }

        // Обработчик кнопки "Удалить фильм"
        // Удаляет выбранный фильм и все связанные с ним сеансы
        private void BtnDeleteFilm_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный фильм
            var selectedFilm = dgFilms.SelectedItem as Film;

            if (selectedFilm == null)
            {
                MessageBox.Show("Выберите фильм для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Подтвержденные удаления 
            // YesNo - кнопки "Да" и "Нет"
            var result = MessageBox.Show($"Удалить запись о фильме \"{selectedFilm.Name}\"?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)  // Пользователь нажал "Да"
            {
                // Удаляем связанные сеансы
                // Находим все сеансы этого фильма (где FilmId совпадает)
                var sessionsToDelete = _sessions.Where(s => s.FilmId == selectedFilm.Id).ToList();

                // Удаляем каждый найденный сеанс
                foreach (var session in sessionsToDelete)
                {
                    _sessions.Remove(session);
                }

                // Удаляем сам фильм 
                _films.Remove(selectedFilm);

                // обновляем обе таблицы 
                RefreshFilms();      // Обновляем таблицу фильмов
                RefreshSessions();   // Обновляем таблицу сеансов
            }
        }

        // Обработчик кнопки "Обновить список фильмов"
        // Просто перезагружает таблицу
        private void BtnRefreshFilms_Click(object sender, RoutedEventArgs e)
        {
            RefreshFilms();
        }

        // Обработчик двойного клика по строке в таблице фильмов
        // Открывает редактирование (удобно вместо кнопки)
        private void DgFilms_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BtnEditFilm_Click(sender, null);  // Вызываем редактирование
        }

        #endregion

        // Раздел: Управление сеансами 

        #region Сеансы

        // Обновляет таблицу сеансов
        private void RefreshSessions()
        {
            dgSessions.ItemsSource = null;
            dgSessions.ItemsSource = _sessions;
        }

        // Обработчик кнопки "Добавить сеанс"
        // Открывает диалог создания нового сеанса
        private void BtnAddSession_Click(object sender, RoutedEventArgs e)
        {
            // Передаём список фильмов, чтобы выбрать фильм для сеанса
            var dialog = new SessionDialog(_films);

            if (dialog.ShowDialog() == true)
            {
                var session = dialog.GetSession();      // Получаем созданный сеанс
                session.Id = _nextSessionId++;         // Назначаем новый ID
                _sessions.Add(session);                // Добавляем в список
                RefreshSessions();                     // Обновляем таблицу
            }
        }

        // Обработчик кнопки "Редактировать сеанс"
        // Открывает диалог редактирования выбранного сеанса
        private void BtnEditSession_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный сеанс
            var selectedSession = dgSessions.SelectedItem as Session;

            if (selectedSession == null)
            {
                MessageBox.Show("Выберите сеанс для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаём диалог редактирования, передаём список фильмов и выбранный сеанс
            var dialog = new SessionDialog(_films, selectedSession);

            if (dialog.ShowDialog() == true)
            {
                var updatedSession = dialog.GetSession();   // Получаем обновлённый сеанс
                updatedSession.Id = selectedSession.Id;     // Сохраняем оригинальный ID

                // Находим и заменяем сеанс в списке
                var index = _sessions.FindIndex(s => s.Id == selectedSession.Id);
                _sessions[index] = updatedSession;

                RefreshSessions();  // Обновляем таблицу
            }
        }

        // Обработчик кнопки "Удалить сеанс"
        // Удаляет выбранный сеанс
        private void BtnDeleteSession_Click(object sender, RoutedEventArgs e)
        {
            var selectedSession = dgSessions.SelectedItem as Session;

            if (selectedSession == null)
            {
                MessageBox.Show("Выберите сеанс для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Показываем подтверждение с информацией о сеансе
            var result = MessageBox.Show($"Удалить сеанс фильма \"{selectedSession.FilmName}\" от {selectedSession.Date:dd.MM.yyyy}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _sessions.Remove(selectedSession);  // Удаляем из списка
                RefreshSessions();                   // Обновляем таблицу
            }
        }

        // Обработчик кнопки "Обновить список сеансов"
        private void BtnRefreshSessions_Click(object sender, RoutedEventArgs e)
        {
            RefreshSessions();
        }

        // Обработчик двойного клика по строке в таблице сеансов
        // Открывает редактирование
        private void DgSessions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BtnEditSession_Click(sender, null);
        }

        #endregion
    }
}