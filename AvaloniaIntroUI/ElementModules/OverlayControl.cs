using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Diagnostics;


namespace AvaloniaIntroUI;

public class OverlayControl : UserControl
{
    private readonly Rectangle _Child;
    private double _LeftOffset;
    private double _TopOffset;

    public OverlayControl(Control SelControl)
    {
        _Child = new Rectangle
        {
            Width = SelControl.Bounds.Width,
            Height = SelControl.Bounds.Height,
        };

        Debug.WriteLine($"### _Child Width : {_Child.Width}, _Child Height : {_Child.Height}");

        var brush = new VisualBrush(SelControl);
        _Child.Fill = brush;
    }

    public Control? GetOverlay()
    {
        return _Child;
    }

    public void UpdatePosition(double mx, double my)
    {
        _LeftOffset = Bounds.X + mx;
        _TopOffset = Bounds.Y + my;

        /**
         * @Temporary translation mat code
         */
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(new TranslateTransform(_LeftOffset, _TopOffset));
        
        Debug.WriteLine($"### : Left Offset : {_LeftOffset}, Top Offset : {_TopOffset}");
        
        _Child.RenderTransform = transformGroup;
    }
}