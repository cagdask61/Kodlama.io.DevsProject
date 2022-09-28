using Core.Persistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {

        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        protected IConfiguration Configuration { get; }

        public BaseDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgrammingLanguage>(builder =>
            {
                builder.ToTable("ProgrammingLanguages");
                builder.Property(pl => pl.Id).HasColumnName("Id");
                builder.Property(pl => pl.Name).HasColumnName("Name");
                builder.Property(pl => pl.Description).HasColumnName("Description");
                builder.Property(pl => pl.Status).HasColumnName("Status");
                builder.Property(pl => pl.CreatedDate).HasColumnName("CreatedDate");
                builder.Property(pl => pl.UpdatedDate).HasColumnName("UpdatedDate");
            });

            //ProgrammingLanguage[] programmingLanguagesSeeds = { new(1,"C#","net 6",true), new(2, "C++", "test", true) };

            //modelBuilder.Entity<ProgrammingLanguage>().HasData(programmingLanguagesSeeds);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<CommonEntity>();

            foreach (var item in entities)
            {
                _ = item.State switch
                {
                    EntityState.Added => item.Entity.CreatedDate = DateTime.Now,
                    EntityState.Modified => item.Entity.UpdatedDate = DateTime.Now,
                    _ => DateTime.Now
                };
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
