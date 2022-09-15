using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Entities
{
    public class OtpAuthenticator : CommonEntity
    {
        public int UserId { get; set; }
        public byte[] SecretKey { get; set; }
        public bool IsVerified { get; set; }

        public virtual User User { get; set; }

        public OtpAuthenticator()
        {

        }

        public OtpAuthenticator(int userId, byte[] secretKey, bool isVerified) : this()
        {
            UserId = userId;
            SecretKey = secretKey;
            IsVerified = isVerified;
        }
    }
}
