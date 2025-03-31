using System.Collections.ObjectModel;
using CalculatorApp.Services;

namespace CalculatorApp.Views;

public partial class ScientificCalculatorPage : ContentPage
{
    private string currentNumber = "0";
    private string currentOperator = "";
    private double firstNumber = 0;
    private bool isNewNumber = true;
    private readonly CalculationHistoryService _historyService;
    private readonly ColorSettingsService _colorSettingsService;

    public ScientificCalculatorPage()
    {
        InitializeComponent();
        _historyService = CalculationHistoryService.Instance;
        _colorSettingsService = ColorSettingsService.Instance;
        _colorSettingsService.ColorChanged += OnColorChanged;
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
        var mainGrid = this.FindByName<Grid>("ScientificGrid");
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

    private void OnScientificFunctionClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            double number = double.Parse(currentNumber);
            double result = 0;
            string function = button.Text;

            switch (function)
            {
                case "sin":
                    result = Math.Sin(number * Math.PI / 180);
                    function = "sin";
                    break;
                case "cos":
                    result = Math.Cos(number * Math.PI / 180);
                    function = "cos";
                    break;
                case "tan":
                    result = Math.Tan(number * Math.PI / 180);
                    function = "tan";
                    break;
                case "log":
                    if (number > 0)
                        result = Math.Log10(number);
                    else
                    {
                        DisplayLabel.Text = "Error";
                        return;
                    }
                    function = "log";
                    break;
                case "√":
                    if (number >= 0)
                        result = Math.Sqrt(number);
                    else
                    {
                        DisplayLabel.Text = "Error";
                        return;
                    }
                    function = "√";
                    break;
                case "x²":
                    result = number * number;
                    function = "x²";
                    break;
                case "π":
                    result = Math.PI;
                    function = "π";
                    break;
                case "e":
                    result = Math.E;
                    function = "e";
                    break;
            }

            var calculation = $"{function}({number}) = {result}";
            _historyService.AddCalculation(calculation);
            currentNumber = result.ToString();
            DisplayLabel.Text = currentNumber;
            isNewNumber = true;
        }
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