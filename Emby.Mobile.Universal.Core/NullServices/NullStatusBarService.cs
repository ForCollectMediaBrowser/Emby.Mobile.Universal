using Emby.Mobile.Core.Interfaces;
using System;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullStatusBarService : IStatusBarService
    {
        public void DisplayError(string message)
        {
            throw new NotImplementedException();
        }

        public void DisplayIndeterminateStatus(string message)
        {
            throw new NotImplementedException();
        }

        public void DisplayMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void DisplaySuccess(string message)
        {
            throw new NotImplementedException();
        }

        public void DisplayWarning(string message)
        {
            throw new NotImplementedException();
        }
    }
}
