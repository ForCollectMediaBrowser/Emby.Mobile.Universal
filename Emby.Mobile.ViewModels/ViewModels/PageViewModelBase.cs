using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        protected PageViewModelBase(IServices services) : base(services)
        {
        }

        public RelayCommand PageLoadedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await PageLoaded();
                });
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await Refresh();
                });
            }
        }

        protected virtual Task PageLoaded()
        {
            return Task.FromResult(0);
        }

        public virtual void OnNavigatedTo(NavigationMode mode, BaseItemDto item)
        {
        }

        public virtual void OnNavigatingFrom(NavigationMode mode, BaseItemDto item, ref bool isCancelled)
        {
        }

        public virtual void OnNavigatedFrom(NavigationMode mode, BaseItemDto item)
        {
        }
    }
}