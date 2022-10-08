using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Operator selectedOperator;
        double lastNumber, result;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pressing down the left button will allow the user to drag the window
        /// </summary>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            //Prevents user from inputing a number with over 8-digits
            if (resultLabel.Content.ToString().Length < 8)
            {
                int selectedValue = int.Parse((sender as Button).Content.ToString());

                if (resultLabel.Content.ToString() == "0")
                    resultLabel.Content = $"{selectedValue}";
                else
                    resultLabel.Content = $"{resultLabel.Content}{selectedValue}";
            }
        }

        //reset calculator
        private void acButton_Click(object sender, RoutedEventArgs e)
        {
            result = 0;
            lastNumber = 0;
            resultLabel.Content = "0";
            previousNumLabel.Content = string.Empty;
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            if(double.TryParse(resultLabel.Content.ToString(), out lastNumber))
            {
                resultLabel.Content = "0";
            }

            if (sender == AddButton)
            {
                selectedOperator = Operator.Addition;
                previousNumLabel.Content = $"{lastNumber} +";
            }
            else if (sender == SubtractButton)
            {
                selectedOperator = Operator.Subtraction;
                previousNumLabel.Content = $"{lastNumber} -";
            }
            else if (sender == MultiplyButton)
            {
                selectedOperator = Operator.Multiplication;
                previousNumLabel.Content = $"{lastNumber} x";
            }
            else if (sender == DivideButton)
            {
                selectedOperator = Operator.Division;
                previousNumLabel.Content = $"{lastNumber} /";
            }

            previousNumLabel.FontSize = 25;
        }

        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            if(!resultLabel.Content.ToString().Contains("."))
                resultLabel.Content = $"{resultLabel.Content}.";
        }

        private void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(resultLabel.Content.ToString(), out lastNumber))
            {
                lastNumber = lastNumber * -1;
                resultLabel.Content = lastNumber.ToString();
            }
        }

        private void PercentButton_Click(object sender, RoutedEventArgs e)
        {
            double tempNumber;

            if (double.TryParse(resultLabel.Content.ToString(), out tempNumber))
            {
                tempNumber /= 100;
                if(lastNumber != 0)
                    tempNumber *= lastNumber;

                resultLabel.Content = tempNumber.ToString();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            double newNumber;
            if (double.TryParse(resultLabel.Content.ToString(), out newNumber))
            {
                switch (selectedOperator)
                {
                    case Operator.Addition:
                        previousNumLabel.Content = $"{lastNumber} + {newNumber} =";
                        result = SimpleMath.Add(lastNumber, newNumber);         
                        break;                                                  
                    case Operator.Subtraction:                                  
                        previousNumLabel.Content = $"{lastNumber} - {newNumber} =";
                        result = SimpleMath.Subtract(lastNumber, newNumber);    
                        break;                                                  
                    case Operator.Multiplication:                               
                        previousNumLabel.Content = $"{lastNumber} x {newNumber} =";
                        result = SimpleMath.Multiply(lastNumber, newNumber);    
                        break;                                                  
                    case Operator.Division:                                     
                        previousNumLabel.Content = $"{lastNumber} / {newNumber} =";
                        result = SimpleMath.Divide(lastNumber, newNumber);
                        break;
                }
            }

            //Prevents numbers over 8-digit from being displayed
            if (result.ToString().Length > 8) 
            {
                MessageBox.Show("This calculator can't display numbers with more than 8 digits.", "Display Error", MessageBoxButton.OK, MessageBoxImage.Error);
                acButton_Click(MessageBoxButton.OK, e);
                return;
            }

            resultLabel.Content = result.ToString();
        }
    }

    public enum Operator
    {
        Addition, Subtraction, Multiplication, Division, Negative, Percent
    }

    public class SimpleMath
    {
        public static double Add(double num1, double num2)
        {
            return num1 + num2;
        }

        public static double Subtract(double num1, double num2)
        {
            return num1 - num2;
        }

        public static double Multiply(double num1, double num2)
        {
            return num1 * num2;
        }

        public static double Divide(double num1, double num2)
        {
            if (num2 == 0)
            {
                MessageBox.Show("Can't Divide By Zero.", "Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }

            return num1 / num2;
        }

    }
}
