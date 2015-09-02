using System.Windows.Input;

namespace Emby.Mobile.Universal.Interfaces
{
    public interface ICanHasHeaderMenu
    {
        bool Show { get; set; }
        ICommand SearchCommand { get; }
    }
}