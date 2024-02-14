using Microsoft.AspNetCore.Mvc;
using Online_Exam.Models;

namespace Online_Exam.Controllers
{
    public class PrintController : Controller
    {
        private readonly OnlineExammDbContext _Db;

        public PrintController(OnlineExammDbContext Db)
        {
            _Db = Db;
        }

        public IActionResult Index()
        {
            var x = HttpContext.Session.GetString("U_Email");
            if (string.IsNullOrEmpty(x))
            {
                return RedirectToAction("Signin", "Login");
            }
            var Name = HttpContext.Session.GetInt32("ExamCode").Value;
            bool check = _Db.Answers.Any(q => q.U_Email == x && q.Exam_id == Name);

            if (string.IsNullOrEmpty(x))
            {
                return RedirectToAction("Signin", "Login");
            }
            if (check == true)
            {
                TempData["AlertMessage"] = "You have already entered this exam before.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.mess = Name;
                List<Exam> ListExam = _Db.Exams.ToList();
                List<Questions> ListQuestions = _Db.Questions.ToList();
                List<Choices> ListChoices = _Db.Choices.ToList();
                PrintViewModel obViewModel = new PrintViewModel
                {
                    ExamViewModel = ListExam,
                    QuestionsViewModel = ListQuestions,
                    ChoicesViewModel = ListChoices
                };
                bool isFound = false;
                foreach(var item in ListExam)
                {
                    if(item.Exam_id == Name)
                    {
                        isFound = true;
                    }
                }
                if(isFound == false)
                {
                    TempData["NotFound"] = "Exam Not Found";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    isFound = false;
                }
                return View(obViewModel);
            }
        }
    
    }
}
