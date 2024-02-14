using Microsoft.EntityFrameworkCore;
using Online_Exam.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Exam.Models
{
    public class OnlineExammDbContext : DbContext
    {
        public OnlineExammDbContext(DbContextOptions<OnlineExammDbContext> options) : base(options)
        {
        }
        public OnlineExammDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            //modelBuilder.Entity<Exam>().HasKey(e => e.Exam_id);
            /*
              modelBuilder.Entity<Exam>()
             .HasOne(e => e.AdminstrationEmailNavigation)
             .WithMany(e => e.Exams).HasForeignKey(e => e.Adminstration_Email);
              */

            modelBuilder.Entity<Questions>().HasKey(e => new { e.Exam_id, e.Question_id });
            /*
          modelBuilder.Entity<Questions>()
          .HasOne(e => e.Exam)
          .WithMany(e => e.Questions).HasForeignKey(e => e.Exam_id);
            */

            modelBuilder.Entity<Choices>().HasKey(e => new { e.Exam_id, e.Question_id, e.Choice_text });
            /*
            modelBuilder.Entity<Choices>()
            .HasOne(e => e.Questions)
            .WithMany(e => e.Choices).HasForeignKey(e => new {e.Exam_id, e.Question_id});
            */

            // في الفيديو عمل دي بس many to many
            modelBuilder.Entity<Answers>()
               .HasKey(e => new { e.U_Email, e.Exam_id, e.Question_id, e.Answer_text });

            modelBuilder.Entity<Answers>()
            .HasOne(e => e.Questions)
            .WithMany(e => e.Answers).HasForeignKey(e => new { e.Exam_id, e.Question_id }).OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<Answers>()
           .HasOne(e => e.UEmailNavigation)
           .WithMany(e => e.Answers).HasForeignKey(e => new { e.U_Email }).OnDelete(DeleteBehavior.NoAction); ;
           


            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Users> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Choices> Choices { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<Questions> Questions { get; set; }
    }
}