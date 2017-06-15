using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CalculatorRD
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            result.Text = 0.ToString();
        }

        private void AddNumberToResult(double number)
        {
            if(char.IsNumber(result.Text.Last()))
            {
                if(result.Text.Length == 1 && result.Text == "0")
                {
                    result.Text = string.Empty;
                }
                result.Text += number;
            }
            else
            {
                if(number != 0)
                {
                    result.Text += number;
                }
            }
        }

        enum Operation { ADD = 1, SUB = 2, MUL = 3, DIV = 4, NUMBER = 5 }
        private void AddOperationToResult(Operation operation)
        {
            if (result.Text.Length == 1 && result.Text == "0") return;

            if(!char.IsNumber(result.Text.Last()))
            {
                result.Text = result.Text.Substring(0, result.Text.Length - 1);
            }

            switch (operation)
            {
                case Operation.ADD: result.Text += "+"; break;
                case Operation.SUB: result.Text += "-"; break;
                case Operation.MUL: result.Text += "*"; break;
                case Operation.DIV: result.Text += "/"; break;
            }
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(7);
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(8);
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(9);
        }

        private void btnDiv_Click(object sender, RoutedEventArgs e)
        {
            AddOperationToResult(Operation.DIV);
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(4);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(5);
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(6);
        }

        private void btnMul_Click(object sender, RoutedEventArgs e)
        {
            AddOperationToResult(Operation.MUL);
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(1);
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(2);
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(3);
        }

        private void btnSub_Click(object sender, RoutedEventArgs e)
        {
            AddOperationToResult(Operation.SUB);
        }

        #region Equal
        private class Operand
        {
            public Operation operation = Operation.NUMBER; //default
            public double value = 0;

            public Operand left = null;
            public Operand right = null;
        }

        private Operand BuildTreeOperand()
        {
            Operand tree = null;

            string expression = result.Text;
            if(!char.IsNumber(expression.Last()))
            {
                expression = expression.Substring(0, expression.Length - 1);
            }

            string numberStr = string.Empty;
            foreach(char c in expression.ToCharArray())
            {
                if(char.IsNumber(c) || c == '.' || numberStr == string.Empty && c == '-')
                {
                    numberStr += c;
                }
                else
                {
                    AddOperandToTree(ref tree, new Operand() { value = double.Parse(numberStr) });
                    numberStr = string.Empty;

                    Operation op = Operation.SUB; //default
                    switch(c)
                    {
                        case '-': op = Operation.SUB; break;
                        case '+': op = Operation.ADD; break;
                        case '*': op = Operation.MUL; break;
                        case '/': op = Operation.DIV; break;
                    }
                    AddOperandToTree(ref tree, new Operand() { operation = op });
                }
            }
            AddOperandToTree(ref tree, new Operand() { value = double.Parse(numberStr) });

            return tree;
        }

        private void AddOperandToTree(ref Operand tree, Operand elem)
        {
            if(tree == null)
            {
                tree = elem;
            }
            else
            {
                if(elem.operation < tree.operation)
                {
                    Operand auxTree = tree;
                    tree = elem;
                    elem.left = auxTree;
                }
                else
                {
                    AddOperandToTree(ref tree.right, elem);
                }
            }
        }

        private double Calc(Operand tree)
        {
            if(tree.left == null && tree.right == null)
            {
                return tree.value;
            }
            else
            {
                double subResult = 0;
                switch(tree.operation)
                {
                    case Operation.ADD: subResult = Calc(tree.left) + Calc(tree.right); break;
                    case Operation.SUB: subResult = Calc(tree.left) - Calc(tree.right); break;
                    case Operation.MUL: subResult = Calc(tree.left) * Calc(tree.right); break;
                    case Operation.DIV: subResult = Calc(tree.left) / Calc(tree.right); break;
                }
                return subResult;
            }
        }

        private void btnEqual_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(result.Text)) return;

            Operand tree = BuildTreeOperand();

            double value = Calc(tree);

            result.Text = value.ToString();
        }
        #endregion

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(0);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            result.Text = 0.ToString();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOperationToResult(Operation.ADD);
        }
        
        private void btnTheme_Click(object sender, RoutedEventArgs e)
        {
            if(RequestedTheme == ElementTheme.Dark)
            {
                this.RequestedTheme = ElementTheme.Light;
            }
            else
            {
                this.RequestedTheme = ElementTheme.Dark;
            }
        }
    }
}