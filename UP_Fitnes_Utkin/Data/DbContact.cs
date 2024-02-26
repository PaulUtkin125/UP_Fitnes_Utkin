using Microsoft.EntityFrameworkCore;
using UP_Fitnes_Utkin.Model;

namespace UP_Fitnes_Utkin.Data
{
    internal class DbContact : DbContext
    {

        public DbSet<User> users { get; set; }
        public DbSet<Tovar_Sklad> tovar { get; set; }
        public DbSet<KategorTovara> kategorTovaras { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FitnesShopeDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder md)
        {
            md.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);

            md.Entity<Tovar_Sklad>()
                .HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategID);
        }
    }
}
