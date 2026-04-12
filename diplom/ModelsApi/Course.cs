using System;

namespace diplom.ModelsApi;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created_at { get; set; }
    public byte[]? Image { get; set; }
    public int CreatorUserId { get; set; }
}