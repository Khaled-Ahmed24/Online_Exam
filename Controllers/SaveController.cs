using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Exam.Models;
using System.Collections.Generic;
using System.Linq;
namespace Online_Exam.Controllers
{
    public class SaveController : Controller
    {
        private readonly OnlineExammDbContext db;

        public SaveController(OnlineExammDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveAnswers(IFormCollection form)
        {
            var userEmail = HttpContext.Session.GetString("U_Email");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Signin", "Login");
            }
            var examId = HttpContext.Session.GetInt32("ExamCode").Value;

            var user = db.Users.FirstOrDefault(u => u.U_Email == userEmail);
            var exam = db.Exams.FirstOrDefault(e => e.Exam_id == examId);

            if (user == null || exam == null)
            {
                return View("Error");
            }

            ViewBag.UserName = user.U_FullName;
            ViewBag.ExamCode = examId;
            ViewBag.ExamTitle = exam.Exam_title;

            var questions = db.Questions.Where(q => q.Exam_id == examId).ToList();
            var choices = db.Choices.ToList();

            var answers = new List<Answers>();
            var totalPoints = 0m;

            foreach (var question in questions)
            {
                var answer = GetAnswerForQuestion(form, question);
                answers.Add(answer);

                if (answer.Points_Earned > 0)
                {
                    totalPoints += answer.Points_Earned;
                }
            }

            db.Answers.AddRange(answers);
            db.SaveChanges();

            ViewBag.Grade = totalPoints;
            ViewBag.Name = userEmail;
            ViewBag.SumDegree = questions.Sum(q => q.Points);

            return View("Degree");
        }

        private Answers GetAnswerForQuestion(IFormCollection form, Questions question)
        {
            var answer = new Answers
            {
                Exam_id = question.Exam_id,
                Question_id = question.Question_id,
                U_Email = HttpContext.Session.GetString("U_Email"),
                Answer_text = GetAnswerText(form, question)
            };

            var correctChoices = db.Choices
                .Where(ch => ch.Exam_id == question.Exam_id && ch.Question_id == question.Question_id && ch.Is_correct)
                .Select(ch => ch.Choice_text)
                .OrderBy(ch => ch)
                .ToList();

            if (question.Question_type == "CheckBox")
            {
                var selectedChoices = answer.Answer_text?.Split(",").OrderBy(ch => ch).ToList();

                if (selectedChoices != null && correctChoices.SequenceEqual(selectedChoices))
                {
                    answer.Points_Earned = question.Points;
                }
            }
            else
            {
                if (correctChoices.Count == 1 && correctChoices.First() == answer.Answer_text)
                {
                    answer.Points_Earned = question.Points;
                }
            }

            return answer;
        }

        private string GetAnswerText(IFormCollection form, Questions question)
        {
            if (question.Question_type == "CheckBox")
            {
                var checkBoxName = "Q" + question.Question_id;
                string selected = "";
                foreach (var key in form.Keys)
                {
                    if (key.StartsWith(checkBoxName))
                    {
                        selected += form[key];
                    }
                }
                var Fianl = "";
                for (int i = 0; i < selected.Count() - 2; i++)
                {
                    if (selected[i] == ',' && selected[i + 1] == 'f' && selected[i + 2] == 'a')
                    {
                        break;
                    }
                    else Fianl += selected[i];
                }
                return Fianl;
            }
            else
            {
                var userAnswer = "Q" + question.Question_id;
                return form[userAnswer];
            }
        }
    }
}
