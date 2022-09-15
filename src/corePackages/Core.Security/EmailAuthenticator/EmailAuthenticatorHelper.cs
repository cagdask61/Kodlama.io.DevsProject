using System.Security.Cryptography;

namespace Core.Security.EmailAuthenticator
{
    public class EmailAuthenticatorHelper : IEmailAuthenticatorHelper
    {
        public Task<string> CreateEmailActivationCode() => Task.FromResult(Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)));


        public Task<string> CreateEmailActivationKey() => Task.FromResult(RandomNumberGenerator.GetInt32(Convert.ToInt32(Math.Pow(10, 6))).ToString().PadLeft(6, '0'));
    }
}
