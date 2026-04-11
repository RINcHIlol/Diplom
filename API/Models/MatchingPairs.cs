namespace API.Models;

public class MatchingPairs
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int LeftAnswerId { get; set; }
    public int RightAnswerId { get; set; }
}