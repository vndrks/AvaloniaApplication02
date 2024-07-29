using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaIntroUI.ElementModules;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;

using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AvaloniaIntroUI.Views;

public partial class MainView : UserControl
{
    private bool _IsDown;
    private bool _IsDragging;
    private double _OriginalLeft;
    private double _OriginalTop;

    private Point _StartPoint;
    // private UIElement _OriginalElement;
    private Control? _SelControl;

    // private FloatingControl _OverlayElement;
    private LayerController _LayerController;


    public MainView()
    {
        InitializeComponent();
        
        InitializeControls(); // User Defined

        var adornerButton = this.FindControl<Rectangle>("NAME_A");

        if (adornerButton != null)
            AdornerLayer.SetAdorner(adornerButton, null);
        //if (adornerButton is { })
        //{
        //    var adorner = AdornerLayer.GetAdorner(adornerButton);
        //    if (adorner is { })
        //    {
        //        _adorner = adorner;
        //    }
        //    AdornerLayer.SetAdorner(adornerButton, null);
        //}
    }

    private void CheckForNull([NotNull] params object?[] args)
    {
        foreach ( var arg in args )
        {
            ArgumentNullException.ThrowIfNull(arg);
        }
    }

    private void InitializeControls()
    {
        var panel_00 = this.FindControl<ResizablePanel>("Panel_00");
        var panel_01 = this.FindControl<ResizablePanel>("Panel_01");
        var panel_02 = this.FindControl<ResizablePanel>("Panel_02");

        var panel_10 = this.FindControl<ResizablePanel>("Panel_10");
        var panel_11 = this.FindControl<ResizablePanel>("Panel_11");
        // var panel_12 = this.FindControl<ResizablePanel>("Panel12");

        try
        {
            CheckForNull(panel_00, panel_01, panel_02, panel_10, panel_11);
        }
        catch (ArgumentNullException e)
        {
            // @ To do throw 
            Debug.WriteLine(e.ToString());
            throw new ArgumentNullException(nameof(e));
        }

        panel_00.Resize += (sender, e) => {
            var newHeight = panel_00.Bounds.Height;
            var newWidth = panel_00.Bounds.Width;
            panel_01.Height = newHeight;
            panel_02.Height = newHeight;

            panel_10.Width = newWidth;
        };

        panel_01.Resize += (sender, e) => {
            var newHeight = panel_01.Bounds.Height;
            var newWidth = panel_01.Bounds.Width;

            panel_00.Height = newHeight;
            panel_02.Height = newHeight;

            panel_11.Width = newWidth + panel_02.Width;
        };

        panel_02.Resize += (sender, e) => {
            var newHeight = panel_02.Bounds.Height;
            var newWidth = panel_02.Bounds.Width;

            panel_00.Height = newHeight;
            panel_01.Height = newHeight;

            panel_11.Width = newWidth + panel_01.Width;
        };

        panel_10.Resize += (sender, e) => {
            var newHeight = panel_10.Bounds.Height;
            var newWidth = panel_10.Bounds.Width;

            panel_11.Height = newHeight;
            //panel_12.Height = newHeight;

            panel_00.Width = newWidth;
        };

        panel_11.Resize += (sender, e) => {
            var newHeight = panel_11.Bounds.Height;
            var newWidth = panel_11.Bounds.Width;

            panel_10.Height = newHeight;
            //panel_12.Height = newHeight;

            panel_01.Width = newWidth / 2;
            panel_02.Width = newWidth / 2;
        };

        //panel_12.Resize += (sender, e) =>
        //{
        //    var newHeight = panel_12.Bounds.Height;
        //    var newWidth = panel_12.Bounds.Width;

        //    panel_10.Height = newHeight;
        //    panel_11.Height = newHeight;

        //    panel_02.Width = newWidth;
        //};
    }

