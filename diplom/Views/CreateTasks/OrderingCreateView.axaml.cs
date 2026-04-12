using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using diplom.ViewModels.CreateTasks;

namespace diplom.Views.CreateTasks;

public partial class OrderingCreateView : UserControl
{
    public OrderingCreateView()
    {
        InitializeComponent();
    }
    
    private void AddClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is OrderingCreateViewModel vm)
            vm.Add();
    }

    private void RemoveClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is OrderingCreateViewModel vm)
        {
            if ((sender as Button)?.DataContext is AnswerOption item)
                vm.RemoveItem(item);
        }
    }
}