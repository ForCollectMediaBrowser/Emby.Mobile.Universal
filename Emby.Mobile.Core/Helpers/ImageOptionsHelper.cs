using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;

namespace Emby.Mobile.Core.Helpers
{
    public static class ImageOptionsHelper
    {
        public static ImageOptions SearchHint { get; } = GetOptions(ImageType.Primary, 50);

        private static ImageOptions GetOptions(ImageType imageType, int maxWidth)
        {
            var options = new ImageOptions
            {
                Quality = 80,
                ImageType = imageType,
                Format = ImageFormat.Png,
                MaxWidth = 50
            };

            return options;
        }
    }
}
