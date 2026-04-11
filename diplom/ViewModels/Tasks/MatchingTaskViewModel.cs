using System.Collections.Generic;
using System.Linq;
using diplom.DTOs.Profile;

namespace diplom.ViewModels.Tasks;

public class MatchingPair
{
    public SelectableAnswer Left { get; set; }
    public SelectableAnswer? SelectedRight { get; set; }

    public List<SelectableAnswer> RightItems { get; set; } = new();
}

public class MatchingTaskViewModel : TaskViewModel
{
    public List<SelectableAnswer> RightItems { get; set; } = new();

    public List<MatchingPair> Pairs { get; set; } = new();
    public override List<MatchDto> GetMatches()
    {
        return Pairs
            .Where(p => p.SelectedRight != null)
            .Select(p => new MatchDto
            {
                LeftId = p.Left.Id,
                RightId = p.SelectedRight!.Id
            })
            .ToList();
    }

    public override void Submit() { }
}