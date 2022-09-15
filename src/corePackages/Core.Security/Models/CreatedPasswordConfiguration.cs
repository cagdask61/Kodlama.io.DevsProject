using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Models
{
    public class CreatedPasswordConfiguration : IModel
    {
        public CreatedPasswordConfiguration()
        {

        }
        public CreatedPasswordConfiguration(byte[] passwordSalt, byte[] passwordHash)
        {
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
        }

        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
