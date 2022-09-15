using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Models
{
    public class AccessToken : IModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
