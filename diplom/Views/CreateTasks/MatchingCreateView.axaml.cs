using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using diplom.ViewModels.CreateTasks;

namespace diplom.Views.CreateTasks;

public partial class MatchingCreateView : UserControl
{
    public MatchingCreateView()
    {
        InitializeComponent();
    }
    
    private MatchingCreateViewModel VM =>
        (MatchingCreateViewModel)DataContext!;

    private void AddLeftClicked(object? sender, RoutedEventArgs e)
        => VM.AddLeft();

    private void AddRightClicked(object? sender, RoutedEventArgs e)
        => VM.AddRight();

    private void RemoveLeftClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button b && b.DataContext is AnswerOption item)
            VM.RemoveLeft(item);
    }

    private void RemoveRightClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button b && b.DataContext is AnswerOption item)
            VM.RemoveRight(item);
    }
}