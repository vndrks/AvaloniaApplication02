using Avalonia.Controls;

namespace AvaloniaIntroUI.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();        
    }

    private void EH_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        
        var x = point.Position.X;
        var y = point.Position.Y;

        var msg = $" Coordinates X, Y : ({x}, {y}) relative to sender.";

        if (point.Properties.IsLeftButtonPressed)
        {
            msg += " Left button pressed.";
        }
        if (point.Properties.IsRightButtonPressed)
        {
            msg += " Right button pressed.";
        }
        TB_COORDINATE.Text = msg;
    }
}
