using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;

namespace Emby.Mobile.Core.Helpers
{
    public class ImageOptionsHelper
    {
        private static IDeviceInfoService _deviceInfoService;

        public static ImageOptions SearchHint { get; } = GetOptions(ImageType.Primary, 50);
        public static ImageOptions UserProfile { get; } = GetOptions(ImageType.Primary, height: 120);

        public static ImageOptions ItemPrimaryLarge { get; } = GetOptions(ImageType.Primary);
        public static ImageOptions ItemPrimaryMedium { get; } = GetOptions(ImageType.Primary, height: 200);
        public static ImageOptions ItemPrimarySmall { get; } = GetOptions(ImageType.Primary, height: 100);
        public static ImageOptions ItemBackdropLarge { get; } = GetOptions(ImageType.Backdrop, height: 500);
        public static ImageOptions ItemBackdropMedium { get; } = GetOptions(ImageType.Backdrop, height: 200);
        public static ImageOptions ItemBackdropMax { get; } = GetOptions(ImageType.Backdrop);
        public static ImageOptions ItemLogo { get; } = GetOptions(ImageType.Logo);
        public static ImageOptions ItemThumbMedium { get; } = GetOptions(ImageType.Thumb, height: 200);
        public static ImageOptions ItemBannerMedium { get; } = GetOptions(ImageType.Banner, height: 200);

        public static void SetDeviceInfoService(IDeviceInfoService deviceInfoService)
        {
            _deviceInfoService = deviceInfoService;
        }

        private static ImageOptions GetOptions(
            ImageType imageType, 
            int? maxWidth = null,
            int? height = null)
        {
            var options = new ImageOptions
            {
                Quality = 80,
                ImageType = imageType,
                Format = ImageFormat.Png,
                MaxWidth = _deviceInfoService?.GetDeviceScaleImageValue(maxWidth) ?? maxWidth,
                MaxHeight = height
            };

            return options;
        }
    }
}
