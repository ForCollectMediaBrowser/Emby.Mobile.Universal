using Emby.Mobile.Core.Playback;
using Windows.UI.Xaml.Media;

namespace Emby.Mobile.Universal.Extensions
{
    public static class MediaElementStateExtensions
    {
        public static PlayerState ToPlayerState(this MediaElementState state)
        {
            switch (state)
            {
                case MediaElementState.Playing:
                    return PlayerState.Playing;
                case MediaElementState.Stopped:
                    return PlayerState.Stopped;
                case MediaElementState.Paused:
                    return PlayerState.Paused;
                case MediaElementState.Opening:
                    return PlayerState.Opening;
                case MediaElementState.Closed:
                    return PlayerState.Closed;
                case MediaElementState.Buffering:
                    return PlayerState.Buffering;
            }
            return PlayerState.Unknown;
        }
    }
}
