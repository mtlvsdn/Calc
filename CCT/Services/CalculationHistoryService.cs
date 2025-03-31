using System.Collections.ObjectModel;

namespace CalculatorApp.Services;

public class CalculationHistoryService
{
    private static CalculationHistoryService _instance;
    private readonly ObservableCollection<string> _calculationHistory;

    public static CalculationHistoryService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CalculationHistoryService();
            }
            return _instance;
        }
    }

    public ObservableCollection<string> CalculationHistory => _calculationHistory;

    private CalculationHistoryService()
    {
        _calculationHistory = new ObservableCollection<string>();
    }

    public void AddCalculation(string calculation)
    {
        _calculationHistory.Insert(0, calculation);
    }

    public void ClearHistory()
    {
        _calculationHistory.Clear();
    }
} 