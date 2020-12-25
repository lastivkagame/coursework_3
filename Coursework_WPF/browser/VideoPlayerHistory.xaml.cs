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
using CefSharp.Wpf;

namespace Coursework_WPF.windows
{
    /// <summary>
    /// Interaction logic for VideoPlayerHistory.xaml
    /// </summary>
    public partial class VideoPlayerHistory : Window
    {
        List<string> webpages;
        int Current = 0;
        public VideoPlayerHistory()
        {
            InitializeComponent();
        }

        void GoHome()
        {
            string standart = "www.youtube.com";
            Searchresalt.Text = standart;
            chrome.Address = standart;
            webpages.Add(standart);
        }

        void LoadWebPages(string Link, bool addToList = true)
        {
            Searchresalt.Text = Link;
            chrome.Address = Link;
            MenuItem item = new MenuItem();
            item.Click += MenuClicked;
            item.Header = Link;
            item.Width = 200;

            MenuHistoty.Items.Add(item);

            if (addToList)
            {
                Current++;
                webpages.Add(Link);
            }
        }

        void ToggleWebPages(string option)
        {
            if (option == "↠")
            {
                if ((webpages.Count - Current - 1) != 0)
                {
                    Current++;
                    LoadWebPages(webpages[Current], false);
                }
            }
            else
            {
                if ((webpages.Count + Current - 1) >= webpages.Count)
                {
                    Current--;
                    LoadWebPages(webpages[Current], false);
                }
            }
        }

        private void ButtonExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FoxMinimaze_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void GoldenfoxBrowser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnResize_MouseDown(object sender, MouseButtonEventArgs e)
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
        /// Back and forward buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBackForward_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ToggleWebPages(btn.Content.ToString());
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadWebPages(webpages[Current]);
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            LoadWebPages(webpages[0]);
        }

        private void MenuClicked(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            LoadWebPages(item.Header.ToString());
            //LoadWebPages(webpages[0]);
        }

        private void GoldenfoxBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            webpages = new List<string>();
            GoHome();
        }

        private void Searchresalt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoadWebPages(Searchresalt.Text);
            }
        }

        private void LittleButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadWebPages(Searchresalt.Text);
        }

        private void ButtonHistoty_Click(object sender, RoutedEventArgs e)
        {
            if (webpages.Count != 0)
            {
                MenuHistoty.PlacementTarget = ButtonHistoty;
                MenuHistoty.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                //MenuHistoty.HorizontalOffset = -150;
                MenuHistoty.IsOpen = true;
            }
        }

        private void ButtonHistoty_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            //MenuItem item = (MenuItem)sender;
            //LoadWebPages(item.Header.ToString());
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{

        //}

        //private void Btnback_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        mywebBroser.GoBack();
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void Btnforward_Click(object sender, RoutedEventArgs e)
        //{

        //    try
        //    {
        //        mywebBroser.GoForward();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void Btnsearch_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        mywebBroser.Source = new Uri("https://" + Searchresalt.Text);
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void GoldenfoxBrowser_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        mywebBroser.Source = new Uri("https://www.youtube.com/");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void GoldenfoxBrowser_Loaded_1(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
