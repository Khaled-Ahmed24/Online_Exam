using Microsoft.AspNetCore.Mvc;
using Online_Exam.Models;
using System.Security.Cryptography;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
public class LoginController : Controller
{
    private readonly OnlineExammDbContext _Db;
    private readonly IHostingEnvironment _Host;
    public LoginController(OnlineExammDbContext Db , IHostingEnvironment Host)
    {
        _Db = Db;
        _Host = Host;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Signup()
    {
        return View(new Users());
    }

    [HttpPost] 
    public ActionResult Signup(Users user)
    {
        if(ModelState.IsValid)
        {
            var isEmailExist = _Db.Users.Any(x => x.U_Email == user.U_Email);
            if (isEmailExist)
            {
                ModelState.AddModelError("U_Email", "Email is already exists");
                return View();
            }
            if (user.U_Password == user.ConfirmPassword)
            {
                string Filename = null;
                if (user.File != null)
                {
                    string Saveroot = Path.Combine(_Host.WebRootPath, "Photos");
                    Filename = user.File.FileName;
                    string Fullpath = Path.Combine(Saveroot, Filename);
                    user.File.CopyTo(new FileStream(Fullpath, FileMode.Create));
                    user.PhotoPath = Filename;
                }
                user.U_Password = HashPassword(user.U_Password);
                user.ConfirmPassword = HashPassword(user.ConfirmPassword);
                _Db.Users.Add(user);
                _Db.SaveChanges();
                return RedirectToAction("Signin", "Login");
            }
            else
            {
                return View(user);
            }
        }
        else
        {
            return View(user);
        } 
    }

    public IActionResult Signin()
    {
        HttpContext.Session.Clear();
        return View();
    }
    [HttpPost]
    public IActionResult Signin(Signinuser user)
    {
        if(ModelState.IsValid)
        {
            var userdata = _Db.Users.FirstOrDefault(x => x.U_Email == user.U_Email);

            if (user != null && VerifyPassword(user.U_Password, userdata.U_Password))
            {
                HttpContext.Session.SetString("U_Email", user.U_Email);

                ViewBag.IsAuthenticated = true;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (userdata == null)
                {
                    ModelState.AddModelError("U_Email", "Email not found");
                }
                else
                {
                    ModelState.AddModelError("U_Password", "Wrong password");
                }
                ViewBag.IsAuthenticated = false;
                return View();
            }
        }
        else
        {
            return View();
        }
    }



    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        return HashPassword(inputPassword) == hashedPassword;
    }
}