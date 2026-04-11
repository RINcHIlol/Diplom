namespace diplom.ViewModels.Tasks;

public class TextQuestionTaskViewModel : TaskViewModel
{
    private string? _answer;

    public string? Answer
    {
        get => _answer;
        set => SetProperty(ref _answer, value);
    }
    
    public override string? GetTextAnswer() => Answer;

    public override void Submit() { }
}