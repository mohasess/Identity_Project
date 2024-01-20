using Identity_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Identity_Project.Areas.Admin.Models.DTOs.UserDTOs;
using Identity_Project.Areas.Admin.Models.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Identity_Project.Models.DTOs;

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

            //builder.Entity<User>().Ignore(p => p.NormalizedEmail);

            //builder.Entity<SelectListGroup>().HasNoKey();
            //builder.Entity<SelectListItem>().HasNoKey();
            //builder.Entity<SelectListItem>().Ignore(p => p.Group);
        }

        public DbSet<UserListDTO> UserListDTO { get; set; } = default!;

        public DbSet<UserEditDTO> UserEditDTO { get; set; } = default!;

        public DbSet<RoleListDTO> RoleListDTO { get; set; } = default!;
        public DbSet<AddUserRoleDTO> AddUserRoles { get; set; } = default!;

        public DbSet<Identity_Project.Areas.Admin.Models.DTOs.RoleDTOs.RoleEditDTO> RoleEditDTO { get; set; } = default!;

        public DbSet<Identity_Project.Models.DTOs.MyAccountInfoDTO> MyAccountInfoDTO { get; set; } = default!;

    }
}
  