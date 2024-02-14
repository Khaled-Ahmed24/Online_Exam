using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Exam.Models;

namespace Online_Exam.Controllers
{
    public class QuestionController : Controller
    {
        private readonly OnlineExammDbContext _Db;

        public QuestionController(OnlineExammDbContext Db)
        {
            _Db = Db;
        }

        public IActionResult Index()
        {
            var e_id = HttpContext.Session.GetInt32("Exam_id");

            if (e_id == null)
            {
                return RedirectToAction("Create", "Questions");
            }

            var questions = _Db.Questions
               .Where(e => e.Exam_id == e_id).
               ToList();

            return View(questions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _Db.Questions == null)
            {
                return NotFound();
            }

            var questions = await _Db.Questions
                .Include(q => q.Exam)
                .FirstOrDefaultAsync(m => m.Question_id == id);
            var choices = _Db.Choices.Where(e => e.Question_id == id).ToList();
            if (questions == null)
            {
                return NotFound();
            }
            var viewModel = new CreateQuestionViewModel
            {
                Question = questions,
                Choices = choices
            };
            return View(viewModel);
        }


        public IActionResult Create()
        {
            var Name = HttpContext.Session.GetString("U_Email");
            var examId = HttpContext.Session.GetInt32("Exam_id");
            // لو مكنتش مسجل دخول يروح يسجل
            if (string.IsNullOrEmpty(Name))
            {
                return RedirectToAction("Signin", "Login");
            }
            // لو مكنش عمل امتحان بس دخل على السؤال
            if (examId == null)
            {
                return RedirectToAction("Index", "home");
            }
            Questions q = new Questions();

            var viewModel = new CreateQuestionViewModel
            {
                Question = new Questions(),
                Choices = new List<Choices>()
            };

            return View(viewModel);
        }

        [HttpPost]

        public IActionResult Create(CreateQuestionViewModel viewModel)
        {
            int examId = HttpContext.Session.GetInt32("Exam_id").Value;
            viewModel.Question.Exam_id = examId;
            _Db.Add(viewModel.Question);
            _Db.SaveChanges();

            HttpContext.Session.SetInt32("Question_id", viewModel.Question.Question_id);

            foreach (var choice in viewModel.Choices)
            {
                choice.Question_id = viewModel.Question.Question_id;
                choice.Exam_id = viewModel.Question.Exam_id;

                _Db.Choices.Add(choice);
            }
            _Db.SaveChanges();
            return RedirectToAction("Index", "Question");

        }

        //public IActionResult Create(CreateQuestionViewModel viewModel)
        //{
        //    int examId = HttpContext.Session.GetInt32("Exam_id").Value;
        //    viewModel.Question.Exam_id = examId;
        //    _Db.Add(viewModel.Question);
        //    _Db.SaveChanges();

        //    HttpContext.Session.SetInt32("Question_id", viewModel.Question.Question_id);
        //    int i = 0;
        //    try
        //    {
        //        for (i=0;i<viewModel.Choices.Count();i++)
        //        {
        //            viewModel.Choices[i].Question_id = viewModel.Question.Question_id;
        //            viewModel.Choices[i].Exam_id = viewModel.Question.Exam_id;

        //            _Db.Choices.Add(viewModel.Choices[i]);
        //        }
        //        _Db.SaveChanges();
        //        return RedirectToAction("Index", "Question");
        //    }
        //    catch
        //    {
        //        TempData["ErrorMessage"] = "Error: Choices must not be empty.";

        //    }


        //}

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _Db.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Question_id == id);

            if (question == null)
            {
                return NotFound();
            }

            var viewModel = new CreateQuestionViewModel
            {
                Question = question,
                Choices = question.Choices.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateQuestionViewModel viewModel)
        {
            viewModel.Question.Exam_id = HttpContext.Session.GetInt32("Exam_id").Value;
            viewModel.Question.Question_id = HttpContext.Session.GetInt32("Question_id").Value;
            if (id != viewModel.Question.Question_id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    // Update existing question fields
                    _Db.Update(viewModel.Question);

                    // Update existing choices
                    foreach (var choice in viewModel.Choices)
                    {
                        choice.Exam_id = HttpContext.Session.GetInt32("Exam_id").Value;
                        choice.Question_id = viewModel.Question.Question_id;
                        _Db.Update(choice);
                    }

                    await _Db.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency conflict  
                    var entry = ex.Entries.Single();
                    var clientValues = (Choices)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. The question was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Choices)databaseEntry.ToObject();

                        // Resolve the conflict by choosing client values in this example
                        entry.OriginalValues.SetValues(databaseValues);
                        entry.CurrentValues.SetValues(clientValues);

                        // Save changes again and ignore any validation errors
                        await _Db.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    // Log the exception or handle it as needed
                    ModelState.AddModelError(string.Empty, "Unable to save changes. Please try again.");
                }
                return RedirectToAction(nameof(Index)); // Redirect to the action you want to navigate after a successful edit
            }

            // If the model state is not valid, return to the edit view with the current view model
            return View(viewModel);
        }




       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _Db.Questions == null)
            {
                return NotFound();
            }

            var questions = await _Db.Questions
                .Include(q => q.Exam)
                .FirstOrDefaultAsync(m => m.Question_id == id);

            var choices = _Db.Choices.Where(e => e.Question_id == id).ToList();
            var viewModel = new CreateQuestionViewModel
            {
                Question = questions,
                Choices = choices
            };
            if (questions == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_Db.Questions == null)
            {
                return Problem("Entity set 'AppDbContext.Questions'  is null.");
            }
            Questions questions = _Db.Questions.Include(q => q.Choices).FirstOrDefault(q => q.Question_id == id);
            if (questions != null)
            {
                _Db.Questions.Remove(questions);
                _Db.Choices.RemoveRange(questions.Choices);
            }

            await _Db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionsExists(int id)
        {
            return (_Db.Questions?.Any(e => e.Exam_id == id)).GetValueOrDefault();
        }

        public IActionResult Finish()
        {
            return RedirectToAction("Index", "Exam");
        }

    }
}
