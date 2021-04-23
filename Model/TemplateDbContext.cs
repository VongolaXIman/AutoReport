using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class TemplateDbContext : DbContext, ITemplateDbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
          : base(options)
        {
        }

        public TemplateDbContext()
        {
        }

        //public DbSet<TableClass> TableClass { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var tableClass = modelBuilder.Entity<TableClass>();
        }
    }
}
