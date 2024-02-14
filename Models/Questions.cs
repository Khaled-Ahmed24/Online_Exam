using Online_Exam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Online_Exam.Models
{
    public class Questions
    {
        [Key]
        [Required]
        public int Exam_id { get; set; }

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Question_id { get; set; }

        [Required]
        [DisplayName("Question Title")]
        public string Question_title { get; set; }

        [Required]
        [DisplayName("Question Type")]
        public string Question_type { get; set; }

        [Required]
        public decimal Points { get; set; }

        public virtual ICollection<Choices> Choices { get; set; } = new List<Choices>();

        public virtual ICollection<Answers> Answers { get; set; } = new List<Answers>();

        [ForeignKey("Exam_id")]
        public virtual Exam Exam { get; set; }

    }
}