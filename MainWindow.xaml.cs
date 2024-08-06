using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace MarvelSnapCardCreator
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string FrameImagesFolder = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Frames";
        private string LogoImagesFolder = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Logos";
        private string BackgroundImagesFolder = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Backgrounds";
        public string MaskImagePath = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\mask.jpg";
        private string CostImagePath = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\card-cost.png";
        private string PowerImagePath = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\card-power.png";
        
        private Point origin;
        private Point start;
        
        private string _selectedFrameOption;

        public ObservableCollection<string> FrameOptions { get; set; }

        public string SelectedFrameOption
        {
            get { return _selectedFrameOption; }
            set
            {
                if (_selectedFrameOption != value)
                {
                    _selectedFrameOption = value;
                    OnPropertyChanged(nameof(SelectedFrameOption));
                    HandleSelectedFrameOptionChange();
                }
            }
        }

        private void HandleSelectedFrameOptionChange()
        {
            Console.WriteLine($"Selected Frame Option: {SelectedFrameOption}");
            if (SelectedFrameOption != null)
            {
                string selectedFrame = System.IO.Path.Combine(FrameImagesFolder, SelectedFrameOption);
                FrameImage.Source = new BitmapImage(new Uri(selectedFrame));
            }
        }
        
        private string _selectedLogoOption;

        public ObservableCollection<string> LogoOptions { get; set; }

        public string SelectedLogoOption
        {
            get { return _selectedLogoOption; }
            set
            {
                if (_selectedLogoOption != value)
                {
                    _selectedLogoOption = value;
                    OnPropertyChanged(nameof(SelectedLogoOption));
                    HandleSelectedLogoOptionChange();
                }
            }
        }

        private void HandleSelectedLogoOptionChange()
        {
            Console.WriteLine($"Selected Frame Option: {SelectedLogoOption}");
            if (SelectedLogoOption != null)
            {
                string selectedLogo = System.IO.Path.Combine(LogoImagesFolder, SelectedLogoOption);
                LogoImage.Source = new BitmapImage(new Uri(selectedLogo));
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize your properties here
            //BorderOptions = new List<string> { "Border1", "Border2", "Border3" };
            //LogoOptions = new List<string> { "Logo1", "Logo2", "Logo3" };
            FrameOptions = new ObservableCollection<string>();
            
            // Set the image paths
            LogoImageSource = new BitmapImage(new Uri($@"{LogoImagesFolder}\JeanGrey_Logo.png"));
            CostImageSource = new BitmapImage(new Uri(CostImagePath));
            PowerImageSource = new BitmapImage(new Uri(PowerImagePath));
            FrameImageSource = new BitmapImage(new Uri($@"{FrameImagesFolder}\card-frame-common.png"));
            BackgroundImageSource = new BitmapImage(new Uri($@"{BackgroundImagesFolder}\jean.jpg"));
            MaskImageSource = new BitmapImage(new Uri(MaskImagePath));
            
            // Load the dropdown images
            LoadFrameImages();
            LoadLogoImages();

            LoadCostImage();
            LoadPowerImage();
            LoadBackgroundImage();
            LoadMaskImage();
        }

        private void LoadMaskImage()
        {
            if (File.Exists(MaskImagePath))
            {
                CostImage.Source = new BitmapImage(new Uri(MaskImagePath));
            }
            else
            {
                MessageBox.Show("Mask image not found.");
            }
        }

        private void LoadBackgroundImage()
        {
            if (Directory.Exists(BackgroundImagesFolder))
            {
                BackgroundImage.Source = new BitmapImage(new Uri($@"{BackgroundImagesFolder}\jean.jpg"));
            }
            else
            {
                MessageBox.Show("Background images folder not found.");
            }
        }

        private void LoadCostImage()
        {
            if (File.Exists(CostImagePath))
            {
                CostImage.Source = new BitmapImage(new Uri(CostImagePath));
            }
            else
            {
                MessageBox.Show("Frame images folder not found.");
            }
        }
        private void LoadPowerImage()
        {
            if (File.Exists(PowerImagePath))
            {
                PowerImage.Source = new BitmapImage(new Uri(PowerImagePath));
            }
            else
            {
                MessageBox.Show("Frame power file not found.");
            }
        }

        public BitmapImage PowerImageSource { get; set; }

        public BitmapImage CostImageSource { get; set; }

        public BitmapImage LogoImageSource { get; set; }

        public BitmapImage FrameImageSource { get; set; }

        public BitmapImage BackgroundImageSource { get; set; }

        public BitmapImage MaskImageSource { get; set; }

        private void LoadFrameImages()
        {
            if (Directory.Exists(FrameImagesFolder))
            {
                var frameFiles = Directory.GetFiles(FrameImagesFolder, "*.png");
                FrameOptions = new ObservableCollection<string>(frameFiles.Select(file => System.IO.Path.GetFileName(file)));
            }
            else
            {
                MessageBox.Show("Frame images folder not found.");
            }
        }
        private void LoadLogoImages()
        {
            if (Directory.Exists(LogoImagesFolder))
            {
                var logoFiles = Directory.GetFiles(LogoImagesFolder, "*.png");
                LogoOptions = new ObservableCollection<string>(logoFiles.Select(file => System.IO.Path.GetFileName(file)));
            }
            else
            {
                MessageBox.Show("Frame images folder not found.");
            }
        }

        private void LoadImage(System.Windows.Controls.Image imageControl)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                imageControl.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void ExportAsImage_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)CardCanvas.ActualWidth, (int)CardCanvas.ActualHeight,
                96d, 96d, PixelFormats.Pbgra32);
            renderBitmap.Render(CardCanvas);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Files (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }
        }

        /*
         
                    <Image.RenderTransform>
                        <TransformGroup>
                            <ScaleTransform x:Name="BackgroundScaleTransform"/>
                            <TranslateTransform x:Name="BackgroundTranslateTransform"/>
                        </TransformGroup>
                    </Image.RenderTransform>
                    
         private void BackgroundImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            start = e.GetPosition(CardCanvas);
            origin = new Point(BackgroundTranslateTransform.X, BackgroundTranslateTransform.Y);
            element.CaptureMouse();
        }

        private void BackgroundImage_MouseMove(object sender, MouseEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element.IsMouseCaptured)
            {
                Vector v = start - e.GetPosition(CardCanvas);
                BackgroundTranslateTransform.X = origin.X - v.X;
                BackgroundTranslateTransform.Y = origin.Y - v.Y;
            }
        }

        private void BackgroundImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            element.ReleaseMouseCapture();
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //BackgroundScaleTransform.ScaleX = e.NewValue;
            //BackgroundScaleTransform.ScaleY = e.NewValue;
        }*/

        private void CostComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CostComboBox.SelectedItem != null)
            {
                CostText.Text = (CostComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem).Content.ToString();
            }
        }

        private void PowerComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PowerComboBox.SelectedItem != null)
            {
                PowerText.Text = (PowerComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem).Content.ToString();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}