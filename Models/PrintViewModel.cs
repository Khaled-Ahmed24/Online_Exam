using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Exam.Models
{
    public class PrintViewModel
    {
        public IEnumerable<Exam> ExamViewModel { get; set; }
        public IEnumerable<Questions> QuestionsViewModel { get; set; }
        public IEnumerable<Choices> ChoicesViewModel { get; set; }
        public IEnumerable<Answers> AnswersViewModel { get; set; }
        public IEnumerable<Users> UsersViewModel { get; set; }

    }
}
