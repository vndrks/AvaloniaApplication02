using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Windows.Input;
using Tmds.DBus.Protocol;
using Avalonia.Interactivity;
using Avalonia;

namespace AvaloniaIntroUI.Views;

public partial class MainView : UserControl
{
    private bool _IsDown;
    private bool _IsDragging;

    private double _OriginalLeft;
    private double _OriginalTop;

    private double _MouseX;
    private double _MouseY;

    private System.Windows.Point _StartPoint;   // @to be determined (recommend : using avalonia point)

    private PointerPoint _LastMousePosition;
    private Control? _SelControl;
    private OverlayControl? _OverlayControl;

    public MainView()
    {
        InitializeComponent();
        InitializeControls();   // User Defined

        var adornerButton = this.FindControl<Rectangle>("NAME_A");

        if (adornerButton != null)
            AdornerLayer.SetAdorner(adornerButton, null);

        var btn_ok = this.FindControl<Button>("BTN_OK");
        var btn_cancel = this.FindControl<Button>("BTN_CANCEL");

        var tb_name = this.FindControl<TextBlock>("TB_NAME");
        var tb_birth = this.FindControl<TextBlock>("TB_BIRTH");
        var tb_gender = this.FindControl<TextBlock>("TB_GENDER");
        var tb_email = this.FindControl<TextBlock>("TB_EMAIL");
        var tb_phone = this.FindControl<TextBlock>("TB_PHONE");

        if (btn_ok != null && btn_cancel != null)
        {
            btn_ok.Click += (s, e) =>
            {
                // todo

            };

            btn_cancel.Click += (s, e) =>
            {
                // todo
            };
        }
    }

    private void CheckForNull([NotNull] params object?[] args)
    {
        foreach ( var arg in args )
        {
            ArgumentNullException.ThrowIfNull(arg);
        }
    }

