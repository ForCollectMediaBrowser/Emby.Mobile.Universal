using System;
using System.Threading.Tasks;

namespace Emby.Mobile.Core.Extensions
{
    public static class TaskExtensions
    {
        public static void DontAwait(this Task task, string reasonForNotAwaiting)
        {
            if (string.IsNullOrEmpty(reasonForNotAwaiting))
            {
                throw new ArgumentNullException(nameof(reasonForNotAwaiting), "Give a reason!!!!!");
            }
        }
    }
}
