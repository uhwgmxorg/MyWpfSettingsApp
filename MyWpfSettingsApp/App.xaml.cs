using System;
using System.Windows;

namespace MyWpfSettingsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static NLog.Logger _nlogger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        public App()
        {
            _nlogger.Info($"START {System.AppDomain.CurrentDomain.FriendlyName}");
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        /// <summary>
        /// OnExit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            _nlogger.Info($"EXIT {System.AppDomain.CurrentDomain.FriendlyName}");
            base.OnExit(e);
        }

        /// <summary>
        /// OnDispatcherUnhandledException
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            _nlogger.Error($"Exception: {errorMessage}");
            _nlogger.Error($"Exception ToString(): {e.ToString()}");
            //MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            // OR whatever you want like logging etc. MessageBox it's just example
            // for quick debugging etc.
            e.Handled = true;
        }
    }
}
