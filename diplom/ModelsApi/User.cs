using System;

namespace diplom.ModelsApi;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password_hash { get; set; }
    public DateTime Created_at { get; set; }
    public int Xp { get; set; }
    public int Role_id { get; set; }
}