// Services/AdministrationService.cs
using System.Collections.Generic;
using System.Linq;
using Online_Exam.Models;

namespace Online_Exam.Services
{
    public class AdministrationService
    {
        private readonly OnlineExammDbContext _context;

        public AdministrationService(OnlineExammDbContext context)
        {
            _context = context;
        }

        public List<string> GetAdministrationEmails()
        {
            // Assuming there is a DbSet<Users> in your DbContext
            List<string> administrationEmails = _context.Users
                                                        .Select(u => u.U_Email)
                                                        .ToList();
            return administrationEmails;
        }
    }
}
