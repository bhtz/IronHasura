using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace com.ironhasura.Areas.Identity.Data
{
    public class IdentityDataContext : IdentityDbContext<IdentityUser>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.RenameIdentityTables(builder, "mcsp_");
        }

        private void RenameIdentityTables(ModelBuilder builder, string prefix)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var table = entityType.GetTableName();
                if (table.StartsWith("AspNet"))
                {
                    entityType.SetTableName(prefix + table.Substring(6));
                }
            };
        }

        private void Seed()
        {
            
        }
    }
}
