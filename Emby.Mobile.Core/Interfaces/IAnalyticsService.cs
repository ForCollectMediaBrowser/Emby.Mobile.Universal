using System;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IAnalyticsService
    {
        void PageLoad(string pageName);
        void SendUnhandledException(Exception ex);
    }
}