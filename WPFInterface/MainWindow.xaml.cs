using System;
using System.Collections.Generic;
using System.Linq;
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
            Number num = new Number(10);
            Number num2 = new Number(15);

            Console.WriteLine((num + num2).AsBase(16));

            //Console.WriteLine(num.AsBase(2));

            InitializeComponent();
        }

        private void TitleBarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Label button = sender as Label;
            button.Background = new SolidColorBrush(new Color()
            {
                R = 63,
                G = 63,
                B = 65,
                A = 100
            });
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
