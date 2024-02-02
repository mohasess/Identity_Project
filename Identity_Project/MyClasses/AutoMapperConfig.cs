using AutoMapper;
using Identity_Project.Areas.Admin.Models.DTOs.BlogDTOs;
using Identity_Project.Areas.Admin.Models.DTOs.RoleDTOs;
using Identity_Project.Areas.Admin.Models.DTOs.UserDTOs;
using Identity_Project.Models;
using Identity_Project.Models.DTOs;
namespace Identity_Project.Services
{
    public class AutoMapperConfig
    {
        public static IMapper mapper;

        public static void Configuration()
        {
            MapperConfiguration configuration = new MapperConfiguration(t => 
            {
                t.CreateMap<User, RegisterDTO>();
                t.CreateMap<RegisterDTO,User>();
                t.CreateMap<UserEditDTO, User>();
                t.CreateMap<User, UserEditDTO>();
                t.CreateMap<Role, RoleListDTO>();
                t.CreateMap<RoleListDTO, Role>();
                t.CreateMap<RoleCreateDTO, Role>();
                t.CreateMap<Role, RoleCreateDTO>();
                t.CreateMap<RoleEditDTO, Role>();
                t.CreateMap<Role, RoleEditDTO>();
                t.CreateMap<User, UserListDTO>();
                t.CreateMap<UserListDTO, User>();
                t.CreateMap<MyAccountInfoDTO, User>();
                t.CreateMap<User, MyAccountInfoDTO>();
                t.CreateMap<Blog, BlogListDTO>();
                t.CreateMap<BlogListDTO, Blog>();
                t.CreateMap<Blog, BlogCreateDTO>();
                t.CreateMap<BlogCreateDTO, Blog>();
                t.CreateMap<BlogEditDTO, Blog>();
                t.CreateMap<Blog, BlogEditDTO>();

            });
            mapper = configuration.CreateMapper();
        }
    }
}