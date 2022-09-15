using Core.Persistence.Repositories;

namespace Core.Security.Entities
{
    public class RefreshToken : CommonEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? RevokedByToken { get; set; }
        public string? ReasonRevoked { get; set; }

        public virtual User User { get; set; }

        public RefreshToken()
        {

        }

        public RefreshToken(int id, string token, DateTime expires, DateTime created, string createdByIp, DateTime revoked, string? revokedByIp, string? revokedByToken, string? reasonRevoked)
        {
            Id = id;
            Token = token;
            Expires = expires;
            Created = created;
            CreatedByIp = createdByIp;
            Revoked = revoked;
            RevokedByIp = revokedByIp;
            RevokedByToken = revokedByToken;
            ReasonRevoked = reasonRevoked;
        }
    }

}