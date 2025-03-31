using CalculatorApp.Views;

namespace CalculatorApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Views.ScientificCalculatorPage), typeof(Views.ScientificCalculatorPage));
        Routing.RegisterRoute(nameof(Views.SettingsPage), typeof(Views.SettingsPage));
        Routing.RegisterRoute(nameof(Views.ColorPage), typeof(Views.ColorPage));
        Routing.RegisterRoute(nameof(Views.HistoryPage), typeof(Views.HistoryPage));
    }
} 