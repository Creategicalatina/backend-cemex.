using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace back_end_cemex.Entities.InsertData
{
    public class RolesInsert : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "PoweUser",
                    NormalizedName = "POWERUSER"
                },
                new IdentityRole
                {
                    Name = "CemexAdminLogis",
                    NormalizedName = "CEMEXADMINLOGIS"
                },
                new IdentityRole
                {
                    Name = "AdminLogis",
                    NormalizedName = "ADMINLOGIS"
                },
                 new IdentityRole
                 {
                     Name = "ManTruck",
                     NormalizedName = "MANTRUCK"
                 },
                new IdentityRole
                {
                    Name = "Driver",
                    NormalizedName = "DRIVER"
                }
            );
        }
    }
}
