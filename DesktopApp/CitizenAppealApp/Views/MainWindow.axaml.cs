using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CitizenAppealApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }
}