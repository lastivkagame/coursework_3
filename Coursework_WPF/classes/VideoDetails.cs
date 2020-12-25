using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Coursework_WPF.classes
{
    public class VideoDetails : INotifyPropertyChanged
    {
        public VideoDetails(BitmapImage bitmapImage, string name, string lastmoment, string path)
        {
            VideoName = name;
            VideoImage = bitmapImage;
            Media_path = path;
            LastMoment = lastmoment;
        }

        public VideoDetails(string name, string path)
        {
            VideoName = name;
            Media_path = path;

        }

       
        public VideoDetails()
        {
            VideoName = "Video";
            media_path = "nonwhere";
        }

        private string videoName;

        public string VideoName
        {
            get { return videoName; }
            set
            {
                videoName = value;
                OnPropertyChanged();
            }
        }

        private string media_path;

        public string Media_path
        {
            get { return media_path; }
            set { media_path = value; OnPropertyChanged(); }
        }

        private BitmapImage videoImage;
        public string pathtoimage;

        private string lastmoment;

        public string LastMoment
        {
            get { return lastmoment; }
            set { lastmoment = value; }
        }


        public BitmapImage VideoImage
        {
            get { return videoImage; }
            set { videoImage = value; OnPropertyChanged(); }
        }

        // Оголошення події оновлення властивості
        public event PropertyChangedEventHandler PropertyChanged;

        // Створення методу OnPropertyChanged для виклику події
        // В якості параметра буде використано ім'я властивості, що його викликала.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
