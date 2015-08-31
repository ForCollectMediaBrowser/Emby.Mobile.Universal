using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Controls;
using Emby.Mobile.Universal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emby.Mobile.Universal.Services
{
    public class StatusBarService : IStatusBarService
    {
        public void DisplayError(string message)
        {
            SystemTrayControl.GetForCurrentView()?.DisplayProgress(message, StatusType.Error);
        }


        public void DisplayMessage(string message)
        {
            SystemTrayControl.GetForCurrentView()?.DisplayProgress(message, StatusType.Message);
        }

        public void DisplayIndeterminateStatus(string message)
        {
            SystemTrayControl.GetForCurrentView()?.DisplayProgress(message, StatusType.Status);
        }

        public void DisplaySuccess(string message)
        {
            SystemTrayControl.GetForCurrentView()?.DisplayProgress(message, StatusType.Success);
        }

        public void DisplayWarning(string message)
        {
            SystemTrayControl.GetForCurrentView()?.DisplayProgress(message, StatusType.Warning);
        }
    }
}
