using System;
using System.Windows;
namespace PHUP_Windykacje
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
            string info = "Oj oj";
            try
            {
                var poziom = Licenses.COMBO.AccessLevel(ref info);
                if (poziom <= 0)
                {
                    MessageBox.Show(info);
                    App.Current.Shutdown();
                }
            }
            catch
            {
                MessageBox.Show("Błąd podczas weryfikacji licencji!");
                App.Current.Shutdown();
            }
        }

        private void MyHandler(object sender, UnhandledExceptionEventArgs e)
        {

        }
    }
}
