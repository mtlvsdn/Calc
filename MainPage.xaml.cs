using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using CalculatorApp.Views;
using CalculatorApp.Services;

namespace CalculatorApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private string currentNumber = "0";
        private string currentOperator = "";
        private double firstNumber = 0;
        private bool isNewNumber = true;
        private readonly CalculationHistoryService _historyService;
        private readonly ColorSettingsService _colorSettingsService;

        public MainPage()
        {
            InitializeComponent();
            _historyService = CalculationHistoryService.Instance;
            _colorSettingsService = ColorSettingsService.Instance;
            _colorSettingsService.ColorChanged += OnColorChanged;
            Application.Current.UserAppTheme = AppTheme.Dark;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateButtonColors(_colorSettingsService.SelectedColor);
        }

        private void OnColorChanged(object sender, Color color)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateButtonColors(color);
            });
        }

        private void UpdateButtonColors(Color color)
        {
            // Update all buttons in the main grid
            var mainGrid = this.FindByName<Grid>("CalculatorGrid");
            if (mainGrid != null)
            {
                // Update all buttons recursively
                UpdateButtonsInGrid(mainGrid, color);
            }
        }

        private void UpdateButtonsInGrid(Grid grid, Color color)
        {
            foreach (var child in grid.Children)
            {
                if (child is Button button)
                {
                    button.BackgroundColor = color;
                }
                else if (child is Grid childGrid)
                {
                    UpdateButtonsInGrid(childGrid, color);
                }
            }
        }

        private async void OnScientificCalculatorClicked(object sender, EventArgs e)
        {
            // Implementation of the method
        }
    }
} 