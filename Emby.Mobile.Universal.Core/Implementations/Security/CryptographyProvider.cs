using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using MediaBrowser.ApiInteraction.Cryptography;

namespace Emby.Mobile.Universal.Core.Implementations.Security
{
    public class CryptographyProvider : ICryptographyProvider
    {
        public byte[] CreateMD5(byte[] value)
        {
            return CreateHash(value, HashAlgorithmNames.Md5);
        }

        public byte[] CreateSha1(byte[] value)
        {
            return CreateHash(value, HashAlgorithmNames.Sha1);
        }

        private byte[] CreateHash(byte[] value, string algo)
        {
            var input = value.AsBuffer();
            // hash it...
            var hasher = HashAlgorithmProvider.OpenAlgorithm(algo);
            IBuffer hashed = hasher.HashData(input);

            byte[] output = null;
            // format it...
            CryptographicBuffer.CopyToByteArray(hashed, out output);

            return output;
        }
    }
}