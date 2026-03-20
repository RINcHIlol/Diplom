using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

// public class User
// {
//     public int Id { get; set; }
//     public string Login { get; set; }
//     public string Email { get; set; }
//     public string Password_hash { get; set; }
//     public DateTime Created_at { get; set; }
//     public int Xp { get; set; }
//     public int Role_id { get; set; }
// }

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Xp { get; set; }

    // [Column("role_id")] 
    public int RoleId { get; set; }
    public Role Role { get; set; }

    public List<UserProgress> Progresses { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class RegRequest
{
    public string Login { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}