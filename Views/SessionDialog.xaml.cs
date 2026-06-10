using Kino.Models;
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
using System.Windows.Shapes;

namespace Kino.Views
{
    /// <summary>
    /// Логика взаимодействия для SessionDialog.xaml
    /// </summary>
    public partial class SessionDialog : Window
    {
        private Session _session;
        private List<Film> _films;
        private bool _isEditMode;

        public SessionDialog(List<Film> films, Session session = null)
        {
            InitializeComponent();
            _films = films;

            // Заполняем выпадающий список фильмами
            cboFilm.ItemsSource = _films;

            if (session != null)
            {
                _isEditMode = true;
                _session = new Session
                {
                    Id = session.Id,
                    FilmId = session.FilmId,
                    FilmName = session.FilmName,
                    Date = session.Date,
                    StartTime = session.StartTime,
                    Hall = session.Hall,
                    FreeSeats = session.FreeSeats
                };
                LoadDataToForm();
            }
            else
            {
                _isEditMode = false;
                _session = new Session();
                dpDate.SelectedDate = DateTime.Today;
                cboHall.SelectedIndex = 0;
            }

            // Подписываемся на события изменения для валидации
            txtStartTime.TextChanged += (s, e) => Validate();
            txtFreeSeats.TextChanged += (s, e) => Validate();
            cboFilm.SelectionChanged += (s, e) => Validate();
            dpDate.SelectedDateChanged += (s, e) => Validate();
            cboHall.SelectionChanged += (s, e) => Validate();

            Validate();
        }

        private void LoadDataToForm()
        {
            // Выбираем фильм в выпадающем списке
            if (_session.FilmId > 0)
            {
                var film = _films.FirstOrDefault(f => f.Id == _session.FilmId);
                if (film != null)
                {
                    cboFilm.SelectedItem = film;
                }
            }

            dpDate.SelectedDate = _session.Date;
            txtStartTime.Text = _session.StartTime.ToString(@"hh\:mm");
            txtFreeSeats.Text = _session.FreeSeats.ToString();

            // Выбираем зал
            for (int i = 0; i < cboHall.Items.Count; i++)
            {
                var item = cboHall.Items[i] as System.Windows.Controls.ComboBoxItem;
                if (item != null && item.Content.ToString() == _session.Hall)
                {
                    cboHall.SelectedIndex = i;
                    break;
                }
            }
        }

        private void Validate()
        {
            string errorMessage = "";
            bool isValid = true;

            // Валидация выбора фильма
            if (cboFilm.SelectedItem == null)
            {
                errorMessage += "• Выберите фильм\n";
                isValid = false;
                SetValidationStyle(cboFilm, true);
            }
            else
            {
                SetValidationStyle(cboFilm, false);
            }

            // Валидация даты
            if (dpDate.SelectedDate == null || dpDate.SelectedDate < DateTime.Today)
            {
                errorMessage += "• Выберите корректную дату (сегодня или позже)\n";
                isValid = false;
                SetValidationStyle(dpDate, true);
            }
            else
            {
                SetValidationStyle(dpDate, false);
            }

            // Валидация времени начала (формат ЧЧ:ММ)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtStartTime.Text, @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$"))
            {
                errorMessage += "• Введите корректное время в формате ЧЧ:ММ (например, 19:30)\n";
                isValid = false;
                SetValidationStyle(txtStartTime, true);
            }
            else
            {
                SetValidationStyle(txtStartTime, false);
            }

            // Валидация зала
            if (cboHall.SelectedItem == null || string.IsNullOrWhiteSpace(cboHall.Text))
            {
                errorMessage += "• Выберите или введите название зала\n";
                isValid = false;
                SetValidationStyle(cboHall, true);
            }
            else
            {
                SetValidationStyle(cboHall, false);
            }

            // Валидация количества свободных мест
            if (!int.TryParse(txtFreeSeats.Text, out int freeSeats) || freeSeats < 0 || freeSeats > 200)
            {
                errorMessage += "• Количество свободных мест должно быть целым числом от 0 до 200\n";
                isValid = false;
                SetValidationStyle(txtFreeSeats, true);
            }
            else
            {
                SetValidationStyle(txtFreeSeats, false);
            }

            txtError.Text = errorMessage;
            btnSave.IsEnabled = isValid;
        }

        private void SetValidationStyle(FrameworkElement element, bool hasError)
        {
            // Для разных типов элементов управления применяем разную подсветку
            if (element is System.Windows.Controls.TextBox textBox)
            {
                if (hasError)
                {
                    textBox.BorderBrush = System.Windows.Media.Brushes.Red;
                    textBox.BorderThickness = new Thickness(2);
                    textBox.ToolTip = "Поле заполнено неверно";
                }
                else
                {
                    textBox.BorderBrush = System.Windows.Media.Brushes.Gray;
                    textBox.BorderThickness = new Thickness(1);
                    textBox.ToolTip = null;
                }
            }
            else if (element is System.Windows.Controls.ComboBox comboBox)
            {
                if (hasError)
                {
                    comboBox.BorderBrush = System.Windows.Media.Brushes.Red;
                    comboBox.BorderThickness = new Thickness(2);
                    comboBox.ToolTip = "Поле заполнено неверно";
                }
                else
                {
                    comboBox.BorderBrush = System.Windows.Media.Brushes.Gray;
                    comboBox.BorderThickness = new Thickness(1);
                    comboBox.ToolTip = null;
                }
            }
            else if (element is System.Windows.Controls.DatePicker datePicker)
            {
                if (hasError)
                {
                    datePicker.BorderBrush = System.Windows.Media.Brushes.Red;
                    datePicker.BorderThickness = new Thickness(2);
                    datePicker.ToolTip = "Поле заполнено неверно";
                }
                else
                {
                    datePicker.BorderBrush = System.Windows.Media.Brushes.Gray;
                    datePicker.BorderThickness = new Thickness(1);
                    datePicker.ToolTip = null;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Сохраняем данные
                var selectedFilm = cboFilm.SelectedItem as Film;
                if (selectedFilm != null)
                {
                    _session.FilmId = selectedFilm.Id;
                    _session.FilmName = selectedFilm.Name;
                }

                _session.Date = dpDate.SelectedDate ?? DateTime.Today;

                // Парсим время
                var timeParts = txtStartTime.Text.Split(':');
                _session.StartTime = new TimeSpan(int.Parse(timeParts[0]), int.Parse(timeParts[1]), 0);

                _session.Hall = cboHall.Text;
                _session.FreeSeats = int.Parse(txtFreeSeats.Text);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public Session GetSession()
        {
            return _session;
        }
    }
}
