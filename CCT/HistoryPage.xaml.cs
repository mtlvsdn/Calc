using System.Collections.ObjectModel;

namespace CalculatorApp;

public partial class HistoryPage : ContentPage
{
    public ObservableCollection<string> History { get; }

    public HistoryPage(ObservableCollection<string> history)
    {
        InitializeComponent();
        History = history;
        BindingContext = this;
    }
} 