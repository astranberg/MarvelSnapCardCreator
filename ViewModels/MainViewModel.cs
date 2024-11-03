using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace MarvelSnapCardCreator.ViewModels;

public class MainViewModel : INotifyPropertyChanged
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
    private string _selectedLogoOption;

    public ObservableCollection<string> FrameOptions { get; set; }
    public ObservableCollection<string> LogoOptions { get; set; }

    public string SelectedFrameOption
    {
        get => _selectedFrameOption;
        set
        {
            _selectedFrameOption = value;
            OnPropertyChanged(nameof(SelectedFrameOption));
            UpdateFrameImage();
        }
    }

    public string SelectedLogoOption
    {
        get => _selectedLogoOption;
        set
        {
            _selectedLogoOption = value;
            OnPropertyChanged(nameof(SelectedLogoOption));
            UpdateLogoImage();
        }
    }
    
    public string CostText => SelectedCost.ToString();
    private int _selectedCost;
    public int SelectedCost
    {
        get => _selectedCost;
        set
        {
            if (_selectedCost != value)
            {
                _selectedCost = value;
                OnPropertyChanged(nameof(SelectedCost));
                OnPropertyChanged(nameof(CostText));
            }
        }
    }

    public string PowerText => SelectedPower.ToString();
    private int _selectedPower;
    public int SelectedPower
    {
        get => _selectedPower;
        set
        {
            if (_selectedPower != value)
            {
                _selectedPower = value;
                OnPropertyChanged(nameof(SelectedPower));
                OnPropertyChanged(nameof(PowerText));
            }
        }
    }
    
    private BitmapImage _costImageSource;
    public BitmapImage CostImageSource
    {
        get => _costImageSource;
        set
        {
            _costImageSource = value;
            OnPropertyChanged(nameof(CostImageSource));
        }
    }

    private BitmapImage _backgroundImageSource;
    public BitmapImage BackgroundImageSource
    {
        get => _backgroundImageSource;
        set
        {
            _backgroundImageSource = value;
            OnPropertyChanged(nameof(BackgroundImageSource));
        }
    }

    private BitmapImage _powerImageSource;
    public BitmapImage PowerImageSource
    {
        get => _powerImageSource;
        set
        {
            _powerImageSource = value;
            OnPropertyChanged(nameof(PowerImageSource));
        }
    }
    
    private BitmapImage _maskImageSource;
    public BitmapImage MaskImageSource
    {
        get => _maskImageSource;
        set
        {
            _maskImageSource = value;
            OnPropertyChanged(nameof(MaskImageSource));
        }
    }

    private BitmapImage _frameImageSource;
    public BitmapImage FrameImageSource
    {
        get => _frameImageSource;
        set
        {
            _frameImageSource = value;
            OnPropertyChanged(nameof(FrameImageSource));
        }
    }

    private BitmapImage _logoImageSource;
    public BitmapImage LogoImageSource
    {
        get => _logoImageSource;
        set
        {
            _logoImageSource = value;
            OnPropertyChanged(nameof(LogoImageSource));
        }
    }

    public ICommand ExportCommand { get; }

    public UIElement CardCanvas { get; set; } // Added this property

    public MainViewModel()
    {
        FrameOptions = new ObservableCollection<string>();
        LogoOptions = new ObservableCollection<string>();
        LoadFrameOptions();
        LoadLogoOptions();
        LoadImages();
        ExportCommand = new RelayCommand(ExportAsImage);
    }
    
    private void LoadFrameOptions()
    {
        if (Directory.Exists(FrameImagesFolder))
        {
            FrameOptions.Clear(); // Clear existing items
            var frameFiles = Directory.GetFiles(FrameImagesFolder, "*.png");
            foreach (var file in frameFiles)
            {
                FrameOptions.Add(System.IO.Path.GetFileName(file));
            }
        }
        else
        {
            MessageBox.Show("Frame images folder not found.");
        }
    }
    
    private void LoadLogoOptions()
    {
        if (Directory.Exists(LogoImagesFolder))
        {
            LogoOptions.Clear(); // Clear existing items
            var logoFiles = Directory.GetFiles(LogoImagesFolder, "*.png");
            foreach (var file in logoFiles)
            {
                LogoOptions.Add(System.IO.Path.GetFileName(file));
            }
        }
        else
        {
            MessageBox.Show("Logo images folder not found.");
        }
    }
    
    private void LoadImages()
    {
        LoadMaskImage();
        LoadBackgroundImage();
        LoadCostImage();
        LoadPowerImage();
    }

    private void LoadMaskImage()
    {
        if (File.Exists(MaskImagePath))
        {
            MaskImageSource = new BitmapImage(new Uri(MaskImagePath));
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
            BackgroundImageSource = new BitmapImage(new Uri($@"{BackgroundImagesFolder}\jean.jpg"));
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
            CostImageSource = new BitmapImage(new Uri(CostImagePath));
        }
        else
        {
            MessageBox.Show("Cost image not found.");
        }
    }
    private void LoadPowerImage()
    {
        if (File.Exists(PowerImagePath))
        {
            PowerImageSource = new BitmapImage(new Uri(PowerImagePath));
        }
        else
        {
            MessageBox.Show("Power image not found.");
        }
    }

    private void UpdateFrameImage()
    {
        // Update FrameImageSource based on SelectedFrameOption
        string frameImagePath = Path.Combine(FrameImagesFolder, SelectedFrameOption);
        if (File.Exists(frameImagePath))
        {
            FrameImageSource = new BitmapImage(new Uri(frameImagePath));
        }
        else
        {
            MessageBox.Show("Selected frame image not found.");
        }
    }

    private void UpdateLogoImage()
    {
        // Update LogoImageSource based on SelectedLogoOption
        string logoImagePath = Path.Combine(LogoImagesFolder, SelectedLogoOption);
        if (File.Exists(logoImagePath))
        {
            LogoImageSource = new BitmapImage(new Uri(logoImagePath));
        }
        else
        {
            MessageBox.Show("Selected logo image not found.");
        }
    }

    private void ExportAsImage()
    {
        if (CardCanvas == null) return; // Ensure CardCanvas is set

        RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
            (int)CardCanvas.RenderSize.Width, (int)CardCanvas.RenderSize.Height,
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
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}