using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kino
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Глобальная обработка необработанных исключений
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Перехват всех необработанных исключений
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        // Обработка исключений в потоке UI
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Произошла ошибка: {e.Exception.Message}\n\nПриложение продолжит работу.",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true; // Предотвращаем закрытие приложения
        }

        // Обработка исключений в других потоках
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show($"Произошла критическая ошибка: {ex.Message}\n\nПриложение будет закрыто.",
                    "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
