using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Repositories
{
    public interface ICommonRepository<TEntity>
        where TEntity : CommonEntity
    {
         DbSet<TEntity> Table { get; }
    }
}
