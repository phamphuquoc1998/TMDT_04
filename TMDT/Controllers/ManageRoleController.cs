using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class ManageRoleController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: ManageRole
        public ActionResult Index()
        {
            var role = (from r in context.Roles where r.Name.Contains("User") select r).FirstOrDefault();
            var users = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            var userVM = users.Select(user => new UserViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                RoleName = "User"
            }).ToList();


            var role2 = (from r in context.Roles where r.Name.Contains("ADMIN") select r).FirstOrDefault();
            var admins = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role2.Id)).ToList();

            var adminVM = admins.Select(user => new UserViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                RoleName = "ADMIN"
            }).ToList();


            var model = new GroupedUserViewModel { Users = userVM, Admins = adminVM };
            return View(model);

        }
    }
}