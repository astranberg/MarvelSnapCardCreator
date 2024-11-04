using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MarvelSnapCardCreator.ViewModels;

namespace MarvelSnapCardCreator.Views;

public partial class MainWindow : Window
{
    private bool _isDraggingBackground = false;
    private Point _lastMousePosition;
    private TranslateTransform _translateTransform;
    private ScaleTransform _scaleTransform;
    private double _currentScale = 1.0;
    private const double ZOOM_FACTOR = 1.1;

    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainViewModel();
        viewModel.CardCanvas = CardCanvas;
        DataContext = viewModel;

        // Initialize transforms
        _translateTransform = (TranslateTransform)((TransformGroup)BackgroundImage.RenderTransform)
            .Children.First(tr => tr is TranslateTransform);
        _scaleTransform = (ScaleTransform)((TransformGroup)BackgroundImage.RenderTransform)
            .Children.First(tr => tr is ScaleTransform);
    }

    private void BackgroundImage_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            _isDraggingBackground = true;
            _lastMousePosition = e.GetPosition(MaskedContainer);
            BackgroundImage.CaptureMouse();
        }
    }

    private void BackgroundImage_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isDraggingBackground)
        {
            Point currentPosition = e.GetPosition(MaskedContainer);
            Vector offset = currentPosition - _lastMousePosition;
            
            _translateTransform.X += offset.X;
            _translateTransform.Y += offset.Y;
            
            _lastMousePosition = currentPosition;
        }
    }

    private void BackgroundImage_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_isDraggingBackground)
        {
            _isDraggingBackground = false;
            BackgroundImage.ReleaseMouseCapture();
        }
    }

    private void BackgroundImage_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        Point mousePos = e.GetPosition(BackgroundImage);

        double zoomFactor = e.Delta > 0 ? ZOOM_FACTOR : 1 / ZOOM_FACTOR;
        _currentScale *= zoomFactor;

        _scaleTransform.ScaleX = _currentScale;
        _scaleTransform.ScaleY = _currentScale;

        // Adjust position to zoom toward mouse position
        Point relative = BackgroundImage.TransformToVisual(MaskedContainer).Transform(mousePos);
        _translateTransform.X -= (relative.X * (zoomFactor - 1));
        _translateTransform.Y -= (relative.Y * (zoomFactor - 1));
    }
}