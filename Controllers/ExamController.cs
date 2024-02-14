using Microsoft.AspNetCore.Mvc;
using Online_Exam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
public class ExamController : Controller
{
    private readonly OnlineExammDbContext _Db;

    public ExamController(OnlineExammDbContext Db)
    {
        _Db = Db;
    }
    
    public IActionResult Create()
    {
        var adminEmail = HttpContext.Session.GetString("U_Email");
        if (string.IsNullOrEmpty(adminEmail))
        {
            return RedirectToAction("Signin", "Login");
        }
        return View(new Exam());
    }

    [HttpPost]
    public IActionResult Create(Exam exam)
    {
        string adminEmail = HttpContext.Session.GetString("U_Email");
        ModelState.Remove("Adminstration_Email");
        ModelState.Remove("AdminstrationEmailNavigation");
        if(ModelState.IsValid)
        {
            exam.Adminstration_Email = adminEmail;
            _Db.Exams.Add(exam);
            _Db.SaveChanges();
            HttpContext.Session.SetInt32("Exam_id", exam.Exam_id);
            return RedirectToAction("Create", "Question");
        }
        else
        {
            return View(exam);
        }
    }

    public IActionResult Index()
    {
        var adminEmail = HttpContext.Session.GetString("U_Email");
        if (string.IsNullOrEmpty(adminEmail))
        {
            return RedirectToAction("Signin", "Login");
        }
        var exams = _Db.Exams
            .Where(e => e.Adminstration_Email == adminEmail)
            .ToList();

        return View(exams);
    }


    // GET: Examssss/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _Db.Exams == null)
        {
            return NotFound();
        }

        var exam = await _Db.Exams
            .Include(e => e.AdminstrationEmailNavigation)
            .FirstOrDefaultAsync(m => m.Exam_id == id);
        if (exam == null)
        {
            return NotFound();
        }

        return View(exam);
    }


    // GET: Examssss/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _Db.Exams == null)
        {
            return NotFound();
        }

        var exam = await _Db.Exams.FindAsync(id);
        if (exam == null)
        {
            return NotFound();
        }
        ViewData["Adminstration_Email"] = new SelectList(_Db.Users, "U_Email", "U_Email", exam.Adminstration_Email);
        return View(exam);
    }

    // POST: Examssss/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Exam_id,Adminstration_Email,Exam_title,Description,Start_time,Duration,IS_shuffle_Q,IS_shuffle_A")] Exam exam)
    {
        if (id != exam.Exam_id)
        {
            return NotFound();
        }
        string adminEmail = HttpContext.Session.GetString("U_Email");
        exam.Adminstration_Email = adminEmail;
        ///// not valid 26/12
        if (!ModelState.IsValid)
        {
            try
            {
                _Db.Update(exam);
                await _Db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamExists(exam.Exam_id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["Adminstration_Email"] = new SelectList(_Db.Users, "U_Email", "U_Email", exam.Adminstration_Email);
        return View(exam);
    }

    // GET: Examssss/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _Db.Exams == null)
        {
            return NotFound();
        }

        var exam = await _Db.Exams
            .Include(e => e.AdminstrationEmailNavigation)
            .FirstOrDefaultAsync(m => m.Exam_id == id);
        if (exam == null)
        {
            return NotFound();
        }

        return View(exam);
    }

    // POST: Examssss/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_Db.Exams == null)
        {
            return Problem("Entity set 'AppDbContext.Exams'  is null.");
        }
        var exam = await _Db.Exams.FindAsync(id);
        if (exam != null)
        {
            _Db.Exams.Remove(exam);
        }

        await _Db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ExamExists(int id)
    {
        return (_Db.Exams?.Any(e => e.Exam_id == id)).GetValueOrDefault();
    }
    [HttpPost]
    public IActionResult SaveExamCode(int examCode)
    {
        HttpContext.Session.SetInt32("ExamCode", examCode);
        return NoContent();
    }

    public IActionResult Results(int id)
    {
        var userEmail = HttpContext.Session.GetString("U_Email");
        if(string.IsNullOrEmpty(userEmail))
        {
            RedirectToAction("Signin", "Login");
        }
        List<ResultsModel> results = new List<ResultsModel>();
        List<Answers> answers = _Db.Answers.ToList();
        List<Users> users = _Db.Users.ToList();
        int i = 0;
        decimal Total = 0;
        foreach (var user in users)
        {
            bool ch = false;
            foreach (var answer in answers)
            {
                if(user.U_Email ==  answer.U_Email && answer.Exam_id == id)
                {
                    Total += answer.Points_Earned;
                    ch = true;
                }
            }
            if(ch)
               results.Add(new ResultsModel { FullName = user.U_FullName, Email = user.U_Email, Totalgrade = Total});
        }

        return View(results);
    }
    public IActionResult Finish()
    {
        HttpContext.Session.Remove("Exam_id");
        return RedirectToAction("Index", "Exam");
    }
}
