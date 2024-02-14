using Microsoft.AspNetCore.Mvc;
using Online_Exam.Models;

namespace Online_Exam.Controllers
{
    public class ShowSolvedExamsController : Controller
    {
        private readonly OnlineExammDbContext db;

        public ShowSolvedExamsController(OnlineExammDbContext context)
        {
            db = context;
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
                List<Users> ListUser = db.Users.ToList();
                List<Exam> ListExam = db.Exams.ToList();
                List<Answers> ListAnswer = db.Answers.ToList();
                List<Questions> ListQuestion = db.Questions.ToList();
                PrintViewModel obViewModel = new PrintViewModel
                {
                    UsersViewModel = ListUser,
                    ExamViewModel = ListExam,
                    AnswersViewModel = ListAnswer,
                    QuestionsViewModel = ListQuestion,
                };
                ViewBag.UserEmail = userEmail;
                return View(obViewModel);

            }

        }
    }
}
