using MediaBrowser.Model.Dto;
using System;
using MediaBrowser.Model.Dlna;
using System.Threading.Tasks;
using MediaBrowser.ApiInteraction.Playback;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Core.Playback
{
    public class PlaylistItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public BaseItemDto Item { get; set; }
        public PlaylistState State { get; set; }

        public PlaylistItem(BaseItemDto item)
        {
            Item = item;
            State = PlaylistState.Queued;
        }

        public async Task<StreamInfo> GetStreamInfoAsync(int startPositionTicks, bool isOffline, IPlaybackManager playbackManager, IApiClient apiClient, DeviceProfile profile)
        {
            if (Item.IsAudio)
            {
                var audioOptions = GetAudioOptions(Item, GetMaxAudioBitrate(), profile, apiClient.DeviceId);
                var streamInfo = await playbackManager.GetAudioStreamInfo(Item.ServerId, audioOptions, isOffline, apiClient);
                streamInfo.StartPositionTicks = startPositionTicks;
                return streamInfo;
            }

            throw new NotImplementedException();
        }

        private AudioOptions GetAudioOptions(BaseItemDto item, int? audioBitrate, DeviceProfile profile, string deviceId)
        {
            var options = new AudioOptions()
            {
                Profile = profile,
                ItemId = item.Id,
                DeviceId = deviceId,
                MaxBitrate = audioBitrate,
                MaxAudioChannels = 2,
                MediaSources = item.MediaSources
            };

            options.MediaSourceId = item.MediaSources?[0]?.Id;
            return options;
        }

        private static int GetMaxAudioBitrate()
        {
            //TODO Use application Transcode settings, if null default to 2512000
            return 2512000;
        }
    }
}
