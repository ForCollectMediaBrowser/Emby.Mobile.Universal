using System.Threading.Tasks;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IStartUpService
    {
        Task Startup();
        void LoadFrame();
    }
}