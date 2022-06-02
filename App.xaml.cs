using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Bezier
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ShowError(e.Exception);
            App.Current.Shutdown(1);
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                MainWindow mainView = new MainWindow();
                mainView.Show();
            }
            catch (Exception ex)
            {
                ShowError(ex);
                App.Current.Shutdown(1);
            }
        }

        private void ShowError(Exception ex)
        {
            string err = GetErrorDescription(ex.Message);
            string excList = ex.GetType().ToString();
            Exception exc = ex;
            while (exc.InnerException != null)
            {               
                exc = exc.InnerException;
                excList = excList + " => " + exc.GetType().ToString();
                err = err + "\n*****\n" + GetErrorDescription(exc.Message);
            }

            MessageBox.Show(String.Format("Возникла ошибка!\nЦепочка исключений:\n{0}\n\nСообщение:\n{1}\n\nПрограмма будет закрыта.", excList, err),
                App.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private string GetErrorDescription(string message)
        {
            if (message.Length > 100)
                return message.Substring(1, 100);
            else return message;
        }
    }
}
