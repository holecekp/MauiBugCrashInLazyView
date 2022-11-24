namespace MauiBugCrashInLazyView;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new TestViewModel();
    }
}