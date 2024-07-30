namespace AvaloniaIntroUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public SubViewModel SubVM { get; } = new SubViewModel();
}
