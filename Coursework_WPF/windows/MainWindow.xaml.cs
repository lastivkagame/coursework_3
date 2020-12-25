using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
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
using System.Windows.Threading;
using Coursework_WPF.Localizations;
using System.Collections.ObjectModel;

namespace Homework_3_videoplayer_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Videoplayer : Window
    {
        #region Values
        DispatcherTimer timer = new DispatcherTimer();
        public ICollection<TimeSpan> savemoments = new ObservableCollection<TimeSpan>();
        public ICollection<string> savecomments = new ObservableCollection<string>();
        public ICollection<string> names = new ObservableCollection<string>();
        public ICollection<string> media_path = new ObservableCollection<string>();
        public ICollection<string> lastmoments = new ObservableCollection<string>();
        public ICollection<string> starcount = new ObservableCollection<string>();

        public string logs = "\\logs";
        public string logs_NameCollection = "\\Collection";
        public string pathes = "\\path";
        public string lastmoment = "\\lastmomemt";
        public string starcounts = "\\star_counts";
        public string save_mom = "\\save_moments";
        public string save_com = "\\save_coments";
        public string current_video = "";

        public int CurrentVideoIndex = 0;
        #endregion

        public Videoplayer()
        {
            #region Set Language
            try
            {
                string lang = ConfigurationManager.AppSettings["language"];
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            catch { }
            #endregion

            InitializeComponent();

            VideroPlayerApp.AllowDrop = true;
            player.AllowDrop = true;

            BigPlayButton.Visibility = Visibility.Hidden;
            borderplaybutton.Visibility = Visibility.Hidden;
            SpecialSettingsPanel.Visibility = Visibility.Hidden;

            string pathimag = Directory.GetCurrentDirectory() + "\\..\\..\\images\\icon_player.jpg";
            Uri iconUri = new Uri(pathimag, UriKind.RelativeOrAbsolute);
            VideroPlayerApp.Icon = BitmapFrame.Create(iconUri);
            leftsizepanel.Visibility = Visibility.Hidden;
            DownPanel.Visibility = Visibility.Hidden;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Checks for mathes in the name collection and the param name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsDublicate(string name)
        {
            foreach (var item in names)
            {
                if (item == name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Set language value by choosen in combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedLang = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["language"].Value = selectedLang;
            config.Save(ConfigurationSaveMode.Modified);
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// this timer for video 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            videoslider.Value = player.Position.TotalSeconds;

            if (player.Source != null)
            {
                if (player.NaturalDuration.HasTimeSpan)
                    TextTime.Text = String.Format("{0} / {1}", player.Position.ToString(@"mm\:ss"), player.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                TextTime.Text = "No file selected...";
        }

        #region Work with video Speed
        private void SpeedExpander_MouseEnter(object sender, MouseEventArgs e)
        {
            SpeedExpander.IsExpanded = true;
        }

        private void Speed05_Checked(object sender, RoutedEventArgs e)
        {
            player.SpeedRatio = 0.5;
        }

        private void Speed10_Checked(object sender, RoutedEventArgs e)
        {
            player.SpeedRatio = 1.0;
        }

        private void Speed15_Checked(object sender, RoutedEventArgs e)
        {
            player.SpeedRatio = 1.5;
        }

        private void Speed20_Checked(object sender, RoutedEventArgs e)
        {
            player.SpeedRatio = 2.0;
        }

        private void Speed25_Checked(object sender, RoutedEventArgs e)
        {
            player.SpeedRatio = 2.5;
        }

        private void SpeedExpander_MouseLeave(object sender, MouseEventArgs e)
        {
            SpeedExpander.IsExpanded = false;
        }
        #endregion

        #region For window settings
        private void VideroPlayerApp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ButtonExitApp_Click(object sender, RoutedEventArgs e)
        {
            //if (lastmoments.Count == 0)
            //{
            //    lastmoments.Add(player.Position.ToString() + "\n");
            //}
            if (names.Count != lastmoments.Count)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    if (names.ToList()[i] == current_video)
                    {
                        try
                        {
                            lastmoments.ToList()[i] = player.Position.ToString() + "\n";
                        }
                        catch (Exception)
                        {
                            lastmoments.Add(player.Position.ToString() + "\n");
                        }
                        //string temp = lastmoments.ToList()[i];
                        //lastmoments.Remove(temp);
                        //lastmoments.Rep(i, player.Position.ToString() + "\n");
                    }
                }
            }
            else
            {
                lastmoments.Add(player.Position.ToString() + "\n");
            }
            player.Stop();
            this.Close();
        }

        private void ButtonMinimApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonResizor_Click(object sender, RoutedEventArgs e)
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
        #endregion

        /// <summary>
        /// Show/hidden menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (leftsizepanel.Visibility == Visibility.Hidden)
            {
                leftsizepanel.Visibility = Visibility.Visible;
                DownPanel.Visibility = Visibility.Visible;
            }
            else
            {
                leftsizepanel.Visibility = Visibility.Hidden;
                DownPanel.Visibility = Visibility.Hidden;
            }
        }

        private void MiniButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            player.Play();
        }

        private void MiniButtonPause_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
        }

        /// <summary>
        /// change volume slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Volumeslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = volumeslider.Value;
            volumeslider.AutoToolTipPlacement = System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft;
            volumeslider.ToolTip = (int)volumeslider.Value;
        }

        #region VideoBaseButton
        private void Playvideo_Click(object sender, RoutedEventArgs e)
        {
            player.Play();
        }

        private void Pausevideo_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
        }

        private void Resumevideo_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }
        #endregion

        /// <summary>
        /// load new video and save details about previous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Loadvideo_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentVideoDetails();

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            string pathtomedia = "";

            if (dlg.ShowDialog() == true)
            {
                Uri mynewsourse = new Uri(dlg.FileName, UriKind.Absolute);
                pathtomedia = dlg.FileName;
                player.Source = mynewsourse;
                headervideo.Text = "- FOXvideo - " + System.IO.Path.GetFileName(dlg.FileName);
                current_video = System.IO.Path.GetFileName(dlg.FileName);
                player.Play();
                BigPlayButton.Visibility = Visibility.Hidden;
                borderplaybutton.Visibility = Visibility.Hidden;
            }

            AddVideoToCollection(dlg);
        }

        /// <summary>
        /// if video already in list finf info
        /// else add base data
        /// </summary>
        /// <param name="dlg"></param>
        /// <param name="pathtomedia"></param>
        public void AddVideoToCollection(OpenFileDialog dlg)
        {
            if (IsDublicate(System.IO.Path.GetFileName(dlg.FileName)))
            {
                #region Add Save Moments(time value)

                List<string> temp_savemoment = GetCollection(Directory.GetCurrentDirectory() + logs + save_mom + "\\" + current_video + "_sm");

                foreach (var item in temp_savemoment)
                {
                    savemoments.Add(TimeSpan.Parse(item));
                }
                #endregion

                #region Add Save Comments(string value)

                savecomments = GetCollection(Directory.GetCurrentDirectory() + logs + save_com + "\\" + current_video + "_sc");

                #endregion

                for (int i = 0; i < names.Count; i++)
                {
                    if (names.ToList()[i] == System.IO.Path.GetFileName(dlg.FileName))
                    {
                        CurrentVideoIndex = i;
                    }
                }

                BasicRatingBar.Value = Convert.ToInt32(starcount.ToList()[CurrentVideoIndex]);
            }
            else
            {
                names.Add(System.IO.Path.GetFileName(dlg.FileName));
                media_path.Add(dlg.FileName);
            }
            current_video = System.IO.Path.GetFileName(dlg.FileName);

            ExpanderSvMoment.ItemsSource = null;
            ExpanderSvComent.ItemsSource = null;

            ExpanderSvMoment.ItemsSource = savemoments;
            ExpanderSvComent.ItemsSource = savecomments;
        }

        /// <summary>
        /// read from file (from your path) and create string collection
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static List<string> GetCollection(string path)
        {
            List<string> resalt_collecrtion = new List<string>();

            if (File.Exists(path))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] b = new byte[fs.Length];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    fs.Read(b, 0, (int)fs.Length);

                    string temp2 = temp.GetString(b);
                    string resalt = "";
                    string symb = "\n";

                    for (int i = 0; i < temp2.Length; i++)
                    {

                        if (((char)temp2[i]).ToString() == symb)
                        {
                            if (resalt != "")
                            {
                                resalt_collecrtion.Add(resalt);
                                resalt = "";
                            }
                        }
                        else
                        {
                            resalt += temp2[i];
                        }
                    }
                }
            }

            return resalt_collecrtion;
        }

        /// <summary>
        /// Save after work with current video its details (savemoments, comments, rating, lastmoment)
        /// </summary>
        private void SaveCurrentVideoDetails()
        {
            try
            {
                //save moments
                using (FileStream fs = File.Create(Directory.GetCurrentDirectory() + logs + save_mom + "\\" + current_video + "_sm"))
                {
                    foreach (var item in savemoments)
                    {
                        byte[] info = Encoding.UTF8.GetBytes(Convert.ToString(item + "\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }

                //save comments
                using (FileStream fs = File.Create(Directory.GetCurrentDirectory() + logs + save_com + "\\" + current_video + "_sc"))
                {
                    foreach (var item in savemoments)
                    {
                        byte[] info = Encoding.UTF8.GetBytes(Convert.ToString(item + "\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }

            }
            catch (Exception)
            {

            }

            if (names.Count != starcount.Count)
            {
                starcount.Add(BasicRatingBar.Value.ToString() + "\n");
            }
            else if (names.Count != lastmoments.Count)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    if (names.ToList()[i] == current_video)
                    {
                        //lastmoments.ToList()[i] = player.Position.ToString() + "\n";
                        try
                        {
                            starcount.ToList()[i] = BasicRatingBar.Value.ToString() + "\n";
                        }
                        catch (Exception)
                        {
                            starcount.Add(BasicRatingBar.Value.ToString() + "\n");
                        }
                    }
                }
            }
            else
            {
                starcount.Add(BasicRatingBar.Value.ToString() + "\n");
            }

            if (lastmoments.Count == 0)
            {
                lastmoments.Add(player.Position.ToString() + "\n");
            }
            else if (names.Count != lastmoments.Count)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    if (names.ToList()[i] == current_video)
                    {
                        try
                        {
                            lastmoments.ToList()[i] = player.Position.ToString() + "\n";
                        }
                        catch (Exception)
                        {
                            lastmoments.Add(player.Position.ToString() + "\n");
                        }
                    }
                }
            }
            else
            {
                lastmoments.Add(player.Position.ToString() + "\n");
            }

            savemoments.Clear();
            savecomments.Clear();
        }

        /// <summary>
        /// if you change position slider player must change position too (and tooltip)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Videoslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Position = TimeSpan.FromSeconds(videoslider.Value);
            videoslider.AutoToolTipPlacement = System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft;
            videoslider.ToolTip = (int)videoslider.Value;
        }

        /// <summary>
        /// for drop video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideroPlayerApp_Drop(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
            player.Source = new Uri(filename);

            player.LoadedBehavior = MediaState.Manual;
            player.Volume = (double)volumeslider.Value;
            player.Play();
            headervideo.Text = "- FOXvideo - " + System.IO.Path.GetFileName(filename);
            BigPlayButton.Visibility = Visibility.Hidden;
            borderplaybutton.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// for timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan tx = player.NaturalDuration.TimeSpan;
            videoslider.Maximum = tx.TotalSeconds;
        }

        /// <summary>
        /// if you change position slider volume must change position too
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Volumeslider_MouseEnter(object sender, MouseEventArgs e)
        {
            volumeslider.AutoToolTipPlacement = System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft;
            volumeslider.ToolTip = (int)volumeslider.Value;
        }

       /// <summary>
       /// tooltip for slider (plyer)
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Videoslider_MouseEnter(object sender, MouseEventArgs e)
        {
            videoslider.AutoToolTipPlacement = System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft;
            videoslider.ToolTip = (int)videoslider.Value;
        }

        private void BigPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (BigPlayButton.Visibility == Visibility.Visible)
            {
                player.Play();
                BigPlayButton.Visibility = Visibility.Hidden;
                borderplaybutton.Visibility = Visibility.Hidden;
            }
            else
            {
                player.Pause();
                BigPlayButton.Visibility = Visibility.Visible;
                borderplaybutton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// start and stop player by touch enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Borderplaybutton_TouchEnter(object sender, TouchEventArgs e)
        {
            if (BigPlayButton.Visibility == Visibility.Visible)
            {
                player.Play();
                BigPlayButton.Visibility = Visibility.Hidden;
                borderplaybutton.Visibility = Visibility.Hidden;
            }
            else
            {
                player.Pause();
                BigPlayButton.Visibility = Visibility.Visible;
                borderplaybutton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// start and stop player by mouse enter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Player_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (BigPlayButton.Visibility == Visibility.Visible)
            {
                player.Play();
                BigPlayButton.Visibility = Visibility.Hidden;
                borderplaybutton.Visibility = Visibility.Hidden;
            }
            else
            {
                player.Pause();
                BigPlayButton.Visibility = Visibility.Visible;
                borderplaybutton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Show panel with special instruments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpecialInstrument_Click(object sender, RoutedEventArgs e)
        {
            if (SpecialSettingsPanel.Visibility == Visibility.Visible)
            {
                SpecialSettingsPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                SpecialSettingsPanel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Save time and moment when you chech special button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveMoment_Checked(object sender, RoutedEventArgs e)
        {
            player.Pause();
            savemoments.Add(player.Position);
            savecomments.Add(CommentTextBox.Text);
            ExpanderSvMoment.ItemsSource = savemoments;
            ExpanderSvComent.ItemsSource = savecomments;
        }

        /// <summary>
        /// If you want start watch from moment that you choose from your saved moments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpanderSvMoment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedLang = ExpanderSvMoment.SelectedItem;

                player.Position = (TimeSpan)selectedLang;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Save resalts when program end
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideroPlayerApp_Closed(object sender, EventArgs e)
        {
            if (names.Count != 0 && current_video != "")
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Directory.GetCurrentDirectory() + logs);
                }

                if (!Directory.Exists(Directory.GetCurrentDirectory() + logs + save_mom))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Directory.GetCurrentDirectory() + logs + save_mom);
                }

                if (!Directory.Exists(Directory.GetCurrentDirectory() + logs + save_com))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Directory.GetCurrentDirectory() + logs + save_com);
                }

                string filename = Directory.GetCurrentDirectory() + "\\" + current_video;

                //save names
                using (FileStream fs = File.Open(Directory.GetCurrentDirectory() + logs + logs_NameCollection, FileMode.OpenOrCreate))
                {
                    foreach (var item in names)
                    {
                        byte[] info = Encoding.UTF8.GetBytes(Convert.ToString(item + "\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }

                //save path
                using (FileStream fs = File.Open(Directory.GetCurrentDirectory() + logs + pathes, FileMode.OpenOrCreate))
                {
                    foreach (var item in media_path)
                    {
                        byte[] info = Encoding.UTF8.GetBytes(Convert.ToString(item + "\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }

                if (names.Count != starcount.Count)
                {
                    starcount.Add(BasicRatingBar.Value.ToString() + "\n");
                }
                else if (names.Count != lastmoments.Count)
                {
                    for (int i = 0; i < names.Count; i++)
                    {
                        if (names.ToList()[i] == current_video)
                        {
                            //lastmoments.ToList()[i] = player.Position.ToString() + "\n";
                            starcount.ToList()[i] = BasicRatingBar.Value.ToString() + "\n";
                            //string temp = lastmoments.ToList()[i];
                            //lastmoments.Remove(temp);
                            //lastmoments.Rep(i, player.Position.ToString() + "\n");
                        }
                    }
                }
                else
                {
                    starcount.Add(BasicRatingBar.Value.ToString() + "\n");
                }
                //save star count
                using (FileStream fs = File.Open(Directory.GetCurrentDirectory() + logs + starcounts, FileMode.OpenOrCreate))
                {
                    foreach (var item in starcount)
                    {
                        byte[] info = Encoding.UTF8.GetBytes(Convert.ToString(item + "\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }

                //save last moment
                using (FileStream fs = File.Open(Directory.GetCurrentDirectory() + logs + lastmoment, FileMode.OpenOrCreate))
                {
                    foreach (var item in lastmoments)
                    {
                        byte[] info = Encoding.UTF8.GetBytes(Convert.ToString(item + "\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }

                //save moments
                using (FileStream fs = File.Open(Directory.GetCurrentDirectory() + logs + save_mom + "\\" + current_video + "_sm", FileMode.OpenOrCreate))
                {
                    foreach (var item in savemoments)
                    {
                        byte[] info = Encoding.UTF8.GetBytes(Convert.ToString(item + "\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }

                //save comments
                using (FileStream fs = File.Open(Directory.GetCurrentDirectory() + logs + save_com + "\\" + current_video + "_sc", FileMode.OpenOrCreate))
                {
                    foreach (var item in savecomments)
                    {
                        byte[] info = Encoding.UTF8.GetBytes(Convert.ToString(item + "\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
        }
    }
}
