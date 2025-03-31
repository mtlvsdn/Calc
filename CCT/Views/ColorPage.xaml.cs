using CalculatorApp.Services;

namespace CalculatorApp.Views;

public partial class ColorPage : ContentPage
{
    private readonly ColorSettingsService _colorSettingsService;

    public ColorPage()
    {
        InitializeComponent();
        _colorSettingsService = ColorSettingsService.Instance;
        BindingContext = _colorSettingsService;
    }

    private async void OnColorSelected(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            var selectedColor = button.BackgroundColor;
            
            // Update the color settings service
            _colorSettingsService.SelectedColor = selectedColor;
            
            // Force update the app's primary color
            Application.Current.Resources["Primary"] = selectedColor;
            
            // Wait a moment to ensure the color is applied
            await Task.Delay(100);
            
            // Navigate back to the previous page
            await Shell.Current.GoToAsync("..");
        }
    }
} 