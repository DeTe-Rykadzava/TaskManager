using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TaskManager.Views;

public partial class SignInUpView : UserControl
{
    public SignInUpView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}