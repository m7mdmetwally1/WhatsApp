using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace infrastructure.Data.Configuration;

    public class RoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "Adminstrator",
                    NormalizedName = "ADMINSTRATOR"
                },
                 new IdentityRole
                 {
                     Name = "User",
                     NormalizedName = "USER"
                 }

                );
        }
    }

    