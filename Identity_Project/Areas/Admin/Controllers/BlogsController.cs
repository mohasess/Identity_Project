using AutoMapper;
using Identity_Project.Areas.Admin.Models.DTOs.BlogDTOs;
using Identity_Project.Data;
using Identity_Project.Models;
using Identity_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Identity_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogsController : Controller
    {
        private readonly IAuthorizationService _authorizeService;
        private readonly UserManager<User> _userManager;
        private readonly MADbContext _dbContext;
        public BlogsController(MADbContext dbContext, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _dbContext= dbContext;
            _userManager= userManager;
            _authorizeService= authorizationService;
        }
        public IActionResult Index()
        {
            var blogsDTO = _dbContext.Blogs.Select(b => new BlogListDTO
            {
                Id = b.Id,
                Title = b.Title,
                UserId = b.UserId,
                UserName = b.User.UserName,
                WrittenDate = b.WrittenDate
            }).ToList();
            return View(blogsDTO);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogCreateDTO blogCreateDTO)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var blog = AutoMapperConfig.mapper.Map<BlogCreateDTO, Blog>(blogCreateDTO);
            blog.User = user;
            blog.UserId = user.Id;

            _dbContext.Blogs.Add(blog);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Edit(int id)
        {
            var blog = _dbContext.Find<Blog>(id);
            _dbContext.Entry(blog).Reference(b => b.User).Load();
            var blogEditDto = AutoMapperConfig.mapper.Map<Blog, BlogEditDTO>(blog);
            blogEditDto.UserName = blog.User.UserName;

            // check that only user who wrote the blog can edit that blog!!!
            var result = _authorizeService.AuthorizeAsync(User, blog, "UserBlogPolicyHandler").Result;
            if (result.Succeeded)
                return View(blogEditDto);
            else
                return new ChallengeResult();
        }

        [HttpPost]
        public IActionResult Edit(BlogEditDTO blogEditDTO)
        {
            var blog = _dbContext.Blogs.Where(b => b.Id == blogEditDTO.Id).Include(b => b.User).FirstOrDefault();
            if (_authorizeService.AuthorizeAsync(User,blog, "UserBlogPolicyHandler").Result.Succeeded)
            {
                //blog = AutoMapperConfig.mapper.Map<BlogEditDTO, Blog>(blogEditDTO);
                blog.Title = blogEditDTO.Title;
                blog.Description = blogEditDTO.Description;

                _dbContext.Update(blog);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
                return new ChallengeResult();
        }
    }
}
