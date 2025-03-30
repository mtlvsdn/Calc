using System.Collections.ObjectModel;

namespace CalculatorApp;

public partial class MainPage : ContentPage
{
    private double currentNumber = 0;
    private double? previousNumber = null;
    private string currentOperator = null;
    private bool isNewNumber = true;
    private ObservableCollection<string> calculationHistory;

    public MainPage()
    {
        InitializeComponent();
        calculationHistory = new ObservableCollection<string>();
        Application.Current.UserAppTheme = AppTheme.Dark;
    }

    private void OnNumberClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        if (isNewNumber)
        {
            DisplayLabel.Text = button.Text;
            isNewNumber = false;
        }
        else
        {
            DisplayLabel.Text += button.Text;
        }
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        currentNumber = double.Parse(DisplayLabel.Text);
        currentOperator = button.Text;
        previousNumber = currentNumber;
        isNewNumber = true;
    }

    private void OnEqualsClicked(object sender, EventArgs e)
    {
        if (previousNumber.HasValue && currentOperator != null)
        {
            currentNumber = double.Parse(DisplayLabel.Text);
            double result = 0;

            switch (currentOperator)
            {
                case "+":
                    result = previousNumber.Value + currentNumber;
                    break;
                case "-":
                    result = previousNumber.Value - currentNumber;
                    break;
                case "×":
                    result = previousNumber.Value * currentNumber;
                    break;
                case "÷":
                    if (currentNumber != 0)
                        result = previousNumber.Value / currentNumber;
                    else
                    {
                        DisplayLabel.Text = "Error";
                        return;
                    }
                    break;
            }

            string calculation = $"{previousNumber} {currentOperator} {currentNumber} = {result}";
            calculationHistory.Add(calculation);
            DisplayLabel.Text = result.ToString();
            isNewNumber = true;
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        DisplayLabel.Text = "0";
        currentNumber = 0;
        previousNumber = null;
        currentOperator = null;
        isNewNumber = true;
    }

    private void OnSignChangeClicked(object sender, EventArgs e)
    {
        if (DisplayLabel.Text != "0")
        {
            if (DisplayLabel.Text.StartsWith("-"))
                DisplayLabel.Text = DisplayLabel.Text.Substring(1);
            else
                DisplayLabel.Text = "-" + DisplayLabel.Text;
        }
    }

    private void OnPercentClicked(object sender, EventArgs e)
    {
        currentNumber = double.Parse(DisplayLabel.Text);
        currentNumber = currentNumber / 100;
        DisplayLabel.Text = currentNumber.ToString();
    }

    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (!DisplayLabel.Text.Contains("."))
        {
            DisplayLabel.Text += ".";
            isNewNumber = false;
        }
    }

    private void OnThemeSwitchToggled(object sender, ToggledEventArgs e)
    {
        Application.Current.UserAppTheme = e.Value ? AppTheme.Light : AppTheme.Dark;
    }

    private async void OnViewHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HistoryPage(calculationHistory));
    }
} 