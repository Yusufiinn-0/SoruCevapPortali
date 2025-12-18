using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models.Entity;

namespace SoruCevapPortali.Models.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Question - ApplicationUser relationship
            modelBuilder.Entity<Question>()
                .HasOne(s => s.User)
                .WithMany(k => k.Questions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Question - Category relationship
            modelBuilder.Entity<Question>()
                .HasOne(s => s.Category)
                .WithMany(k => k.Questions)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Answer - Question relationship
            modelBuilder.Entity<Answer>()
                .HasOne(c => c.Question)
                .WithMany(s => s.Answers)
                .HasForeignKey(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Answer - ApplicationUser relationship
            modelBuilder.Entity<Answer>()
                .HasOne(c => c.User)
                .WithMany(k => k.Answers)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Default categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Genel", Description = "Genel konular", Icon = "fa-globe", OrderNumber = 1, IsActive = true },
                new Category { CategoryId = 2, CategoryName = "Teknoloji", Description = "Teknoloji ile ilgili sorular", Icon = "fa-laptop", OrderNumber = 2, IsActive = true },
                new Category { CategoryId = 3, CategoryName = "Yazılım", Description = "Yazılım geliştirme soruları", Icon = "fa-code", OrderNumber = 3, IsActive = true },
                new Category { CategoryId = 4, CategoryName = "Eğitim", Description = "Eğitim ile ilgili sorular", Icon = "fa-graduation-cap", OrderNumber = 4, IsActive = true },
                new Category { CategoryId = 5, CategoryName = "Sağlık", Description = "Sağlık ile ilgili sorular", Icon = "fa-heartbeat", OrderNumber = 5, IsActive = true }
            );
        }
    }
}
