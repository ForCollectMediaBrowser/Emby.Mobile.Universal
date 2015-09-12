using System.Linq;
using System.Reflection;

namespace Emby.Mobile.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void CopyItem<T>(this T source, T destination) where T : class
        {
            var destinationProperties = destination.GetType().GetRuntimeProperties().ToList();
            foreach (var sourcePropertyInfo in source.GetType().GetRuntimeProperties())
            {
                var destPropertyInfo = destinationProperties.FirstOrDefault(x => x.Name == sourcePropertyInfo.Name);

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
