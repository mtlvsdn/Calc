using System.Collections.ObjectModel;

namespace CalculatorApp.Views;

public partial class SettingsPage : ContentPage
{
    public ObservableCollection<string> SettingsOptions { get; } = new ObservableCollection<string>
    {
        "Color"
    };

    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnSettingSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string selectedOption)
        {
            if (selectedOption == "Color")
            {
                await Shell.Current.GoToAsync(nameof(ColorPage));
            }
        }
    }
} 