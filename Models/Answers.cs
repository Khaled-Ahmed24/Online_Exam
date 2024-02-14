using Online_Exam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Exam.Models
{
    public class Answers
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        public int Exam_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required]
        public int Question_id { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        [DisplayName("Email Address")]
        public string U_Email { get; set; }
        [DisplayName("Answer Text")]
        public string? Answer_text { get; set; }
        [DisplayName("Your points")]
        public decimal Points_Earned { get; set; }

        [ForeignKey("Exam_id, Question_id")]
        public virtual Questions Questions { get; set; } = null!;

        [ForeignKey("U_Email")]
        public virtual Users UEmailNavigation { get; set; } = null!;


    }
}
