using Coursework_WPF.classes;
using Homework_3_videoplayer_;
using LoadStartWindow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coursework_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            #region Choose language
            try
            {
                string lang = ConfigurationManager.AppSettings["language"];
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            catch { } 
            #endregion

            InitializeComponent();


            this.DataContext = new AppMenu();
            
        }


        #region Window_Settings

        /// <summary>
        /// Close Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExitApp_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            System.Windows.Application.Current.Shutdown();
        }


        /// <summary>
        /// User can move app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoldenFox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /// <summary>
        /// Resize window window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoxMinimaze_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// Mimimize window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnwindowminimized_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Set new language which is on config file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLang = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["language"].Value = selectedLang;
            config.Save(ConfigurationSaveMode.Modified);
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Btnresizor_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        #region Load Wallper must be first then main menu
        public static bool flag = true;

        private void GoldenFox_Activated(object sender, EventArgs e)
        {
            if (flag)
            {
                this.Visibility = Visibility.Hidden;

                try
                {
                    LoadWindow loadWindow = new LoadWindow();
                    loadWindow.Owner = Application.Current.MainWindow;

                    loadWindow.ShowInTaskbar = false;

                    loadWindow.ShowDialog();
                    //loadWindow.Show();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Visibility = Visibility.Visible;
                flag = false;
            }
        }
        #endregion

        #endregion
    }
}
