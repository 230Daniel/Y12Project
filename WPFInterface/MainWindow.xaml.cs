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
            // Load the user interface
            InitializeComponent();

            // Set the default character set in all required places
            // This could be loaded from a file but I thought it'd be better to have default settings each time it starts.

            const string defaultCharacters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Characters.Text = defaultCharacters;
            BaseConverter.Characters = defaultCharacters.ToCharArray();
            CharactersLabel.Content = $"Characters: Base {BaseConverter.Characters.Length}";
        }

        private bool _avoidRecursion;

        // Simple tab

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

            // Set the value of each box to the value converted to the box's base

            Binary.Text = number.AsBase(2);
            Decimal.Text = number.AsBase(10);
            Hex.Text = number.AsBase(16);

            // Because we're not changing the values programatically,
            // we no longer need to worry about recursion

            _avoidRecursion = false;
        }

        private void ValidateSimpleTextFields()
        {
            // Store the current position of the caret so that we can restore it later
            
            int decimalCaret = Decimal.CaretIndex;
            int binaryCaret = Binary.CaretIndex;
            int hexCaret = Hex.CaretIndex;

            // Also store the current length of the text so if we remove text we know how much has been removed

            int decimalOriginalLength = Decimal.Text.Length;
            int binaryOriginalLength = Binary.Text.Length;
            int hexOriginalLength = Hex.Text.Length;

            // Remove the invalid text

            char[] validCharacters = BaseConverter.GetValidCharactersForBase(10);
            Decimal.Text = string.Concat(Decimal.Text.ToCharArray().Where(x => validCharacters.Contains(x)));

            validCharacters = BaseConverter.GetValidCharactersForBase(2);
            Binary.Text = string.Concat(Binary.Text.ToCharArray().Where(x => validCharacters.Contains(x)));

            validCharacters = BaseConverter.GetValidCharactersForBase(16);
            Hex.Text = string.Concat(Hex.Text.ToCharArray().Where(x => validCharacters.Contains(x)));

            // If we've removed text we need to alter the caret position so it doesn't move on the screen

            decimalCaret -= decimalOriginalLength - Decimal.Text.Length;
            binaryCaret -= binaryOriginalLength - Binary.Text.Length;
            hexCaret -= hexOriginalLength - Hex.Text.Length;

            // It's possible that our caret has been moved below 0. To avoid an error, make sure this can't happen

            if (decimalCaret < 1) decimalCaret = 0;
            if (binaryCaret < 1) binaryCaret = 0;
            if (hexCaret < 1) hexCaret = 0;

            // Restore the position of the caret

            Decimal.CaretIndex = decimalCaret;
            Binary.CaretIndex = binaryCaret;
            Hex.CaretIndex = hexCaret;
        }

        // Custom tab

        private void Custom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded) return;

            if(_avoidRecursion) return;
            _avoidRecursion = true;

            // Delete invalid characters from the text fields
            ValidateCustomBaseTextFields();
            ValidateCustomTextFields();

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

            // Set the value of each box to the value converted to the box's base

            Custom1.Text = number.AsBase(base1);
            Custom2.Text = number.AsBase(base2);
            Custom3.Text = number.AsBase(base3);

            // Because we're not changing the values programatically,
            // we no longer need to worry about recursion

            _avoidRecursion = false;
        }

        private void ValidateCustomTextFields()
        {
            // Get the bases of each field so we can validate them

            if (!int.TryParse(Base1.Text, out int base1)) base1 = -1;
            if (!int.TryParse(Base2.Text, out int base2)) base2 = -1;
            if (!int.TryParse(Base3.Text, out int base3)) base3 = -1;

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

            if (caret1 < 1) caret1 = 0;
            if (caret2 < 1) caret2 = 0;
            if (caret3 < 1) caret3 = 0;

            Custom1.CaretIndex = caret1;
            Custom2.CaretIndex = caret2;
            Custom3.CaretIndex = caret3;
        }

        private void Custom_BaseTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded) return;

            ValidateCustomBaseTextFields();
        }

        private void ValidateCustomBaseTextFields()
        {
            // Make sure that the base fields are only numbers

            Base1.Text = new Regex("[^0-9]").Replace(Base1.Text, "");
            Base2.Text = new Regex("[^0-9]").Replace(Base2.Text, "");
            Base3.Text = new Regex("[^0-9]").Replace(Base3.Text, "");

            // Change appearence based on validity

            if (int.TryParse(Base1.Text, out int base1) && base1 <= BaseConverter.Characters.Length && base1 > 1)
            {
                Custom1.IsEnabled = true;
                Custom1Label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                Custom1Label.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                Custom1.IsEnabled = false;
            }
            Custom1Label.Content = $"Base {base1}";

            if (int.TryParse(Base2.Text, out int base2) && base2 <= BaseConverter.Characters.Length && base2 > 1)
            {
                Custom2.IsEnabled = true;
                Custom2Label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                Custom2Label.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                Custom2.IsEnabled = false;
            }
            Custom2Label.Content = $"Base {base2}";

            if (int.TryParse(Base3.Text, out int base3) && base3 <= BaseConverter.Characters.Length && base3 > 1)
            {
                
                Custom3.IsEnabled = true;
                Custom3Label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                Custom3Label.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                Custom3.IsEnabled = false;
            }
            Custom3Label.Content = $"Base {base3}";
        }

        // Settings tab

        private void Settings_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded) return;

            ValidateSettingsTextFields();
            TextBox textBox = sender as TextBox;

            switch (textBox.Name)
            {
                case "Characters":
                    BaseConverter.Characters = textBox.Text.ToCharArray();
                    CharactersLabel.Content = $"Characters: Base {BaseConverter.Characters.Length}";
                    break;
            }
        }

        private void ValidateSettingsTextFields()
        {
            int charactersCaret = Characters.CaretIndex;

            string oldCharacters = Characters.Text;
            Characters.Text = string.Concat(Characters.Text.ToCharArray().Distinct());

            if (oldCharacters.Length > Characters.Text.Length) charactersCaret--;

            if (charactersCaret < 1) charactersCaret = 0;
            Characters.CaretIndex = charactersCaret;
        }

        // Title bar controls

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
