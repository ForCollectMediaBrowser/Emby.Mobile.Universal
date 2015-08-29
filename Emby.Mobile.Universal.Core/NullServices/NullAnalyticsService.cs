using System;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullAnalyticsService : IAnalyticsService
    {
        public void PageLoad(string pageName)
        {
        }

        public void SendUnhandledException(Exception ex)
        {
        }
    }
}