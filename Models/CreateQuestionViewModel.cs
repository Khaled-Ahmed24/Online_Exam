using Online_Exam.Models;

namespace Online_Exam.Models
{
    public class CreateQuestionViewModel
    {
        public Questions Question { get; set; }
        public List<Choices> Choices { get; set; }
    }
}
