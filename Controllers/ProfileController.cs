using Microsoft.AspNetCore.Mvc;
using Online_Exam.Models;
using System.Diagnostics;

namespace Online_Exam.Controllers
{
    public class ProfileController : Controller
    {
        private readonly OnlineExammDbContext _Db;

        public ProfileController(OnlineExammDbContext Db)
        {
            _Db = Db;
        }
        public IActionResult Index()
        {
            var userEmail = HttpContext.Session.GetString("U_Email");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Signin", "Login");
            }
            else
            {
                List<Users> ListUser = _Db.Users.ToList();
                PrintViewModel obViewModel = new PrintViewModel
                {
                    UsersViewModel = ListUser,
                };
                ViewBag.UserEmail = userEmail;
                return View(obViewModel);
            }

        }
    }
}
