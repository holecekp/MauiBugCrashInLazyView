using System.ComponentModel;
using System.Windows.Input;

namespace MauiBugCrashInLazyView;

public class TestViewModel : INotifyPropertyChanged
{
    public ICommand LoadViewCommand { get; protected set; }
    public TestViewModel()
    {
        LoadViewCommand = new Command(LoadView);
    }

    private bool showView = false;

    public bool ShowView
    {
        get => showView;
        set
        {
            if (showView != value)
            {
                showView = value;
                OnPropertyChanged(nameof(ShowView));
            }
        }
    }

    private void LoadView()
    {
        ShowView = true;
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
