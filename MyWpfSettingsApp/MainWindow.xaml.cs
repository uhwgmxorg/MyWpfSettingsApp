using MyWpfSettingsApp.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MyWpfSettingsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// we need:
    /// Install-Package NLog
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static NLog.Logger _nlogger = NLog.LogManager.GetCurrentClassLogger();
        private int _counter = 0;
        
        public Globals Globals { get; set; } = new Globals();

        #region INotify Changed Properties  
        private string message;
        public string Message
        {
            get { return message; }
            set { SetField(ref message, value, nameof(Message)); }
        }
        private string myText;
        public string MyText
        {
            get { return myText; }
            set { SetField(ref myText, value, nameof(MyText)); }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

#if DEBUG
            Title += "    Debug Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " " + Globals.Revison;
#else
            Title += "    Release Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " " + Globals.Revison;
#endif
            _nlogger.Info($"MainWindow() {System.AppDomain.CurrentDomain.FriendlyName} {Title}");

            MyText= LoadTextFileContent(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\MyText.txt");

            Globals.Load();
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_1_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            Message = "You pressed Button #1";
            _nlogger.Debug(Message);

            Globals.Save();
            //Globals.Load();
        }

        /// <summary>
        /// Button_StackOverflow_Click
        /// Provokes a stack overflow to test exception handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_StackOverflow_Click(object sender, RoutedEventArgs e)
        {
            Message = "You pressed Button Stack Overflow";
            _nlogger.Debug(Message);
            StackOverflow("*");
        }

        /// <summary>
        /// Button_DivideByZero_Click
        /// Provokes a divide by zero to test exception handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_DivideByZero_Click(object sender, RoutedEventArgs e)
        {
            Message = "You pressed Button Divide by Zero";
            _nlogger.Debug(Message);
            int n = 0;
            int m = 100;
            int o;

            o = m / n;
        }

        /// <summary>
        /// Button_DeleteLogFiles_Click
        /// Delete all files in the dir, where the .exe is 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_DeleteLogFiles_Click(object sender, RoutedEventArgs e)
        {
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string pattern = "*.log";
            System.IO.Directory.GetFiles(dir, pattern, System.IO.SearchOption.TopDirectoryOnly).ToList().ForEach(System.IO.File.Delete);
        }

        /// <summary>
        /// Button_Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
            _nlogger.Info($"Close() {System.AppDomain.CurrentDomain.FriendlyName}");
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// Window_MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var relativePosition = e.GetPosition(this);
            var point = PointToScreen(relativePosition);
            Message = $"Relative Position: x={relativePosition.X} y={relativePosition.Y}  Screen Position: x={point.X} y={point.Y}";
        }

        /// <summary>
        /// Window_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Message = $"Window Size: x={Width} y={Height}";
        }

        /// <summary>
        /// Window_SizeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Message = $"Window Size: x={Width} y={Height}";
        }

        /// <summary>
        /// Lable_Message_MouseDown
        /// Clear Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lable_Message_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Message = "";
        }

        /// <summary>
        /// Window_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Globals.Save();
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// StackOverflow
        /// ATTENTION calling this function provokes a stack overflow
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string StackOverflow(string str)
        {
            str += $"{_counter++}|";
            StackOverflow(str);
            return str;
        }

        /// <summary>
        /// LoadTextFileContent
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string LoadTextFileContent(string fileName)
        {
            string text = "";
            text = System.IO.File.ReadAllText(fileName);
            return text;
        }

        /// <summary>
        /// SetField
        /// for INotify Changed Properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
