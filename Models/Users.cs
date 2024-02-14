using Online_Exam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Online_Exam.Models
{
    public class Signinuser
    {
        [Key]
        [Required]
        [DisplayName("Email Address")]
        public string U_Email { get; set; }
        [Required]
        [DisplayName("Password")]
        public string U_Password { get; set; }
    }
    public class Users
    {
        [Key]
        [Required]
        [EmailAddress]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please Enter Valid E-mail")]
        [DisplayName("Email Address")]
        public string U_Email { get; set; }

        [Required]
        [DisplayName("Full Name")]
        public string U_FullName { get; set; }

        [Required]
        [StringLength(11)]
        [RegularExpression(@"01[0-2]\d{8}|015\d{8}", ErrorMessage = "Invalid phone number")]
        [DisplayName("Phone Number")]
        public string U_PhoneNumber { get; set; }

        [DisplayName("Photo")]
        public string? PhotoPath { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8 , ErrorMessage = "Password must consist of at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain a combination of upper case characters, lower case characters and digits.")]
        [DisplayName("Password")]
        public string U_Password { get; set; }
        [Compare("U_Password", ErrorMessage = "The passwords do not match.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Answers> Answers { get; set; } = new List<Answers>();

        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

        [NotMapped]
        public IFormFile? File { get; set; }

    }
}
