using cookie_stand_api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cookie_stand_api.Data
{
    public class CookieSalmonDbContext : IdentityDbContext<ApplicationUser>
    {
        public CookieSalmonDbContext(DbContextOptions options):base(options) { }

        public DbSet<CookieStand> CookieStands { get; set; }

        public DbSet<HourlySales> HourlySales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HourlySales>().HasKey(

                hourlySales => new
                {
                    hourlySales.ID,
                    hourlySales.CookieStandID
                });

            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>()
    {
            new IdentityUserRole<string> { UserId ="1" , RoleId = "admin manager" },
            new IdentityUserRole<string> { UserId = "2", RoleId = "hotel manager" } ,
            new IdentityUserRole<string> { UserId = "3", RoleId = "trip manager" } ,
            new IdentityUserRole<string> { UserId = "4", RoleId = "user" }    };
            SeedRole(modelBuilder, "Admin", "Create","Read","Update","Delete");
            SeedRole(modelBuilder, "User", "Read");
        }
        int nextId = 1;
        private void SeedRole(ModelBuilder modelBuilder, string roleName, params string[] permissions)
        {
            var role = new IdentityRole()
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };

            modelBuilder.Entity<IdentityRole>().HasData(role);

            var roleClaim = permissions.Select(permission =>
            new IdentityRoleClaim<string>
            {
                Id = nextId++,
                RoleId = role.Id,
                ClaimType = "permissions",
                ClaimValue = permission
            }).ToArray();

            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(roleClaim);
        }

    }
}