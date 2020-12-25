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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoadStartWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class LoadWindow : Window
    {

        public LoadWindow()
        {
            InitializeComponent();
        }

        private void BtnExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// for moving window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoxPlayerLoader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /// <summary>
        /// Begin work(load window lose, go to main menu)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnstart_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
