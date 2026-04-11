namespace diplom.ViewModels.Tasks;

public class CodingTaskViewModel : TaskViewModel
{
    public string Code { get; set; }
    public override string? GetTextAnswer() => Code;
    
    public override void Submit() { }
}