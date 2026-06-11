using Kino.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    // Класс диалогового окна для работы с сеансом
    public partial class SessionDialog : Window
    {
        // Приватные поля
        // Редактируемый сеанс (хранит данные)
        private Session _session;
        // Список всех фильмов (для выпадающего списка)
        private List<Film> _films;
        // true - редактирование, false - добавление нового
        private bool _isEditMode;

        // Конструктор
        // Конструктор диалогового окна сеанса
        public SessionDialog(List<Film> films, Session session = null)
        {
            // Инициализация компонентов из XAML (обязательно!)
            InitializeComponent();

            // Сохраняем список фильмов для выпадающего списка
            _films = films;

            // Заполняем выпадающий список фильмами
            cboFilm.ItemsSource = _films;

            // Проверка: редактируем и создаем 
            // Если передан сеанс - режим редактирования
            if (session != null)
            {
                _isEditMode = true;  // Включаем режим редактирования

                // Создаём КОПИЮ сеанса, чтобы не изменять оригинал до сохранения
                _session = new Session
                {
                    Id = session.Id,                    // Копируем ID
                    FilmId = session.FilmId,            // Копируем ID фильма
                    FilmName = session.FilmName,        // Копируем название фильма
                    Date = session.Date,                // Копируем дату
                    StartTime = session.StartTime,      // Копируем время начала
                    Hall = session.Hall,                // Копируем зал
                    FreeSeats = session.FreeSeats       // Копируем количество мест
                };

                LoadDataToForm();  // Загружаем данные в поля формы
            }
            else  // Если сеанс не передан - режим создания нового
            {
                _isEditMode = false;        // Режим добавления
                _session = new Session();   // Создаём пустой объект
                dpDate.SelectedDate = DateTime.Today;  // Сегодняшняя дата по умолчанию
                cboHall.SelectedIndex = 0;  // Выбираем первый зал из списка
            }

            // подписка на события валидации 
            // При каждом изменении данных вызываем Validate()
            txtStartTime.TextChanged += (s, e) => Validate();       // Время начала
            txtFreeSeats.TextChanged += (s, e) => Validate();       // Количество мест
            cboFilm.SelectionChanged += (s, e) => Validate();       // Выбор фильма
            dpDate.SelectedDateChanged += (s, e) => Validate();      // Выбор даты
            cboHall.SelectionChanged += (s, e) => Validate();        // Выбор зала

            // Первоначальная проверка (кнопка "Сохранить" будет отключена, если поля не заполнены)
            Validate();
        }

        // Загрузка данных в форму
        // Загружает данные из объекта Session в поля ввода на форме
        private void LoadDataToForm()
        {
            // Выбор фильма в выпадающем списке
            // Если есть ID фильма (больше 0)
            if (_session.FilmId > 0)
            {
                // Ищем фильм с таким же ID в списке
                // FirstOrDefault - возвращает первый подходящий элемент или null
                var film = _films.FirstOrDefault(f => f.Id == _session.FilmId);
                if (film != null)
                {
                    cboFilm.SelectedItem = film;  // Устанавливаем выбранный фильм
                }
            }

            // Дата сеанса
            dpDate.SelectedDate = _session.Date;

            // Время начала (форматируем TimeSpan в строку "ЧЧ:ММ")
            // @"hh\:mm" - экранирование двоеточия для корректного вывода
            txtStartTime.Text = _session.StartTime.ToString(@"hh\:mm");

            // Количество свободных мест
            txtFreeSeats.Text = _session.FreeSeats.ToString();

            // Выбор зала в выпадающем списке
            // Проходим по всем элементам ComboBox
            for (int i = 0; i < cboHall.Items.Count; i++)
            {
                // Получаем элемент как ComboBoxItem
                var item = cboHall.Items[i] as System.Windows.Controls.ComboBoxItem;

                // Сравниваем содержимое элемента с нужным названием зала
                if (item != null && item.Content.ToString() == _session.Hall)
                {
                    cboHall.SelectedIndex = i;  // Устанавливаем выбранный индекс
                    break;  // Прерываем цикл, так как нашли нужный элемент
                }
            }
        }

        // Валидация данных (проверка корректности
        // Проверяет все поля на корректность
        // Собирает сообщения об ошибках и управляет кнопкой "Сохранить"
        private void Validate()
        {
            string errorMessage = "";  // Строка для сбора всех сообщений об ошибках
            bool isValid = true;       // Флаг: true - все данные верны, false - есть ошибки

            // ПРОВЕРКА ВЫБОРА ФИЛЬМА 
            if (cboFilm.SelectedItem == null)
            {
                errorMessage += " !Выберите фильм\n";
                isValid = false;
                SetValidationStyle(cboFilm, true);  // Красная рамка
            }
            else
            {
                SetValidationStyle(cboFilm, false); // Убираем красную рамку
            }

            // Проверка даты 
            // Дата не может быть в прошлом (только сегодня или позже)
            if (dpDate.SelectedDate == null || dpDate.SelectedDate < DateTime.Today)
            {
                errorMessage += "!Выберите корректную дату (сегодня или позже)\n";
                isValid = false;
                SetValidationStyle(dpDate, true);
            }
            else
            {
                SetValidationStyle(dpDate, false);
            }

            // Проверка времени начала 
            // Регулярное выражение для проверки формата ЧЧ:ММ

            if (!Regex.IsMatch(txtStartTime.Text, @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$"))
            {
                errorMessage += "!Введите корректное время в формате ЧЧ:ММ (19:30)\n";
                isValid = false;
                SetValidationStyle(txtStartTime, true);
            }
            else
            {
                SetValidationStyle(txtStartTime, false);
            }

            // Проверка зала 
            // Зал не может быть пустым
            if (cboHall.SelectedItem == null || string.IsNullOrWhiteSpace(cboHall.Text))
            {
                errorMessage += "!Выберите или введите название зала\n";
                isValid = false;
                SetValidationStyle(cboHall, true);
            }
            else
            {
                SetValidationStyle(cboHall, false);
            }

            // Проверка кол-ва мест в зале 
            // TryParse - пытается преобразовать строку в число
            // freeSeats - выходной параметр с результатом
            if (!int.TryParse(txtFreeSeats.Text, out int freeSeats) || freeSeats < 0 || freeSeats > 200)
            {
                errorMessage += "!Количество свободных мест должно быть целым числом от 0 до 200\n";
                isValid = false;
                SetValidationStyle(txtFreeSeats, true);
            }
            else
            {
                SetValidationStyle(txtFreeSeats, false);
            }

            // Обновление UI 
            txtError.Text = errorMessage;      // Показываем сообщения об ошибках
            btnSave.IsEnabled = isValid;       // Включаем кнопку "Сохранить" только если всё верно
        }

        // установка стиля подсветки ошибки
        /// Устанавливает красную рамку и всплывающую подсказку для поля с ошибкой
        /// Поддерживает разные типы элементов: TextBox, ComboBox, DatePicker
        private void SetValidationStyle(FrameworkElement element, bool hasError)
        {
            // TextBox (текстовое поле)
            if (element is System.Windows.Controls.TextBox textBox)
            {
                if (hasError)
                {
                    // Есть ошибка: красная рамка, подсказка
                    textBox.BorderBrush = System.Windows.Media.Brushes.Red;
                    textBox.BorderThickness = new Thickness(2);
                    textBox.ToolTip = "Поле заполнено неверно";
                }
                else
                {
                    // Ошибки нет: серая рамка, убираем подсказку
                    textBox.BorderBrush = System.Windows.Media.Brushes.Gray;
                    textBox.BorderThickness = new Thickness(1);
                    textBox.ToolTip = null;
                }
            }
            // ComboBox (выпадающий список) 
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
            // DatePicker (выбор даты) 
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

        // Обработчик кнопки "сохранить"
        // Сохраняет данные из формы в объект Session
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try  // try - блок, где может возникнуть ошибка
            {
                // Сохраняем данные из формы  

                // Получаем выбранный фильм
                var selectedFilm = cboFilm.SelectedItem as Film;
                if (selectedFilm != null)
                {
                    _session.FilmId = selectedFilm.Id;      // Сохраняем ID фильма
                    _session.FilmName = selectedFilm.Name;  // Сохраняем название (для отображения)
                }

                // Сохраняем дату (?? - если SelectedDate = null, используем Today)
                _session.Date = dpDate.SelectedDate ?? DateTime.Today;

                // (преобразуем строку в TimeSpan)
                var timeParts = txtStartTime.Text.Split(':');  // Разделяем "19:30" → ["19", "30"]
                _session.StartTime = new TimeSpan(
                    int.Parse(timeParts[0]),  // Часы (19)
                    int.Parse(timeParts[1]),  // Минуты (30)
                    0                         // Секунды (0)
                );

                _session.Hall = cboHall.Text;                    // Название зала
                _session.FreeSeats = int.Parse(txtFreeSeats.Text); // Количество мест

                // DialogResult = true - сообщает, что пользователь нажал "Сохранить"
                DialogResult = true;
                Close();  // Закрываем окно
            }
            catch (Exception ex)  // Если произошла ошибка при сохранении
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "отмена"

        /// Закрывает окно без сохранения
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  // Сообщает, что пользователь отменил операцию
            Close();               // Закрываем окно
        }

        // Получение созданного/отредактированного сеанса

        // Возвращает объект Session с данными из формы
        public Session GetSession()
        {
            return _session;
        }
    }
}