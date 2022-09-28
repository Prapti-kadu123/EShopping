using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShopping.Data;
using System.Threading.Tasks;
using System.Linq;
using EShopping.Web.Data;

namespace EShopping.Areas.eshopping.Controllers
{
    [Area("eshopping")]
    public class ListUsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ListUsersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            //var users1 = await _dbContext.Users.Select(u => new
            //{
            //    u.UserName,
            //    u.Email,
            //    u.PhoneNumber

            //}).ToListAsync();

            //var users = (from user in _dbContext.Users
            //            select new
            //            {
            //                user.UserName,
            //                user.Email,
            //                user.PhoneNumber
            //            }).ToListAsync();
            //ViewBag.Users = User;

            var users = await _dbContext.Users.ToListAsync();
            return View(users);
        }
    }
}