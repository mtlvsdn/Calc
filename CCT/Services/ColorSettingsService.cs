using System.Text.Json;

namespace CalculatorApp.Services;

public class ColorSettingsService
{
    private static ColorSettingsService _instance;
    public static ColorSettingsService Instance => _instance ??= new ColorSettingsService();

    private const string ColorKey = "SelectedColor";
    private Color _selectedColor = Colors.Blue;

    public event EventHandler<Color> ColorChanged;

    public Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (_selectedColor != value)
            {
                _selectedColor = value;
                SaveColor();
                ColorChanged?.Invoke(this, value);
                Application.Current.Resources["Primary"] = value;
            }
        }
    }

    private ColorSettingsService()
    {
        LoadColor();
    }

    private void SaveColor()
    {
        var colorString = _selectedColor.ToArgbHex();
        Preferences.Default.Set(ColorKey, colorString);
    }

    private void LoadColor()
    {
        var colorString = Preferences.Default.Get(ColorKey, Colors.Blue.ToArgbHex());
        _selectedColor = Color.FromArgb(colorString);
        Application.Current.Resources["Primary"] = _selectedColor;
    }
} 