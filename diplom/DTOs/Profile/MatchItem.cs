using diplom.ViewModels;

namespace diplom.DTOs.Profile;

public class MatchingItem : ViewModelBase
{
    private string _left;
    public string Left
    {
        get => _left;
        set => SetProperty(ref _left, value);
    }

    private string _right;
    public string Right
    {
        get => _right;
        set => SetProperty(ref _right, value);
    }
}