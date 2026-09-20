using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Models;

namespace OnlineExamApi.Data;

public partial class ExamDbContext : DbContext  //ExamDbContext is entire db in c#
{
    public ExamDbContext(DbContextOptions<ExamDbContext> options)//reads db configuration from program.cs for sql
        : base(options)
    {
    }

    public virtual DbSet<Option> Options { get; set; } //DbSet is like db table in c# //so we can write _dbContext.Options

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestAnswer> TestAnswers { get; set; }

    public virtual DbSet<TestAttempt> TestAttempts { get; set; }

    public virtual DbSet<User> Users { get; set; }


    //needed to know keys and relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Options__92C7A1FF43EDFD91");

            entity.HasIndex(e => new { e.QuestionId, e.OptionOrder }, "UQ_Options_Question_Order").IsUnique();

            entity.Property(e => e.OptionText).HasMaxLength(500);

            entity.HasOne(d => d.Question).WithMany(p => p.Options)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Options_Questions");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FACF5118CAD");

            entity.HasIndex(e => new { e.TestId, e.QuestionOrder }, "UQ_Questions_Test_Order").IsUnique();

            entity.HasOne(d => d.Test).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK_Questions_Tests");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__Tests__8CC33160B0B17117");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PassingPercentage).HasDefaultValue(50);
            entity.Property(e => e.Subject).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Tests)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tests_Users");
        });

        modelBuilder.Entity<TestAnswer>(entity =>
        {
            entity.HasKey(e => e.TestAnswerId).HasName("PK__TestAnsw__0B8662E9F9F84C59");

            entity.HasIndex(e => new { e.TestAttemptId, e.QuestionId }, "UQ_TestAnswers_Attempt_Question").IsUnique();

            entity.HasOne(d => d.Question).WithMany(p => p.TestAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestAnswers_Questions");

            entity.HasOne(d => d.SelectedOption).WithMany(p => p.TestAnswers)
                .HasForeignKey(d => d.SelectedOptionId)
                .HasConstraintName("FK_TestAnswers_Options");

            entity.HasOne(d => d.TestAttempt).WithMany(p => p.TestAnswers)
                .HasForeignKey(d => d.TestAttemptId)
                .HasConstraintName("FK_TestAnswers_Attempts");
        });

        modelBuilder.Entity<TestAttempt>(entity =>
        {
            entity.HasKey(e => e.TestAttemptId).HasName("PK__TestAtte__FCCE33E742296B72");

            entity.HasOne(d => d.Test).WithMany(p => p.TestAttempts)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestAttempts_Tests");

            entity.HasOne(d => d.User).WithMany(p => p.TestAttempts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestAttempts_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB82C45C0");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F2845609ED3956").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

//bridege btw c# app and database
//we create it so ef core knows which table exist , what relationship exist..
//..primary keys , foreign keys, constraints, db configurations