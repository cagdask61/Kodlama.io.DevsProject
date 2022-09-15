using Core.Persistence.Repositories;
using Core.Security.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Entities
{
    public class User : CommonEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PassswordHash { get; set; }
        public byte[] PassswordSalt { get; set; }
        public bool Status { get; set; }
        public AuthenticationType AuthenticationType { get; set; }

        public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            UserOperationClaims = new HashSet<UserOperationClaim>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public User(string firstName, string lastName, string email, byte[] passswordHash, byte[] passswordSalt, bool status, AuthenticationType authenticationType) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PassswordHash = passswordHash;
            PassswordSalt = passswordSalt;
            Status = status;
            AuthenticationType = authenticationType;
        }
    }
}
