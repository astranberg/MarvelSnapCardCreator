using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MarvelSnapCardCreator.ViewModels;

namespace MarvelSnapCardCreator.Views;

public partial class MainWindow : Window
{
    private bool isDragging = false;
    private Point clickPosition;

    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainViewModel();
        viewModel.CardCanvas = CardCanvas;
        DataContext = viewModel;

        // Attach mouse event handlers
        BackgroundImage.MouseDown += BackgroundImage_MouseDown;
        BackgroundImage.MouseMove += BackgroundImage_MouseMove;
        BackgroundImage.MouseUp += BackgroundImage_MouseUp;
    }

    
    private Point _startPoint;
    private bool _isDragging = false;
    private double _originalLeft;
    private double _originalTop;
    
    private void BackgroundImage_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            _startPoint = e.GetPosition(this);
            _originalLeft = Canvas.GetLeft(BackgroundImage);
            _originalTop = Canvas.GetTop(BackgroundImage);
            _isDragging = true;
            BackgroundImage.CaptureMouse();
        }
    }

    private void BackgroundImage_MouseMove(object sender, MouseEventArgs e)
    {

        if (_isDragging)
        {
            Point currentPoint = e.GetPosition(this);
            double offsetX = currentPoint.X - _startPoint.X;
            double offsetY = currentPoint.Y - _startPoint.Y;

            Canvas.SetLeft(BackgroundImage, _originalLeft + offsetX);
            Canvas.SetTop(BackgroundImage, _originalTop + offsetY);
        }
    }

    private void BackgroundImage_MouseUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        BackgroundImage.ReleaseMouseCapture();
    }
}