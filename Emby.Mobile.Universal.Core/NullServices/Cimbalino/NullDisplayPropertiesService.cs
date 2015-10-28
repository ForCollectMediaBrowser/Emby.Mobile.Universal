using Cimbalino.Toolkit.Foundation;
using Cimbalino.Toolkit.Services;

namespace Emby.Mobile.Universal.Core.NullServices.Cimbalino
{
    public class NullDisplayPropertiesService : IDisplayPropertiesService
    {
        public float LogicalDpi { get; }
        public float RawDpiX { get; }
        public float RawDpiY { get; }
        public Rect Bounds { get; }
        public Rect PhysicalBounds { get; }
        public float ScreenDiagonal { get; }
        public double RawPixelsPerViewPixel { get; }
    }
}