    private ResizablePanel? _Panel_00;
    private void InitializeControls()
    {
        _Panel_00 = this.FindControl<ResizablePanel>("Panel_00");
        var panel_01 = this.FindControl<ResizablePanel>("Panel_01");
        var panel_02 = this.FindControl<ResizablePanel>("Panel_02");

        var panel_10 = this.FindControl<ResizablePanel>("Panel_10");
        var panel_11 = this.FindControl<ResizablePanel>("Panel_11");
        // var panel_12 = this.FindControl<ResizablePanel>("Panel12");

        try
        {
            CheckForNull(_Panel_00, panel_01, panel_02, panel_10, panel_11);
        }
        catch (ArgumentNullException e)
        {
            // @To do throw 
            Debug.WriteLine(e.ToString());
            throw new ArgumentNullException(nameof(e));
        }

        _Panel_00.Resize += (sender, e) => {
            var newHeight = _Panel_00.Bounds.Height;
            var newWidth = _Panel_00.Bounds.Width;
            panel_01.Height = newHeight;
            panel_02.Height = newHeight;

            panel_10.Width = newWidth;
        };

        panel_01.Resize += (sender, e) => {
            var newHeight = panel_01.Bounds.Height;
            var newWidth = panel_01.Bounds.Width;

            _Panel_00.Height = newHeight;
            panel_02.Height = newHeight;

            panel_11.Width = newWidth + panel_02.Width;
        };

        panel_02.Resize += (sender, e) => {
            var newHeight = panel_02.Bounds.Height;
            var newWidth = panel_02.Bounds.Width;

            _Panel_00.Height = newHeight;
            panel_01.Height = newHeight;

            panel_11.Width = newWidth + panel_01.Width;
        };

        panel_10.Resize += (sender, e) => {
            var newHeight = panel_10.Bounds.Height;
            var newWidth = panel_10.Bounds.Width;

            panel_11.Height = newHeight;
            //panel_12.Height = newHeight;

            _Panel_00.Width = newWidth;
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
        _LastMousePosition = e.GetCurrentPoint(this);

        if (sender is Control control)
        {
            if (control.Name == "Panel_00")
            {
                _IsDown = true;
                _StartPoint.X = e.GetCurrentPoint(sender as Control).Position.X;
                _StartPoint.Y = e.GetCurrentPoint(sender as Control).Position.Y;

                _SelControl = e.Source as Control;
                e.Handled = true;
            }

            if (control.Name == "FloatingCanvas")
            {
                _IsDown = true;
                _StartPoint.X = e.GetCurrentPoint(sender as Control).Position.X;
                _StartPoint.Y = e.GetCurrentPoint(sender as Control).Position.Y;

                _SelControl = e.Source as Control;

                e.Handled = true;
            }
            else
            {
                _IsDown = true;
                _StartPoint.X = e.GetCurrentPoint(sender as Control).Position.X;
                _StartPoint.Y = e.GetCurrentPoint(sender as Control).Position.Y;

                e.Handled = true;
            }
        }
    }

    private void EH_MouseMoved(object? sender, PointerEventArgs e)
    {
        _MouseX = e.GetCurrentPoint(sender as Control).Position.X;
        _MouseY = e.GetCurrentPoint(sender as Control).Position.Y;

        /**
         * @Function : Get pt(Avalonia.Input.PointerPoint) Transform Rendering
         */
        if (_OverlayControl != null)
        {
            //var delta_x = e.GetCurrentPoint(_OverlayControl).Position.X - _LastMousePosition.Position.X;
            //var delta_y = e.GetCurrentPoint(_OverlayControl).Position.Y - _LastMousePosition.Position.Y;
            var delta_x = e.GetCurrentPoint(_OverlayControl).Position.X - _LastMousePosition.Position.X;
            var delta_y = e.GetCurrentPoint(_OverlayControl).Position.Y - _LastMousePosition.Position.Y;

            // _OverlayControl.UpdatePosition(delta_x, delta_y);
        }

        /**
         * @Function : Resizing Panel
         */
        if (_IsDown)
        {
            if ((_IsDragging == false) &&
                ((Math.Abs(_MouseX - _StartPoint.X) > System.Windows.SystemParameters.MinimumHorizontalDragDistance) ||
                 (Math.Abs(_MouseY - _StartPoint.Y) > System.Windows.SystemParameters.MinimumVerticalDragDistance)))
            {
                DragStarted();
            }

            if (_IsDragging)
            {
                DragMoved(e);
                Debug.WriteLine($"### Drag Moved X : {_MouseX}, Y : {_MouseY}");
            }
        }
    }

    private void EH_MouseReleased(object? sender, PointerReleasedEventArgs e)
    {
        /**
         * @How to use PointerPoint
         */
        var point = e.GetCurrentPoint(sender as Control);

        var x = point.Position.X;
        var y = point.Position.Y;

        var msg = $"[Released] Coordinates X, Y : ({x}, {y}) relative to sender.";

        if (point.Properties.IsLeftButtonPressed)
            msg += " Left button pressed.";

        if (point.Properties.IsRightButtonPressed)
            msg += " Right button pressed.";
        
        if (_IsDown)
        {
            DragFinished(e.GetCurrentPoint(sender as Control));
            e.Handled = true;
        }
    }

    private void EH_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        _LastMousePosition = e.GetCurrentPoint(this);

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
            if (cancelled == false && _SelControl != null)
            {
                Canvas.SetTop(_SelControl, popt.Position.Y);
                Canvas.SetLeft(_SelControl, popt.Position.X);
            }
            _SelControl = null;
            if (_TopPanel != null)
                _TopPanel.Children.Remove(_OverlayControl.GetOverlay());
        }
        _IsDragging = false;
        _IsDown = false;
    }

    private StackPanel? _StackPanel;
    private Panel? _TopPanel;
    private Canvas? _Canvas;
    private void DragStarted()
    {
        _IsDragging = true;
        if (_SelControl != null && _IsDown == true && !_Panel_00.IsResizing)
        {
            _OriginalLeft = Canvas.GetLeft(_SelControl);
            _OriginalTop = Canvas.GetTop(_SelControl);

            _OverlayControl = new OverlayControl(_SelControl);
            _OverlayControl.UpdatePosition(_SelControl.Bounds.X, _SelControl.Bounds.Y);

            _StackPanel = this.FindControl<StackPanel>("HomeStackPanel");
            _TopPanel = this.FindControl<Panel>("TopPanel");
            _Canvas = this.FindControl<Canvas>("FloatingCanvas");

            /* Test Code 
            var textBlock = new TextBlock
            {
                Text = "Hello, Avalonia!",
                Foreground = Brushes.Black,
                FontSize = 24
            };
            
            Canvas.SetLeft(textBlock, 100);
            Canvas.SetTop(textBlock, 100);

            _Canvas.Children.Add(_OverlayControl);
            */

            if (_TopPanel != null)
                _TopPanel.Children.Add(_OverlayControl.GetOverlay());
            
        }
    }

    private void DragMoved(PointerEventArgs e)
    {
        var point = e.GetPosition(this);

        // var currentPosition = Mouse.GetPosition(MyCanvas);

        //_OverlayControl.LeftOffset = _MouseX;
        //_OverlayControl.TopOffset = _MouseY;

        double diff_x = _MouseX - _LastMousePosition.Position.X;
        double diff_y = _MouseY - _LastMousePosition.Position.Y;

        if (_OverlayControl != null && !_Panel_00.IsResizing)
        {
            _OverlayControl.UpdatePosition(diff_x, diff_y);
        }
        //if (_TopPanel != null)
        //    _TopPanel.Children.Add(_OverlayControl.GetOverlay());
    }
}
