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
    // Логика взаимодействия для FilmDialog.xaml
    // Класс диалогового окна для работы с фильмом
    public partial class FilmDialog : Window, INotifyDataErrorInfo // Window - наследуется от окна WPF     INotifyDataErrorInfo - интерфейс для валидации данных (показывает ошибки в UI)
    {
        // Приватные поля
        // Редактируемый фильм (хранит данные)
        private Film _film;
        // true - редактирование, false - добавление нового
        private bool _isEditMode;

        // Конструктор
        // Конструктор диалогового окна фильма
        public FilmDialog(Film film = null)
        {
            // Инициализация компонентов из XAML
            InitializeComponent();

            // Если передан фильм - режим редактирования
            if (film != null)
            {
                _isEditMode = true; // Включаем режим редактирования
                _film = new Film // Создаём КОПИЮ фильма, чтобы не изменять оригинал до сохранения
                {
                    Id = film.Id,
                    Name = film.Name,
                    Genre = film.Genre,
                    Director = film.Director,
                    Duration = film.Duration,
                    AgeRating = film.AgeRating,
                    Price = film.Price
                };
                LoadDataToForm(); // Загружаем данные в поля формы
            }
            else
            {
                _isEditMode = false; // Если фильм не передан - режим создания нового
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

        // Загружает данные из объекта Film в поля ввода на форме
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
                // Получаем элемент как ComboBoxItem
                var item = cboAgeRating.Items[i] as System.Windows.Controls.ComboBoxItem;
                // Сравниваем содержимое элемента с нужным возрастным рейтингом
                if (item != null && item.Content.ToString() == _film.AgeRating)
                {
                    cboAgeRating.SelectedIndex = i; // Устанавливаем выбранный индекс
                    break; // Прерываем цикл, так как нашли нужный элемент
                }
            }
        }

        // Валидация данных (проверка корректности)
        // Проверяет все поля на корректность
        // Собирает сообщения об ошибках и управляет кнопкой "Сохранить"
        private void Validate()
        {
            string errorMessage = ""; // Строка для сбора всех сообщений об ошибках
            bool isValid = true; // true - все данные верны, false - есть ошибки


            // Валидация названия
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorMessage += "!Введите название фильма\n";
                isValid = false;
            }

            // Валидация жанра
            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            {
                errorMessage += "!Введите жанр\n";
                isValid = false;
            }

            // Валидация режиссёра
            if (string.IsNullOrWhiteSpace(txtDirector.Text))
            {
                errorMessage += "!Введите режиссёра\n";
                isValid = false;
            }

            // Валидация продолжительности
            if (!int.TryParse(txtDuration.Text, out int duration) || duration < 30 || duration > 300)
            {
                errorMessage += "!Продолжительность должна быть целым числом от 30 до 300 минут\n";
                isValid = false;
            }

            // Валидация цены
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 100 || price > 2000)
            {
                errorMessage += "!Цена билета должна быть числом от 100 до 2000 рублей\n";
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

        // Устанавливает красную рамку и всплывающую подсказку для поля с ошибкой
        private void SetValidationStyle(System.Windows.Controls.TextBox textBox, bool hasError)
        {
            if (hasError)
            {
                // Есть ошибка - красная рамка и подсказка
                textBox.BorderBrush = System.Windows.Media.Brushes.Red;
                textBox.BorderThickness = new Thickness(2);
                textBox.ToolTip = "!Поле заполнено неверно";
            }
            else
            {
                // Ошибки нет - серая рамка, убираем подсказку
                textBox.BorderBrush = System.Windows.Media.Brushes.Gray;
                textBox.BorderThickness = new Thickness(1);
                textBox.ToolTip = null;
            }
        }

        // Обработчик кнопки "сохранить"
        // Сохраняет данные из формы в объект Film
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try // try - блок, где может возникнуть ошибка
            {
                // Сохраняем данные
                _film.Name = txtName.Text.Trim(); // Trim() - удаляет пробелы по краям
                _film.Genre = txtGenre.Text.Trim();
                _film.Director = txtDirector.Text.Trim();
                _film.Duration = int.Parse(txtDuration.Text); // Преобразуем строку в число
                // Получаем выбранное значение из ComboBox
                // ?. - оператор условного доступа (если null, вернёт null)
                // ?? - оператор объединения с null (если null, используем "0+")
                _film.AgeRating = (cboAgeRating.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString() ?? "0+";
                _film.Price = decimal.Parse(txtPrice.Text); // Преобразуем строку в decimal

                DialogResult = true; // DialogResult = true - сообщает, что пользователь нажал "Сохранить"
                Close(); // Это закроет окно и вернёт true в ShowDialog()
            }
            catch (Exception ex) // Если произошла ошибка при сохранении
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "отмена"
        // Закрывает окно без сохранения
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Сообщает, что пользователь отменил операцию
            Close();  // Закрываем окно
        }

        // Возвращает объект Film с данными из формы
        public Film GetFilm()
        {
            return _film;
        }

        // INotifyDataErrorInfo (реализация для валидации)
        #region INotifyDataErrorInfo (для совместимости)
        // Свойство: есть ли ошибки валидации
        // true - если кнопка сохранения отключена
        public bool HasErrors => !btnSave.IsEnabled;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged; // Событие изменения ошибок (не используется в данном коде, требуется интерфейсом)
        public System.Collections.IEnumerable GetErrors(string propertyName) => null; // Возвращает ошибки для указанного свойства (не используется)
        #endregion //#endregion - это директива препроцессора, которая закрывает блок кода, открытый директивой #region.
    }
}
