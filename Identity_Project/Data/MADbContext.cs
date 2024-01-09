using Identity_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Identity_Project.Areas.Admin.Models.DTOs.UserDTOs;

namespace Identity_Project.Data
{
    public class MADbContext : IdentityDbContext<User,Role,string>
    {
        public MADbContext(DbContextOptions<MADbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>().HasKey(keys => new { keys.LoginProvider, keys.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(keys => new { keys.UserId, keys.RoleId });
            builder.Entity<IdentityUserToken<string>>().HasKey(keys => new { keys.UserId, keys.LoginProvider, keys.Name });

            builder.Entity<User>().Ignore(p => p.NormalizedEmail);
        }

        public DbSet<UserListDTO> UserListDTO { get; set; } = default!;

        public DbSet<UserEditDTO> UserEditDTO { get; set; } = default!;
    }
}
  