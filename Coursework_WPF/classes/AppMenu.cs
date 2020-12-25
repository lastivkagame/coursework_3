//using Coursework_WPF.windows;
//using DAL;
using Homework_3_videoplayer_;
using SimpleCrypto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Coursework_WPF.classes
{
    internal sealed class AppMenu : INotifyPropertyChanged
    {
        #region Commands
        private readonly Command settingswindow;
        private readonly Command perssettingswindow;
        private readonly Command videopplayerwindow;
        private readonly Command historyvideowindow;
        private readonly Command mainwindow;
        private readonly Command addvideo;
        private readonly Command browsercomand;
        private readonly Command showVideo;
        private readonly Command refreshCommand;

        public ICommand SettingCommand => settingswindow;
        public ICommand PerssetCommand => perssettingswindow;
        public ICommand MainCommand => mainwindow;
        public ICommand VideoCommand => videopplayerwindow;
        public ICommand AddVideo => addvideo;
        public ICommand VideoHistoryCommand => historyvideowindow;
        public ICommand BrowserCommand => browsercomand;
        public ICommand ShowVideo => showVideo;
        public ICommand RefreshCommand => refreshCommand;
        #endregion

        #region Save File Names
        public string logs = "\\logs";
        public string logs_NameCollection = "\\Collection";
        public string pathes = "\\path";
        public string lastmoment = "\\lastmomemt";
        public string starcounts = "\\star_counts";
        public string save_mom = "\\save_moments";
        public string save_com = "\\save_coments";
        #endregion

        #region Collections
        public ICollection<VideoDetails> videoDetails = new ObservableCollection<VideoDetails>();
        public IEnumerable<VideoDetails> VideoDetails => videoDetails;
        public VideoDetails SelectedVideo { get; set; }

        public List<string> videocollecrtion = new List<string>();
        public List<string> videocollecrtionpath = new List<string>();
        public List<string> videolastmoment = new List<string>();

        public List<string> image_collection = new List<string>()
        {
            Directory.GetCurrentDirectory() + "\\..\\..\\windows\\Images\\pine_trees.png",
            Directory.GetCurrentDirectory() + "\\..\\..\\windows\\Images\\imag2 _musicplayer.png",
            Directory.GetCurrentDirectory() + "\\..\\..\\windows\\Images\\imag3_musicplayer.png",
            Directory.GetCurrentDirectory() + "\\..\\..\\windows\\Images\\imag4_musicplayer.png",
            Directory.GetCurrentDirectory() + "\\..\\..\\windows\\Images\\imag5_musicplayer.png"
        };

        public Random random = new Random();
        #endregion

        #region Panels
        private string setting_panel_visible;

        public string Setting_panel_visible
        {
            get { return setting_panel_visible; }
            set
            {
                setting_panel_visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Setting_panel_visible)));
            }
        }

        public string Personalization_panel_visible
        {
            get { return setting_panel_visible; }
            set
            {
                setting_panel_visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Personalization_panel_visible)));
            }
        }

        public string HistVideo_panel_visible
        {
            get { return setting_panel_visible; }
            set
            {
                setting_panel_visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(HistVideo_panel_visible)));
            }
        }
        #endregion

        public AppMenu()
        {
            Setting_panel_visible = "Hidden";
            Personalization_panel_visible = "Hidden";

            #region Show and Hidden some panels

            settingswindow = new DelegateCommand(() => { Setting_panel_visible = "Visible"; Personalization_panel_visible = "Hidden"; HistVideo_panel_visible = "Hidden"; });
            mainwindow = new DelegateCommand(() => Setting_panel_visible = Personalization_panel_visible = HistVideo_panel_visible = "Hidden");
            perssettingswindow = new DelegateCommand(() => { Personalization_panel_visible = "Visible"; HistVideo_panel_visible = "Hidden"; });
            historyvideowindow = new DelegateCommand(() => { Setting_panel_visible = "Visible"; Personalization_panel_visible = "Hidden"; HistVideo_panel_visible = "Visible"; });

            #endregion

            addvideo = new DelegateCommand(() =>
            {
                App.Current.MainWindow.WindowState = WindowState.Minimized;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";

                if (dlg.ShowDialog() == true)
                {
                    //fill parametrs video(info about video)
                    Uri mynewsourse = new Uri(dlg.FileName, UriKind.Absolute);
                    Videoplayer videoplayer = new Videoplayer();
                    videoplayer.WindowState = WindowState.Normal;
                    videoplayer.Show();
                    videoplayer.WindowState = WindowState.Maximized;

                    videoplayer.player.Source = mynewsourse;
                    videoplayer.headervideo.Text = "- FOXvideo - " + System.IO.Path.GetFileName(dlg.FileName);
                    videoplayer.current_video = System.IO.Path.GetFileName(dlg.FileName);

                    videoplayer.savemoments.Clear();
                    videoplayer.savecomments.Clear();

                    //fill collection parametrs(info about videos)(we read and fill all collections, work with it next save it again)
                    videoplayer.names = GetCollection(Directory.GetCurrentDirectory() + logs + logs_NameCollection);
                    videoplayer.media_path = GetCollection(Directory.GetCurrentDirectory() + logs + pathes);
                    videoplayer.lastmoments = GetCollection(Directory.GetCurrentDirectory() + logs + lastmoment);
                    videoplayer.starcount = GetCollection(Directory.GetCurrentDirectory() + logs + starcounts);
                    videoplayer.AddVideoToCollection(dlg);
                    
                    //video setting 
                    videoplayer.player.Play();
                    videoplayer.BigPlayButton.Visibility = Visibility.Hidden;
                    videoplayer.borderplaybutton.Visibility = Visibility.Hidden;

                    //show in list in main vindow
                    videoDetails.Add(new classes.VideoDetails(new BitmapImage
                        (new Uri(image_collection[random.Next(0, image_collection.Count())])),
                        System.IO.Path.GetFileName(dlg.FileName), "recently", dlg.FileName));
                }
            });

            //you can watch video that you choose in list(was selected) similary by addvideo command
            showVideo = new DelegateCommand(() =>
            {
                if (SelectedVideo.VideoName != "")
                {
                    App.Current.MainWindow.WindowState = WindowState.Minimized;

                    Uri mynewsourse = new Uri(SelectedVideo.Media_path, UriKind.Absolute);
                    Videoplayer videoplayer = new Videoplayer();

                    videoplayer.WindowState = WindowState.Normal;
                    videoplayer.Show();
                    videoplayer.WindowState = WindowState.Maximized;
                    videoplayer.player.Source = mynewsourse;
                    videoplayer.headervideo.Text = "- FOXvideo - " + System.IO.Path.GetFileName(SelectedVideo.VideoName);
                    videoplayer.current_video = System.IO.Path.GetFileName(SelectedVideo.VideoName);

                    videoplayer.savemoments.Clear();
                    videoplayer.savecomments.Clear();

                    Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
                    op.FileName = SelectedVideo.Media_path;

                    videoplayer.names = GetCollection(Directory.GetCurrentDirectory() + logs + logs_NameCollection);
                    videoplayer.media_path = GetCollection(Directory.GetCurrentDirectory() + logs + pathes);
                    videoplayer.lastmoments = GetCollection(Directory.GetCurrentDirectory() + logs + lastmoment);
                    videoplayer.starcount = GetCollection(Directory.GetCurrentDirectory() + logs + starcounts);
                    videoplayer.AddVideoToCollection(op);

                    videoplayer.player.Play();
                    videoplayer.BigPlayButton.Visibility = Visibility.Hidden;
                    videoplayer.borderplaybutton.Visibility = Visibility.Hidden;
                }
                else
                {
                    System.Windows.MessageBox.Show("Nonone selected");
                }

            });

            //simple fill collections and open player
            videopplayerwindow = new DelegateCommand(() =>
            {
                App.Current.MainWindow.WindowState = WindowState.Minimized;
                Videoplayer videoplayer = new Videoplayer();

                videoplayer.WindowState = WindowState.Normal;
                videoplayer.Show();

                //videoplayer.WindowState = WindowState.Maximized;
                videoplayer.WindowState = WindowState.Maximized;

                videoplayer.headervideo.Text = "- FOXvideo - please choose media: menu->load ";
                videoplayer.BigPlayButton.Visibility = Visibility.Hidden;
                videoplayer.borderplaybutton.Visibility = Visibility.Hidden;

                videoplayer.names = GetCollection(Directory.GetCurrentDirectory() + logs + logs_NameCollection);
                videoplayer.media_path = GetCollection(Directory.GetCurrentDirectory() + logs + pathes);
                videoplayer.lastmoments = GetCollection(Directory.GetCurrentDirectory() + logs + lastmoment);
                videoplayer.starcount = GetCollection(Directory.GetCurrentDirectory() + logs + starcounts);

            });

            //refresh list in main window
            refreshCommand = new DelegateCommand(() =>
            {
                try
                {
                    videoDetails.Clear();
                    //save names, path, lastmoments
                    videocollecrtion = GetCollection(Directory.GetCurrentDirectory() + logs + logs_NameCollection);
                    videocollecrtionpath = GetCollection(Directory.GetCurrentDirectory() + logs + pathes);
                    videolastmoment = GetCollection(Directory.GetCurrentDirectory() + logs + lastmoment);

                    try
                    {
                        for (int i = 0; i < videocollecrtion.Count; i++)
                        {
                            videoDetails.Add(new classes.VideoDetails(new BitmapImage
                            (new Uri(image_collection[random.Next(0, image_collection.Count())])),
                            videocollecrtion[i], videolastmoment[i], videocollecrtionpath[i]));
                        }
                    }
                    catch (Exception)
                    {
                        for (int i = 0; i < videocollecrtion.Count; i++)
                        {
                            videoDetails.Add(new classes.VideoDetails(new BitmapImage
                           (new Uri(image_collection[random.Next(0, image_collection.Count())])),
                           videocollecrtion[i], DateTime.Now.ToShortTimeString(), videocollecrtionpath[i]));
                        }
                    }

                }
                catch (Exception)
                {
                    // System.Windows.MessageBox.Show(ex.Message);
                }
            });

            //open browser
            browsercomand = new DelegateCommand(() =>
            {
                try
                {
                    string file = Directory.GetCurrentDirectory() + "\\..\\..\\browser\\Coursework_WPF.exe";
                    App.Current.MainWindow.WindowState = WindowState.Minimized;

                    Process.Start(file);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

            });

            #region FillVIdeoHistory
            try
            {
                videoDetails.Add(new classes.VideoDetails(new BitmapImage(new Uri(image_collection[random.Next(0, image_collection.Count())])), "Happy Holidays", "now", "by goldefox"));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            try
            {
                //save names, path, lastmoments
                videocollecrtion = GetCollection(Directory.GetCurrentDirectory() + logs + logs_NameCollection);
                videocollecrtionpath = GetCollection(Directory.GetCurrentDirectory() + logs + pathes);
                videolastmoment = GetCollection(Directory.GetCurrentDirectory() + logs + lastmoment);

                try
                {
                    for (int i = 0; i < videocollecrtion.Count; i++)
                    {
                        videoDetails.Add(new classes.VideoDetails(new BitmapImage
                        (new Uri(image_collection[random.Next(0, image_collection.Count())])),
                        videocollecrtion[i], videolastmoment[i], videocollecrtionpath[i]));
                    }
                }
                catch (Exception)
                {
                    for (int i = 0; i < videocollecrtion.Count; i++)
                    {
                        videoDetails.Add(new classes.VideoDetails(new BitmapImage
                       (new Uri(image_collection[random.Next(0, image_collection.Count())])),
                       videocollecrtion[i], DateTime.Now.ToShortTimeString(), videocollecrtionpath[i]));
                    }
                }

            }
            catch (Exception)
            {
                // System.Windows.MessageBox.Show(ex.Message);
            }
            #endregion
        }

        /// <summary>
        /// Read file and do string collection by it
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
                    //resalt_collecrtion.Add(resalt);
                }
            }

            return resalt_collecrtion;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