    private void EH_MouseButtonDown(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Control control)
        {
            if (control.Name == "FloatingCanvas")
            {
                _IsDown = true;
                _StartPoint.X = e.GetCurrentPoint(sender as Control).Position.X;
                _StartPoint.Y = e.GetCurrentPoint(sender as Control).Position.Y;

                _SelControl = e.Source as Control;

                // _OriginalElement = e.Source as UIElement;
                e.Handled = true;
            }
            else
            {
                _IsDown = true;
                _StartPoint.X = e.GetCurrentPoint(sender as Control).Position.X;
                _StartPoint.Y = e.GetCurrentPoint(sender as Control).Position.Y;

                // _OriginalElement = e.Source as UIElement;
                // FloatingCanvas.CaptureMouse();
                e.Handled = true;
            }
        }
        //if (e.Source == FloatingCanvas)
        //{
        //}
        //else
        //{

        //    _IsDown = true;
        //    _StartPoint = e.GetPosition(FloatingCanvas);
        //    _OriginalElement = e.Source as UIElement;
        //    // FloatingCanvas.CaptureMouse();
        //    e.Handled = true;

        //}
    }

    private double _mouse_x;
    private double _mouse_y;

    private void EH_MouseMoved(object? sender, PointerEventArgs e)
    {
        _mouse_x = e.GetCurrentPoint(sender as Control).Position.X;
        _mouse_y = e.GetCurrentPoint(sender as Control).Position.Y;

        if (_IsDown)
        {
            if ((_IsDragging == false) &&
                ((Math.Abs(_mouse_x - _StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                 (Math.Abs(_mouse_y - _StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
            {
                DragStarted();
                Debug.WriteLine("### DragStart");
            }

            if (_IsDragging)
            {
                DragMoved();
                Debug.WriteLine($"### Drag Moved X : {_mouse_x}, Y : {_mouse_y}");
            }
        }
    }

    private void EH_MouseReleased(object? sender, PointerReleasedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);

        var x = point.Position.X;
        var y = point.Position.Y;

        var msg = $"[Released] Coordinates X, Y : ({x}, {y}) relative to sender.";

        if (point.Properties.IsLeftButtonPressed)
            msg += " Left button pressed.";

        if (point.Properties.IsRightButtonPressed)
            msg += " Right button pressed.";

        // TB_COORDINATE.Text = msg;
        if (_IsDown)
        {
            DragFinished(e.GetCurrentPoint(sender as Control));
            e.Handled = true;
        }
    }

    private void EH_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        
        var x = point.Position.X;
        var y = point.Position.Y;

        var msg = $"[Pressed] Coordinates X, Y : ({x}, {y}) relative to sender.";

        if (point.Properties.IsLeftButtonPressed)
            msg += " Left button pressed.";

        if (point.Properties.IsRightButtonPressed)
            msg += " Right button pressed.";

        TB_COORDINATE.Text = msg;
    }

    private void DragFinished(PointerPoint popt, bool cancelled = false)
    {
        Mouse.Capture(null);
        if (_IsDragging)
        {
            // AdornerLayer.GetAdornerLayer(_OverlayElement.AdornedElement).Remove(_OverlayElement);
            
            if (cancelled == false && _SelControl != null)
            {
                Canvas.SetTop(_SelControl, popt.Position.Y);
                Canvas.SetLeft(_SelControl, popt.Position.X);
            }
            _SelControl = null;
        }
        _IsDragging = false;
        _IsDown = false;
    }

    private Control? _adorner;

    private void DragStarted()
    {
        _IsDragging = true;
        if (_SelControl != null && _IsDown == true)
        {
            _OriginalLeft = Canvas.GetLeft(_SelControl);
            _OriginalTop = Canvas.GetTop(_SelControl);

            _LayerController = new LayerController(_SelControl);

            var adornerButton = this.FindControl<Rectangle>("NAME_A");
            AdornerLayer.SetAdorner(adornerButton, _adorner);
        }
    }

    private void DragMoved()
    {
        // var currentPosition = Mouse.GetPosition(MyCanvas);

        _LayerController.LeftOffset = _mouse_x;
        _LayerController.TopOffset = _mouse_y;
    }
}
