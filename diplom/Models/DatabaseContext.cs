using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace diplom.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Lvl> Lvls { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskAnswer> TaskAnswers { get; set; }

    public virtual DbSet<TaskType> TaskTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProgress> UserProgresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=5432");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("courses_pkey");

            entity.ToTable("courses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lessons_pkey");

            entity.ToTable("lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("character varying")
                .HasColumnName("content");
            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");

            entity.HasOne(d => d.Module).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("lessons_module_id_fkey");
        });

        modelBuilder.Entity<Lvl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lvls_pkey");

            entity.ToTable("lvls");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Xp).HasColumnName("xp");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("modules_pkey");

            entity.ToTable("modules");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");

            entity.HasOne(d => d.Course).WithMany(p => p.Modules)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("modules_course_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_pkey");

            entity.ToTable("tasks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("character varying")
                .HasColumnName("content");
            entity.Property(e => e.CorrectAnswer)
                .HasColumnType("character varying")
                .HasColumnName("correct_answer");
            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.Question)
                .HasColumnType("character varying")
                .HasColumnName("question");
            entity.Property(e => e.TaskTypeId).HasColumnName("task_type_id");

            entity.HasOne(d => d.Module).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("tasks_module_id_fkey");

            entity.HasOne(d => d.TaskType).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskTypeId)
                .HasConstraintName("tasks_task_type_id_fkey");
        });

        modelBuilder.Entity<TaskAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_answers_pkey");

            entity.ToTable("task_answers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnswerText)
                .HasColumnType("character varying")
                .HasColumnName("answer_text");
            entity.Property(e => e.IsCorrect)
                .HasDefaultValue(false)
                .HasColumnName("is_correct");
            entity.Property(e => e.MatchKey).HasColumnName("match_key");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskAnswers)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_answers_task_id_fkey");
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_type_pkey");

            entity.ToTable("task_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash)
                .HasColumnType("character varying")
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Xp)
                .HasDefaultValue(0)
                .HasColumnName("xp");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<UserProgress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_progress_pkey");

            entity.ToTable("user_progress");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completed_at");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Lesson).WithMany(p => p.UserProgresses)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("user_progress_lesson_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserProgresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_progress_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
