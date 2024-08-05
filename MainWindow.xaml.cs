using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MarvelSnapCardCreator
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _selectedCost;
        public string SelectedCost
        {
            get { return _selectedCost; }
            set { _selectedCost = value; OnPropertyChanged(); }
        }

        private string _selectedPower;
        public string SelectedPower
        {
            get { return _selectedPower; }
            set { _selectedPower = value; OnPropertyChanged(); }
        }

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged(); }
        }

        private List<string> _borderOptions;
        public List<string> BorderOptions
        {
            get { return _borderOptions; }
            set { _borderOptions = value; OnPropertyChanged(); }
        }

        private string _selectedBorderOption;
        public string SelectedBorderOption
        {
            get { return _selectedBorderOption; }
            set { _selectedBorderOption = value; OnPropertyChanged(); }
        }

        private List<string> _logoOptions;
        public List<string> LogoOptions
        {
            get { return _logoOptions; }
            set { _logoOptions = value; OnPropertyChanged(); }
        }

        private string _selectedLogoOption;
        public string SelectedLogoOption
        {
            get { return _selectedLogoOption; }
            set { _selectedLogoOption = value; OnPropertyChanged(); }
        }
// Set the image sources in the MainWindow class
        private BitmapImage _logoImageSource;
        public BitmapImage LogoImageSource
        {
            get { return _logoImageSource; }
            set { _logoImageSource = value; OnPropertyChanged(); }
        }

        private BitmapImage _costImageSource;
        public BitmapImage CostImageSource
        {
            get { return _costImageSource; }
            set { _costImageSource = value; OnPropertyChanged(); }
        }

        private BitmapImage _powerImageSource;
        public BitmapImage PowerImageSource
        {
            get { return _powerImageSource; }
            set { _powerImageSource = value; OnPropertyChanged(); }
        }

        private BitmapImage _frameImageSource;
        public BitmapImage FrameImageSource
        {
            get { return _frameImageSource; }
            set { _frameImageSource = value; OnPropertyChanged(); }
        }

        private BitmapImage _backgroundImageSource;
        public BitmapImage BackgroundImageSource
        {
            get { return _backgroundImageSource; }
            set { _backgroundImageSource = value; OnPropertyChanged(); }
        }

        private BitmapImage _maskImageSource;
        public BitmapImage MaskImageSource
        {
            get { return _maskImageSource; }
            set { _maskImageSource = value; OnPropertyChanged(); }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize your properties here
            BorderOptions = new List<string> { "Border1", "Border2", "Border3" };
            LogoOptions = new List<string> { "Logo1", "Logo2", "Logo3" };
            
            // Set the image paths
            LogoImageSource = new BitmapImage(new Uri(@"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Logos\JeanGrey_Logo.png"));
            CostImageSource = new BitmapImage(new Uri(@"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\card-cost.png"));
            PowerImageSource = new BitmapImage(new Uri(@"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\card-power.png"));
            FrameImageSource = new BitmapImage(new Uri(@"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Frames\card-frame-common.png"));
            BackgroundImageSource = new BitmapImage(new Uri(@"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Backgrounds\jean.jpg"));
            MaskImageSource = new BitmapImage(new Uri(@"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\card-frame-mask-bg.png"));
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            // Logic to handle the upload image button click event
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}