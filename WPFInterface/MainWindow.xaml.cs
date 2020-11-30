using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using CalculatorLibrary;

namespace WPFInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool _avoidRecursion;
        private void Convert_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_avoidRecursion) return;

            // Delete invalid characters from the text fields

            ValidateTextFields();

            TextBox textBox = sender as TextBox;
            Number number = new Number(0);

            // Set the value of number depending on the text field that was just changed

            switch (textBox.Name)
            {
                case "Binary":
                    number = new Number(textBox.Text, 2);
                    break;
                case "Decimal":
                    number = new Number(textBox.Text, 10);
                    break;
                case "Hex":
                    number = new Number(textBox.Text, 16);
                    break;
                case "Base36":
                    number = new Number(textBox.Text, 36);
                    break;
            }

            // Because this method is called whenever the text is updated, we need to ensure that
            // when the program is updating the text, we don't go into a loop.
            // The _avoidRecursion variable prevents this.

            if (!number.Invalid)
            {
                _avoidRecursion = true;
                Binary.Text = number.AsBase(2);
                Decimal.Text = number.AsBase(10);
                Hex.Text = number.AsBase(16);
                _avoidRecursion = false;
            }
        }

        private void ValidateTextFields()
        {
            Decimal.Text = new Regex("[^0-9]").Replace(Decimal.Text, "");
            Binary.Text = new Regex("[^0-1]").Replace(Binary.Text, "");
            Hex.Text = new Regex("[^0-9a-fA-F]").Replace(Hex.Text, "").ToLower();
        }

        private void TitleBarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Label button = sender as Label;
            if (button.Name == "CloseButton")
            {
                button.Background = new SolidColorBrush(new Color()
                {
                    R = 232,
                    G = 17,
                    B = 35,
                    A = 100
                });
            }
            else
            {
                button.Background = new SolidColorBrush(new Color()
                {
                    R = 63,
                    G = 63,
                    B = 65,
                    A = 100
                });
            }
            
        }

        private void TitleBarButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Label button = sender as Label;
            button.Background = new SolidColorBrush(new Color()
            {
                A = 0
            });
        }

        private void TitleBarButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label button = sender as Label;
            if (button.Name == "CloseButton")
            {
                Close();
            }
            else if (button.Name == "MinimiseButton")
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                WindowState = WindowState.Normal;
                DragMove();
            }
        }

        private void Convert_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
        }
    }
}
