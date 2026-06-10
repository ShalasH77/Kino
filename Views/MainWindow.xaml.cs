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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Хранилища данных
        private List<Film> _films = new List<Film>();
        private List<Session> _sessions = new List<Session>();
        private int _nextFilmId = 1;
        private int _nextSessionId = 1;

        public MainWindow()
        {
            InitializeComponent();
            LoadSampleData(); // Загрузка тестовых данных
            RefreshFilms();
            RefreshSessions();
        }

        // Загрузка тестовых данных для демонстрации
        private void LoadSampleData()
        {
            _films = new List<Film>
            {
                new Film { Id = _nextFilmId++, Name = "1+1", Genre = "Комедия", Director = "Оливье Накаш", Duration = 120, AgeRating = "18+", Price = 350 },
                new Film { Id = _nextFilmId++, Name = "Волк с Уолл-стрит", Genre = "Биография", Director = "Мартин Скорсезе", Duration = 180, AgeRating = "18+", Price = 400 },
                new Film { Id = _nextFilmId++, Name = "Король Лев", Genre = "Мультфильм", Director = "Джон Фавро", Duration = 118, AgeRating = "6+", Price = 250 }
            };

            _sessions = new List<Session>
            {
                new Session { Id = _nextSessionId++, FilmId = 1, FilmName = "Король Лев", Date = DateTime.Today.AddDays(1), StartTime = new TimeSpan(9, 0, 0), Hall = "Зал 1", FreeSeats = 45 },
                new Session { Id = _nextSessionId++, FilmId = 1, FilmName = "1+1", Date = DateTime.Today.AddDays(1), StartTime = new TimeSpan(19, 0, 0), Hall = "Зал 1", FreeSeats = 50 },
                new Session { Id = _nextSessionId++, FilmId = 1, FilmName = "1+1", Date = DateTime.Today.AddDays(1), StartTime = new TimeSpan(21, 30, 0), Hall = "Зал 1", FreeSeats = 45 },
                new Session { Id = _nextSessionId++, FilmId = 2, FilmName = "Волк с Уолл-стрит", Date = DateTime.Today.AddDays(2), StartTime = new TimeSpan(18, 0, 0), Hall = "Зал 2", FreeSeats = 60 }
            };
        }

        #region Фильмы

        private void RefreshFilms()
        {
            dgFilms.ItemsSource = null;
            dgFilms.ItemsSource = _films;
        }

        private void BtnAddFilm_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FilmDialog();
            if (dialog.ShowDialog() == true)
            {
                var film = dialog.GetFilm();
                film.Id = _nextFilmId++;
                _films.Add(film);
                RefreshFilms();
            }
        }

        private void BtnEditFilm_Click(object sender, RoutedEventArgs e)
        {
            var selectedFilm = dgFilms.SelectedItem as Film;
            if (selectedFilm == null)
            {
                MessageBox.Show("Выберите фильм для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new FilmDialog(selectedFilm);
            if (dialog.ShowDialog() == true)
            {
                var updatedFilm = dialog.GetFilm();
                updatedFilm.Id = selectedFilm.Id;
                var index = _films.FindIndex(f => f.Id == selectedFilm.Id);
                _films[index] = updatedFilm;
                RefreshFilms();
            }
        }

        private void BtnDeleteFilm_Click(object sender, RoutedEventArgs e)
        {
            var selectedFilm = dgFilms.SelectedItem as Film;
            if (selectedFilm == null)
            {
                MessageBox.Show("Выберите фильм для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Подтверждение удаления
            var result = MessageBox.Show($"Удалить запись о фильме \"{selectedFilm.Name}\"?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Удаляем также связанные сеансы
                var sessionsToDelete = _sessions.Where(s => s.FilmId == selectedFilm.Id).ToList();
                foreach (var session in sessionsToDelete)
                {
                    _sessions.Remove(session);
                }




                _films.Remove(selectedFilm);
                RefreshFilms();
                RefreshSessions();
            }
        }

        private void BtnRefreshFilms_Click(object sender, RoutedEventArgs e)
        {
            RefreshFilms();
        }

        private void DgFilms_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BtnEditFilm_Click(sender, null);
        }

        #endregion

        #region Сеансы

        private void RefreshSessions()
        {
            dgSessions.ItemsSource = null;
            dgSessions.ItemsSource = _sessions;
        }

        private void BtnAddSession_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SessionDialog(_films);
            if (dialog.ShowDialog() == true)
            {
                var session = dialog.GetSession();
                session.Id = _nextSessionId++;
                _sessions.Add(session);
                RefreshSessions();
            }
        }

        private void BtnEditSession_Click(object sender, RoutedEventArgs e)
        {
            var selectedSession = dgSessions.SelectedItem as Session;
            if (selectedSession == null)
            {
                MessageBox.Show("Выберите сеанс для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new SessionDialog(_films, selectedSession);
            if (dialog.ShowDialog() == true)
            {
                var updatedSession = dialog.GetSession();
                updatedSession.Id = selectedSession.Id;
                var index = _sessions.FindIndex(s => s.Id == selectedSession.Id);
                _sessions[index] = updatedSession;
                RefreshSessions();
            }
        }

        private void BtnDeleteSession_Click(object sender, RoutedEventArgs e)
        {
            var selectedSession = dgSessions.SelectedItem as Session;
            if (selectedSession == null)
            {
                MessageBox.Show("Выберите сеанс для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить сеанс фильма \"{selectedSession.FilmName}\" от {selectedSession.Date:dd.MM.yyyy}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _sessions.Remove(selectedSession);
                RefreshSessions();
            }
        }

        private void BtnRefreshSessions_Click(object sender, RoutedEventArgs e)
        {
            RefreshSessions();
        }

        private void DgSessions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BtnEditSession_Click(sender, null);
        }
        #endregion
    }
}
