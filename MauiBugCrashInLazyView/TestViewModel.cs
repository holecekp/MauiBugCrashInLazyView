using System.ComponentModel;
using System.Windows.Input;

namespace MorseCode.ViewModels
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public ICommand LoadViewCommand { get; protected set; }
        public TestViewModel()
        {
            LoadViewCommand = new Command(LoadView);
        }

        private bool showView = false;

        public event PropertyChangedEventHandler PropertyChanged;

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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadView()
        {
            ShowView = true;
        }
    }
}