using System.Reflection;

namespace Emby.Mobile.Universal.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void CopyItem<T>(this T source, T destination) where T : class
        {
            foreach (var sourcePropertyInfo in source.GetType().GetProperties())
            {
                var destPropertyInfo = source.GetType().GetProperty(sourcePropertyInfo.Name);

                if (destPropertyInfo.CanWrite)
                {
                    destPropertyInfo.SetValue(
                        destination,
                        sourcePropertyInfo.GetValue(source, null),
                        null);
                }
            }
        }
    }
}
