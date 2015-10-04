using System;
using Windows.Foundation;

namespace Emby.Mobile.Universal.Extensions
{
    public static class TaskExtensions
    {
        public static void DontAwait(this IAsyncAction task, string reasonForNotAwaiting)
        {
            if (string.IsNullOrEmpty(reasonForNotAwaiting))
            {
                throw new ArgumentNullException(nameof(reasonForNotAwaiting), "Give a reason!!!!!");
            }
        }
    }
}