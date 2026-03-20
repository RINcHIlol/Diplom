using API.Models;
using Microsoft.EntityFrameworkCore;
using Task = API.Models.Task;
using Npgsql.EntityFrameworkCore.PostgreSQL;
namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskAnswer> TaskAnswers { get; set; }
    public DbSet<UserProgress> UserProgresses { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }
    public DbSet<Lvl> Lvls { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     foreach (var entity in modelBuilder.Model.GetEntityTypes())
    //     {
    //         entity.SetTableName(entity.GetTableName().ToLower());
    //     
    //         foreach (var property in entity.GetProperties())
    //         {
    //             property.SetColumnName(property.Name.ToLower());
    //         }
    //     
    //         foreach (var key in entity.GetKeys())
    //         {
    //             key.SetName(key.GetName()?.ToLower());
    //         }
    //     
    //         foreach (var fk in entity.GetForeignKeys())
    //         {
    //             fk.SetConstraintName(fk.GetConstraintName()?.ToLower());
    //         }
    //     
    //         foreach (var index in entity.GetIndexes())
    //         {
    //             index.SetDatabaseName(index.GetDatabaseName()?.ToLower());
    //         }
    //     }
    // }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProgress>()
            .ToTable("user_progress");
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()));
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
            }
        }

        base.OnModelCreating(modelBuilder);
    }
    
    private string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var builder = new System.Text.StringBuilder();
        builder.Append(char.ToLowerInvariant(name[0]));

        for (int i = 1; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]))
            {
                builder.Append('_');
                builder.Append(char.ToLowerInvariant(name[i]));
            }
            else
            {
                builder.Append(name[i]);
            }
        }

        return builder.ToString();
    }
}