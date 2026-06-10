using Kino.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для FilmDialog.xaml
    /// </summary>
    public partial class FilmDialog : Window, INotifyDataErrorInfo
    {
        private Film _film;
        private bool _isEditMode;

        public FilmDialog(Film film = null)
        {
            InitializeComponent();

            if (film != null)
            {
                _isEditMode = true;
                _film = new Film
                {
                    Id = film.Id,
                    Name = film.Name,
                    Genre = film.Genre,
                    Director = film.Director,
                    Duration = film.Duration,
                    AgeRating = film.AgeRating,
                    Price = film.Price
                };
                LoadDataToForm();
            }
            else
            {
                _isEditMode = false;
                _film = new Film();
                cboAgeRating.SelectedIndex = 0;
            }

            // Подписываемся на события изменения текста для валидации
            txtName.TextChanged += (s, e) => Validate();
            txtGenre.TextChanged += (s, e) => Validate();
            txtDirector.TextChanged += (s, e) => Validate();
            txtDuration.TextChanged += (s, e) => Validate();
            txtPrice.TextChanged += (s, e) => Validate();

            Validate();
        }

        private void LoadDataToForm()
        {
            txtName.Text = _film.Name;
            txtGenre.Text = _film.Genre;
            txtDirector.Text = _film.Director;
            txtDuration.Text = _film.Duration.ToString();
            txtPrice.Text = _film.Price.ToString();

            // Установка выбранного значения в ComboBox
            for (int i = 0; i < cboAgeRating.Items.Count; i++)
            {
                var item = cboAgeRating.Items[i] as System.Windows.Controls.ComboBoxItem;
                if (item != null && item.Content.ToString() == _film.AgeRating)
                {
                    cboAgeRating.SelectedIndex = i;
                    break;
                }
            }
        }

        private void Validate()
        {
            string errorMessage = "";
            bool isValid = true;

            // Валидация названия
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorMessage += "! Введите название фильма\n";
                isValid = false;
            }

            // Валидация жанра
            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            {
                errorMessage += "! Введите жанр\n";
                isValid = false;
            }

            // Валидация режиссёра
            if (string.IsNullOrWhiteSpace(txtDirector.Text))
            {
                errorMessage += "! Введите режиссёра\n";
                isValid = false;
            }

            // Валидация продолжительности
            if (!int.TryParse(txtDuration.Text, out int duration) || duration < 30 || duration > 300)
            {
                errorMessage += "! Продолжительность должна быть целым числом от 30 до 300 минут\n";
                isValid = false;
            }

            // Валидация цены
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 100 || price > 2000)
            {
                errorMessage += "! Цена билета должна быть числом от 100 до 2000 рублей\n";
                isValid = false;
            }

            // Подсветка полей красной рамкой через стиль
            SetValidationStyle(txtName, string.IsNullOrWhiteSpace(txtName.Text));
            SetValidationStyle(txtGenre, string.IsNullOrWhiteSpace(txtGenre.Text));
            SetValidationStyle(txtDirector, string.IsNullOrWhiteSpace(txtDirector.Text));
            SetValidationStyle(txtDuration, !int.TryParse(txtDuration.Text, out int d) || d < 30 || d > 300);
            SetValidationStyle(txtPrice, !decimal.TryParse(txtPrice.Text, out decimal p) || p < 100 || p > 2000);

            txtError.Text = errorMessage;
            btnSave.IsEnabled = isValid;
        }

        private void SetValidationStyle(System.Windows.Controls.TextBox textBox, bool hasError)
        {
            if (hasError)
            {
                textBox.BorderBrush = System.Windows.Media.Brushes.Red;
                textBox.BorderThickness = new Thickness(2);
                textBox.ToolTip = "! Поле заполнено неверно";
            }
            else
            {
                textBox.BorderBrush = System.Windows.Media.Brushes.Gray;
                textBox.BorderThickness = new Thickness(1);
                textBox.ToolTip = null;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Сохраняем данные
                _film.Name = txtName.Text.Trim();
                _film.Genre = txtGenre.Text.Trim();
                _film.Director = txtDirector.Text.Trim();
                _film.Duration = int.Parse(txtDuration.Text);
                _film.AgeRating = (cboAgeRating.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString() ?? "0+";
                _film.Price = decimal.Parse(txtPrice.Text);

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

        public Film GetFilm()
        {
            return _film;
        }

        #region INotifyDataErrorInfo (для совместимости)
        public bool HasErrors => !btnSave.IsEnabled;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public System.Collections.IEnumerable GetErrors(string propertyName) => null;
        #endregion
    }
}
