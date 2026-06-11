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
    // Класс окна авторизации
    public partial class Avtorizaciya : Window
    {

        // Предопределённые учётные данные
        // const - константа (значение НЕ может измениться)
        private const string VALID_LOGIN = "kino";  // VALID_LOGIN - правильный логин для входа
        private const string VALID_PASSWORD = "kin"; // VALID_PASSWORD - правильный пароль для входа

        // Конструктор окна авторизации
        public Avtorizaciya()
        {
            InitializeComponent(); // InitializeComponent() - загружает и инициализирует XAML
        }
        // Обработчик закрытия окна
        // Срабатывает когда пользователь пытается закрыть окно (через крестик или Alt+F4)
        // Спрашивает подтверждение - точно ли хочет выйти
        private void Avtorizaciya_Closing (object sender, System.ComponentModel.CancelEventArgs e)
        {
            // MessageBox.Show() - показывает диалоговое окно с вопросом
            // MessageBoxResult result - сохраняет, какую кнопку нажал пользователь
            MessageBoxResult result = MessageBox.Show("вы уверены что хотите закрыть программу?", " Подтвердите", MessageBoxButton.YesNo, MessageBoxImage.Question);
            // Если пользователь нажал "Нет"
            if (result == MessageBoxResult.No)
            {
                // Окно НЕ закроется, программа продолжит работу
                e.Cancel = true;    // e.Cancel = true - ОТМЕНЯЕМ закрытие окна 
                // Если нажал "Да" - e.Cancel остаётся false, окно закроется
            }
        }

        // Обработчик кнопки "Вход"
        // Срабатывает при нажатии на кнопку "Вход"
        // Проверяет логин и пароль, открывает главное окно при успехе
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try  // try - блок, где может возникнуть ошибка
            {
                // Получаем текст из поля логина (TextBox)
                string login = txtLogin.Text;

                // Получаем пароль из поля пароля (PasswordBox)
                // Password - специальное свойство, скрывающее символы
                string password = txtPassword.Password;

                // Проверка правильности данных
                // && - логическое "И"
                if (login == VALID_LOGIN && password == VALID_PASSWORD)
                {
                    // успешная авторизация 

                    // Создаём новое главное окно
                    MainWindow mainWindow = new MainWindow();

                    // Показываем главное окно
                    mainWindow.Show();

                    // Закрываем текущее окно авторизации
                    this.Close();
                }
                else
                {
                    // ошибка авторизации 

                    // Показываем сообщение об ошибке в статусной строке
                    txtStatus.Text = "Ошибка авторизации. Проверьте логин и пароль";

                    // Очищаем поле логина
                    txtLogin.Clear();

                    // Очищаем поле пароля
                    txtPassword.Clear();

                    // Устанавливаем курсор в поле логина (чтобы сразу печатать)
                    txtLogin.Focus();
                }
            }
            catch (System.Exception ex)  // catch - блок, если произошла ошибка в try
            {
                // Показываем сообщение об ошибке
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "выход"

        // Срабатывает при нажатии на кнопку "Выход"
        // Полностью закрывает приложение

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            // Application.Current - получаем текущее приложение
            // Shutdown() - закрывает ВСЁ приложение полностью
            Application.Current.Shutdown();
        }
    }
}
