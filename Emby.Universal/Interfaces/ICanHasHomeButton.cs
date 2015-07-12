using System.Windows.Input;

namespace Emby.Universal.Interfaces
{
    public interface ICanHasHomeButton
    {
        bool ShowHomeButton { get; set; }
        ICommand NavigateHomeCommand { get; }
    }
}