using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kino
{
    // Логика взаимодействия для App.xaml
    // Класс приложения - управляет всем WPF приложением
    public partial class App : Application
    {
        // Глобальная обработка необработанных исключений
        // override - переопределяем метод базового класса
        // StartupEventArgs e - аргументы запуска (параметры командной строки и т.д.)
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); // base.OnStartup(e) - вызываем метод родительского класса

            // Перехват всех необработанных исключений
            // DispatcherUnhandledException - событие, когда в UI потоке произошла ошибка
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            // App_DispatcherUnhandledException - наш метод, который обработает эту ошибку
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        // Обработка исключений в потоке UI
        // Обрабатывает ошибки, которые возникли в интерфейсе пользователя
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) //"sender">Объект, вызвавший событие  "e">Аргументы события, содержащие информацию об ошибке
        {
            // MessageBox.Show() - показывает диалоговое окно с сообщением
            
            MessageBox.Show($"Произошла ошибка: {e.Exception.Message}\n\nПриложение продолжит работу.", // $"" - интерполяция строк   e.Exception.Message - текст ошибки из исключения   \n\n - два переноса строки (пустая строка между сообщениями)
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true; // Предотвращаем закрытие приложения
        }

        // Обработка исключений в других потоках
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // is Exception ex - проверяем, является ли объект типом Exception
            // Если да - создаём переменную ex с этим исключением
            if (e.ExceptionObject is Exception ex) // e.ExceptionObject - объект исключения
            {
                // Показываем сообщение об ошибке
                MessageBox.Show($"Произошла критическая ошибка: {ex.Message}\n\nПриложение будет закрыто.",   // Приложение закроется автоматически (это критическая ошибка)
                    "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
