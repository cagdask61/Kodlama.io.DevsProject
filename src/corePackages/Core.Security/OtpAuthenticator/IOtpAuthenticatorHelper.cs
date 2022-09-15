using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.OtpAuthenticator
{
    public interface IOtpAuthenticatorHelper
    {
        Task<byte[]> GenerateSecretKeyAsync();
        Task<string> ConvertSecretKetToStringAsync(byte[] secretKey);
        Task<bool> VerifyCodeAsync(byte[] secretKey, string code);
    }
}
