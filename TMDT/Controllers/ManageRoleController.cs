using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class ManageRoleController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        // GET: ManageRole
        
        public ActionResult Index()
        {
                var usersWithRoles = (from user in context.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Username = user.UserName,
                                          Email = user.Email,
                                          RoleNames = (from userRole in user.Roles
                                                       join role in context.Roles on userRole.RoleId
                                                       equals role.Id
                                                       select role.Name).ToList()
                                      }).ToList().Select(p => new UserInRoleViewModel()

                                      {
                                          UserId = p.UserId,
                                          Username = p.Username,
                                          Email = p.Email,
                                          Role = string.Join(",", p.RoleNames)
                                      });
           ViewBag.Roles = context.Roles.AsEnumerable();
         
            return View(usersWithRoles);
            

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [HttpPost]
        public ActionResult Edit(FormCollection formCollection)
        {

            string roleName = formCollection["roleName"];
            string userId = formCollection["userId"];

            string currentRole = UserManager.GetRoles(userId).First();

            var roles =  context.Roles.AsEnumerable();
            if (currentRole != null)
            {
                UserManager.RemoveFromRole(userId, currentRole);
            }        
            // userManager.RemoveFromRole(userId, roleName);
             UserManager.AddToRole(userId, roleName);
            return RedirectToAction(nameof(Index));
        }
    }
}