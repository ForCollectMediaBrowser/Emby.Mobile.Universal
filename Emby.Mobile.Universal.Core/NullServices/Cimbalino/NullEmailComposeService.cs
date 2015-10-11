using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;

namespace Emby.Mobile.Universal.Core.NullServices.Cimbalino
{
    public class NullEmailComposeService : IEmailComposeService
    {
        public Task ShowAsync(string subject, string body)
        {
            return Task.FromResult(0);
        }

        public Task ShowAsync(string to, string subject, string body)
        {
            return Task.FromResult(0);
        }

        public Task ShowAsync(string to, string cc, string bcc, string subject, string body)
        {
            return Task.FromResult(0);
        }
    }
}