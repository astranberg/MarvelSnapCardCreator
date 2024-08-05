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
        private string FrameImagesFolder = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Frames";
        private string LogoImagesFolder = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Logos";
        private string BackgroundImagesFolder = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\Backgrounds";
        private string MaskImage = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\card-frame-mask-bg.png";
        private string CostImage = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\card-cost.png";
        private string PowerImage = @"C:\Users\Adam\RiderProjects\MarvelSnapCardCreator\Resources\card-power.png";
        private Point origin;
        private Point start;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize your properties here
            BorderOptions = new List<string> { "Border1", "Border2", "Border3" };
            LogoOptions = new List<string> { "Logo1", "Logo2", "Logo3" };
            
            // Set the image paths
            LogoImageSource = new BitmapImage(new Uri($@"{LogoImagesFolder}\JeanGrey_Logo.png"));
            CostImageSource = new BitmapImage(new Uri(CostImage));
            PowerImageSource = new BitmapImage(new Uri(PowerImage));
            FrameImageSource = new BitmapImage(new Uri($@"{FrameImagesFolder}\card-frame-common.png"));
            BackgroundImageSource = new BitmapImage(new Uri($@"{BackgroundImagesFolder}\jean.jpg"));
            MaskImageSource = new BitmapImage(new Uri(MaskImage));
        }

        private void LoadFrameImages()
        {
            if (Directory.Exists(frameImagesFolder))
            {
                var frameFiles = Directory.GetFiles(frameImagesFolder, "*.png");
                foreach (var file in frameFiles)
                {
                    FrameComboBox.Items.Add(System.IO.Path.GetFileName(file));
                }
            }
            else
            {
                MessageBox.Show("Frame images folder not found.");
            }
        }

        private void FrameComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FrameComboBox.SelectedItem != null)
            {
                string selectedFrame = System.IO.Path.Combine(frameImagesFolder, FrameComboBox.SelectedItem.ToString());
                FrameImage.Source = new BitmapImage(new Uri(selectedFrame));
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
            BackgroundScaleTransform.ScaleX = e.NewValue;
            BackgroundScaleTransform.ScaleY = e.NewValue;
        }

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
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}