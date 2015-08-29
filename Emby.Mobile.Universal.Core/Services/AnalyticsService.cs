using System;
using Emby.Mobile.Core.Interfaces;
using GoogleAnalytics;

namespace Emby.Mobile.Universal.Core.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        public void PageLoad(string pageName)
        {
            SendView(pageName);
        }

        public void SendUnhandledException(Exception ex)
        {
            SendException(ex, true);
        }

        private static void SendView(string view)
        {
            EasyTracker.GetTracker().SendView(view);
        }

        private static void SendEvent(string eventName)
        {
            EasyTracker.GetTracker().SendEvent(string.Empty, eventName, null, 0);
        }

        private static void SendException(Exception ex, bool isFatal)
        {
            EasyTracker.GetTracker().SendException(ex.Message, isFatal);
        }
    }
}
