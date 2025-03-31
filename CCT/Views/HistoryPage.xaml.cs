using System.Collections.ObjectModel;
using CalculatorApp.Services;

namespace CalculatorApp.Views
{
    public partial class HistoryPage : ContentPage
    {
        private readonly CalculationHistoryService _historyService;
        public ObservableCollection<string> History { get; }

        public HistoryPage()
        {
            InitializeComponent();
            _historyService = CalculationHistoryService.Instance;
            History = _historyService.CalculationHistory;
            BindingContext = this;
        }

        private void OnClearHistoryClicked(object sender, EventArgs e)
        {
            _historyService.ClearHistory();
        }
    }
} 