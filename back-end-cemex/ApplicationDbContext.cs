using back_end_cemex.Entities;
using back_end_cemex.Entities.InsertData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace back_end_cemex
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("users");
            });
            builder.ApplyConfiguration(new RolesInsert());
            builder.ApplyConfiguration(new TypeConveyorInsert());
            builder.ApplyConfiguration(new CompanyInsert());
         
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TypeConveyor> TypeConveyors { get; set; }
        public DbSet<Conveyor> Conveyors { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        //public DbSet<User> Users { get; set; }
    }
}
