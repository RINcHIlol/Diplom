namespace API.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[]? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatorUserId { get; set; }
    
    public List<Module> Modules { get; set; }
}