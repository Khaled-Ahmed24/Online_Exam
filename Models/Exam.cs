using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Exam.Models
{
    public class checkCurrentdate : ValidationAttribute
    {
        public override bool IsValid(Object value)
        {
            if (value is DateTime date)
            {
                return date >= DateTime.Now;
            }
            return false;
        }
    }

    public partial class Exam
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Exam Code")]
        public int Exam_id { get; set; }

        [Required]
        public string Adminstration_Email { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [DisplayName("Exam Title")]
        public string Exam_title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [checkCurrentdate(ErrorMessage = ("Start time must be greater than the current time"))]
        [DisplayName("Start Time")]
        public DateTime Start_time { get; set; }

        [DataType(DataType.Time)]
        private TimeSpan? _duration;
        public TimeSpan? Duration
        {
            get { return _duration ?? TimeSpan.Parse("00:00:00"); }
            set { _duration = value; }
        }

        [ForeignKey("Adminstration_Email")]
        [DisplayName("Adminstration Email")]
        public virtual Users AdminstrationEmailNavigation { get; set; } = null!;

        public virtual ICollection<Questions> Questions { get; set; } = new List<Questions>();
    }
}