using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CalculatorLibrary;

namespace WPFInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool _avoidRecursion;
        private void Convert_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_avoidRecursion) return;
            _avoidRecursion = true;

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

            Binary.Text = number.AsBase(2);
            Decimal.Text = number.AsBase(10);
            Hex.Text = number.AsBase(16);

            _avoidRecursion = false;
        }

        private void ValidateTextFields()
        {
            // Store the current position of the caret

            int decimalCaret = Decimal.CaretIndex;
            int binaryCaret = Binary.CaretIndex;
            int hexCaret = Hex.CaretIndex;

            // If they entered invalid text, take one away from the caret so it will end up in the same place

            if (new Regex("[^0-9]").IsMatch(Decimal.Text)) decimalCaret--;
            if (new Regex("[^0-1]").IsMatch(Binary.Text)) binaryCaret--;
            if (new Regex("[^0-9a-fA-F]").IsMatch(Hex.Text)) hexCaret--;

            // Replace the invalid text with an empty string

            Decimal.Text = new Regex("[^0-9]").Replace(Decimal.Text, "");
            Binary.Text = new Regex("[^0-1]").Replace(Binary.Text, "");
            Hex.Text = new Regex("[^0-9a-fA-F]").Replace(Hex.Text, "").ToLower();

            // Set the position of the caret back to what it was before

            Decimal.CaretIndex = decimalCaret;
            Binary.CaretIndex = binaryCaret;
            Hex.CaretIndex = hexCaret;
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
    }
}
