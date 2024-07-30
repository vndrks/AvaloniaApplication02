using ReactiveUI;

namespace AvaloniaIntroUI.ViewModels
{
    public class SubViewModel : ReactiveObject
    {
        private string? _name = "Jaecheol";
        private string? _birth = string.Empty;
        private string? _gender = "Man";
        private string? _email = "jaecheol@wafour.com";
        private string? _phone = "1025345203";

        public string? Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string? Birth
        {
            get => _name;
            // get => _birth;
            set => this.RaiseAndSetIfChanged(ref _birth, value);
        }

        public string? Gender
        {
            get => _gender;
            set => this.RaiseAndSetIfChanged(ref _gender, value);
        }

        public string? Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        public string? Phone
        {
            get => _phone;
            set => this.RaiseAndSetIfChanged(ref _phone, value);
        }
    }
}
