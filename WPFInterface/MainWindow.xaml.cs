using System.Diagnostics;
using System.Linq;
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
        private void Simple_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded) return;

            if(_avoidRecursion) return;
            _avoidRecursion = true;

            // Delete invalid characters from the text fields

            ValidateSimpleTextFields();

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
            }
            
            // Because this method is called whenever the text is updated, we need to ensure that
            // when the program is updating the text, we don't go into a loop.
            // The _avoidRecursion variable prevents this.

            Binary.Text = number.AsBase(2);
            Decimal.Text = number.AsBase(10);
            Hex.Text = number.AsBase(16);

            _avoidRecursion = false;
        }

        private void ValidateSimpleTextFields()
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



        private void Custom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded) return;

            if(_avoidRecursion) return;
            _avoidRecursion = true;

            // Delete invalid characters from the text fields

            ValidateComplexTextFields();

            TextBox textBox = sender as TextBox;
            Number number = new Number(0);

            // Set the value of number depending on the text field that was just changed

            switch (textBox.Name)
            {
                case "Custom1":
                    number = new Number(textBox.Text, int.Parse(Base1.Text));
                    break;
                case "Custom2":
                    number = new Number(textBox.Text, int.Parse(Base2.Text));
                    break;
                case "Custom3":
                    number = new Number(textBox.Text, int.Parse(Base3.Text));
                    break;
            }
            
            // Because this method is called whenever the text is updated, we need to ensure that
            // when the program is updating the text, we don't go into a loop.
            // The _avoidRecursion variable prevents this.

            // Get the bases of each field so we can convert their values correctly

            int base1 = int.Parse(Base1.Text);
            int base2 = int.Parse(Base2.Text);
            int base3 = int.Parse(Base3.Text);

            Custom1.Text = number.AsBase(base1);
            Custom2.Text = number.AsBase(base2);
            Custom3.Text = number.AsBase(base3);

            _avoidRecursion = false;
        }

        private void ValidateComplexTextFields()
        {
            // Make sure that the base fields are only numbers

            Base1.Text = new Regex("[^0-9]").Replace(Base1.Text, "");
            Base2.Text = new Regex("[^0-9]").Replace(Base2.Text, "");
            Base3.Text = new Regex("[^0-9]").Replace(Base3.Text, "");

            // Get the bases of each field so we can validate them

            if (!int.TryParse(Base1.Text, out int base1)) base1 = 10;
            if (!int.TryParse(Base2.Text, out int base2)) base2 = 2;
            if (!int.TryParse(Base3.Text, out int base3)) base3 = 16;

            // If the bases are invalid set them to their default values

            if (base1 <= 1 || base1 > 36) base1 = 10;
            if (base2 <= 1 || base2 > 36) base2 = 2;
            if (base3 <= 1 || base3 > 36) base3 = 16;

            // Set the values of the base fields in case they were invalid

            Base1.Text = base1.ToString();
            Base2.Text = base2.ToString();
            Base3.Text = base3.ToString();

            // Store the current position of the caret

            int caret1 = Custom1.CaretIndex;
            int caret2 = Custom2.CaretIndex;
            int caret3 = Custom3.CaretIndex;

            // If they entered invalid text, take one away from the caret so it will end up in the same place

            char[] validCharacters = BaseConverter.GetValidCharactersForBase(base1);
            if (!string.IsNullOrEmpty(Base1.Text) && Custom1.Text.ToCharArray().Any(x => !validCharacters.Contains(x))) caret1--;

            validCharacters = BaseConverter.GetValidCharactersForBase(base2);
            if(!string.IsNullOrEmpty(Custom2.Text) && Custom2.Text.ToCharArray().Any(x => !validCharacters.Contains(x))) caret2--;

            validCharacters = BaseConverter.GetValidCharactersForBase(base3);
            if(!string.IsNullOrEmpty(Custom3.Text) && Custom3.Text.ToCharArray().Any(x => !validCharacters.Contains(x))) caret3--;

            // Remove the invalid text

            validCharacters = BaseConverter.GetValidCharactersForBase(base1);
            Custom1.Text = string.Concat(Custom1.Text.ToCharArray().Where(x => validCharacters.Contains(x)));

            validCharacters = BaseConverter.GetValidCharactersForBase(base2);
            Custom2.Text = string.Concat(Custom2.Text.ToCharArray().Where(x => validCharacters.Contains(x)));

            validCharacters = BaseConverter.GetValidCharactersForBase(base3);
            Custom3.Text = string.Concat(Custom3.Text.ToCharArray().Where(x => validCharacters.Contains(x)));

            // Set the position of the caret back to what it was before

            Custom1.CaretIndex = caret1;
            Custom2.CaretIndex = caret2;
            Custom3.CaretIndex = caret3;
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
