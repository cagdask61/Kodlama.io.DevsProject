using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Entities
{
    public class EmailAuthenticator : CommonEntity
    {
        public int UserId { get; set; }
        public string ActivationKey { get; set; }
        public bool IsVerified { get; set; }

        public virtual User User { get; set; }

        public EmailAuthenticator()
        {

        }

        public EmailAuthenticator(int userId, string activationKey, bool isVerified) : this()
        {
            UserId = userId;
            ActivationKey = activationKey;
            IsVerified = isVerified;
        }
    }
}
