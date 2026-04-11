using Avalonia.Controls;
using Avalonia.Input;
using diplom.ViewModels.Tasks;

namespace diplom.Views.Tasks;

public partial class OrderingTaskView : UserControl
{
    public OrderingTaskView()
    {
        InitializeComponent();
    }

    private void AddClick(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is not OrderingTaskViewModel vm)
            return;

        if (sender is not Control c)
            return;

        if (c.DataContext is SelectableAnswer item)
        {
            vm.AddToOrder(item);
        }
    }

    private void RemoveClick(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is not OrderingTaskViewModel vm)
            return;

        if (sender is not Control c)
            return;

        if (c.DataContext is SelectableAnswer item)
        {
            vm.RemoveFromOrder(item);
        }
    }
}