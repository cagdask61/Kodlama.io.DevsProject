using Core.Persistence.Repositories;

namespace Core.Security.Entities
{
    public class OperationClaim : CommonEntity
    {
        public string Name { get; set; }

        public OperationClaim()
        {
        }

        public OperationClaim(int id, string name) : base(id)
        {
            Name = name;
        }
    }
}