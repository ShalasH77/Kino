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
    /// Логика взаимодействия для Avtorizaciya.xaml
    /// </summary>
    public partial class Avtorizaciya : Window
    {
        // Предопределённые учётные данные
        private const string VALID_LOGIN = "kino";
        private const string VALID_PASSWORD = "kin";

        public Avtorizaciya()
        {
            InitializeComponent();
        }
        private void Avtorizaciya_Closing (object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("вы уверены что хотите закрыть программу?", " Подтвердите", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        // Обработчик кнопки "Вход"
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = txtLogin.Text;
                string password = txtPassword.Password;

                // Проверка логина и пароля
                if (login == VALID_LOGIN && password == VALID_PASSWORD)
                {
                    // Успешная авторизация
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close(); // Закрываем окно авторизации
                }
                else
                {
                    // Ошибка авторизации
                    txtStatus.Text = "Ошибка авторизации. Проверьте логин и пароль";
                    txtLogin.Clear();
                    txtPassword.Clear();
                    txtLogin.Focus();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "Выход"
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
