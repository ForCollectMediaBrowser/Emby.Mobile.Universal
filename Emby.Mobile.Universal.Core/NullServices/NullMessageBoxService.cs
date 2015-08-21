using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullMessageBoxService : IMessageBoxService
    {
        public Task ShowAsync(string text)
        {
            throw new NotImplementedException();
        }

        public Task ShowAsync(string text, string caption)
        {
            throw new NotImplementedException();
        }

        public Task<int> ShowAsync(string text, string caption, IEnumerable<string> buttons)
        {
            throw new NotImplementedException();
        }
    }
}