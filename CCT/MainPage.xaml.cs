using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Collections.ObjectModel;
using CalculatorApp.Views;
using CalculatorApp.Services;

namespace CalculatorApp;

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
            else if (child is HorizontalStackLayout stackLayout)
            {
                foreach (var stackChild in stackLayout.Children)
                {
                    if (stackChild is Button stackButton)
                    {
                        stackButton.BackgroundColor = color;
                    }
                    else if (stackChild is Frame frame)
                    {
                        frame.BackgroundColor = color;
                        frame.BorderColor = color;
                    }
                }
            }
        }
    }

    private async void OnScientificCalculatorClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.ScientificCalculatorPage));
    }

    private void OnNumberClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (isNewNumber)
            {
                currentNumber = button.Text;
                isNewNumber = false;
            }
            else
            {
                currentNumber += button.Text;
            }
            DisplayLabel.Text = currentNumber;
        }
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (!string.IsNullOrEmpty(currentOperator))
            {
                CalculateResult();
            }
            firstNumber = double.Parse(currentNumber);
            currentOperator = button.Text;
            isNewNumber = true;
        }
    }

    private void OnEqualsClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(currentOperator))
        {
            CalculateResult();
        }
    }

    private void CalculateResult()
    {
        double secondNumber = double.Parse(currentNumber);
        double result = 0;

        switch (currentOperator)
        {
            case "+":
                result = firstNumber + secondNumber;
                break;
            case "-":
                result = firstNumber - secondNumber;
                break;
            case "×":
                result = firstNumber * secondNumber;
                break;
            case "÷":
                if (secondNumber != 0)
                    result = firstNumber / secondNumber;
                else
                {
                    DisplayLabel.Text = "Error";
                    return;
                }
                break;
        }

        var calculation = $"{firstNumber} {currentOperator} {secondNumber} = {result}";
        _historyService.AddCalculation(calculation);
        DisplayLabel.Text = result.ToString();
        currentNumber = result.ToString();
        currentOperator = "";
        isNewNumber = true;
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        currentNumber = "0";
        currentOperator = "";
        firstNumber = 0;
        isNewNumber = true;
        DisplayLabel.Text = currentNumber;
    }

    private void OnSignChangeClicked(object sender, EventArgs e)
    {
        if (currentNumber != "0")
        {
            if (currentNumber.StartsWith("-"))
            {
                currentNumber = currentNumber.Substring(1);
            }
            else
            {
                currentNumber = "-" + currentNumber;
            }
            DisplayLabel.Text = currentNumber;
        }
    }

    private void OnPercentClicked(object sender, EventArgs e)
    {
        double number = double.Parse(currentNumber);
        number = number / 100;
        currentNumber = number.ToString();
        DisplayLabel.Text = currentNumber;
    }

    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (!currentNumber.Contains("."))
        {
            currentNumber += ".";
            isNewNumber = false;
            DisplayLabel.Text = currentNumber;
        }
    }

    private void OnThemeSwitchToggled(object sender, ToggledEventArgs e)
    {
        Application.Current.UserAppTheme = e.Value ? AppTheme.Light : AppTheme.Dark;
    }

    private async void OnViewHistoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.HistoryPage));
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ColorPage());
    }

    private async void OnHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HistoryPage());
    }
} 