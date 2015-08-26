using System.ComponentModel;
using System.Windows.Input;

namespace Emby.Mobile.Core.Interfaces
{
    public interface ICanSignIn : INotifyPropertyChanged
    {
        string Username { get; set; }
        string Password { get; set; }
        bool CanSignIn { get; }
        ICommand SignInCommand { get; }
        ICommand SignUpCommand { get; }
        bool IsEmbyConnect { get; }
    }
}
