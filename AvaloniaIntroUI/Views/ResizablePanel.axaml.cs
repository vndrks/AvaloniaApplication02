using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;

namespace AvaloniaIntroUI.Views;

public partial class ResizablePanel : Panel
{
    public static readonly RoutedEvent<RoutedEventArgs> ResizeEvent =
        RoutedEvent.Register<ResizablePanel, RoutedEventArgs>(nameof(Resize), RoutingStrategies.Bubble);


    private bool _IsResizing;
    private Point _LastMousePosition;

    public event EventHandler<RoutedEventArgs> Resize
    {
        add => AddHandler(ResizeEvent, value);
        remove => RemoveHandler(ResizeEvent, value);
    }

    public ResizablePanel()
    {
        InitializeComponent();

        this.PointerPressed += OnPointerPressed;
        this.PointerReleased += OnPointerReleased;
        this.PointerMoved += OnPointerMoved;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        foreach (var child in Children)
        {
            child.Measure(availableSize);
        }
        // Debug.WriteLine($"### availableSize Width : {availableSize.Width}, Height : {availableSize.Height}");
        
        if (_IsResizing)
            return new Size(availableSize.Width, availableSize.Height);
        else
            return base.MeasureOverride(availableSize);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        foreach (var child in Children)
        {
            child.Arrange(new Rect(finalSize));
        }

        // Debug.WriteLine($"### Final Size Width : {finalSize.Width}, Height : {finalSize.Height}");
        return finalSize;
        //return base.ArrangeOverride(finalSize);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetPosition(this);
        if (IsOnResizeEdge(point).isEdge)
        {
            _IsResizing = true;
            _LastMousePosition = point;
            e.Pointer.Capture(this);
        }
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var point = e.GetPosition(this);
        Debug.WriteLine($"### Coordinates X : {point.X}, Y : {point.Y}");

        if (_IsResizing)
        {
            var delta = point - _LastMousePosition;

            //var newWidth = this.Width + delta.X;
            //var newHeight = this.Height + delta.Y;

            var newWidth = this.Bounds.Width + delta.X;
            var newHeight = this.Bounds.Height + delta.Y;

            if (newWidth > 0 && newHeight > 0)
            {
                this.Width = newWidth;
                this.Height = newHeight;
                _LastMousePosition = point;
                Debug.WriteLine($"### {sender.ToString()} : Width : {this.Width}, Height : {this.Height}");

                RaiseEvent(new RoutedEventArgs(ResizeEvent));
            }
        }
        else
        {
            if (IsOnResizeEdge(point).isEdge)
            {
                //this.Cursor = new Cursor(StandardCursorType.SizeNorthWestSouthEast);
                // this.Cursor = new Cursor(StandardCursorType.BottomRightCorner);
                this.Cursor = new Cursor(IsOnResizeEdge(point).cursorType);
            }
            else
            {
                this.Cursor = new Cursor(StandardCursorType.Arrow);
            }
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        //if (sender != null)
        //    Debug.WriteLine($"### {sender.ToString()}");

        _IsResizing = false;
        e.Pointer.Capture(null);
    }

    private (StandardCursorType cursorType, bool isEdge) IsOnResizeEdge(Point pt)
    {
        const double thickness = 20.0;
        (StandardCursorType, bool) result = (StandardCursorType.Arrow, false);

        //if (pt.X > 0 && pt.X < thickness)
        //    result = (StandardCursorType.LeftSide, true);
        if (pt.X < this.Bounds.Width + thickness && pt.X > this.Bounds.Width - thickness)
            result = (StandardCursorType.RightSide, true);
        else if (pt.X >= this.Bounds.Width - thickness && pt.Y >= this.Bounds.Height - thickness)
            result = (StandardCursorType.BottomRightCorner, true);
        else
            result = (StandardCursorType.Arrow, false);

        return result;
    }
}