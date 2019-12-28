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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        public string hexcolor = "", hexcolor2 = "";
        public bool flag = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flag = true;
            this.Visibility = Visibility.Hidden;
        }
        private void yellow_Click(object sender, RoutedEventArgs e)
        {
            hexcolor = ((SolidColorBrush)yellow.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;

        }

        private void red_Click(object sender, RoutedEventArgs e)
        {
            hexcolor = ((SolidColorBrush)red.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;

        }

        private void green_Click(object sender, RoutedEventArgs e)
        {
            hexcolor = ((SolidColorBrush)green.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;

        }

        private void blue_Click(object sender, RoutedEventArgs e)
        {
            hexcolor = ((SolidColorBrush)blue.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;

        }

        private void black_Click(object sender, RoutedEventArgs e)
        {
            hexcolor = ((SolidColorBrush)black.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;

        }

        private void white_Click(object sender, RoutedEventArgs e)
        {

            hexcolor = ((SolidColorBrush)white.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;
        }

        private void green_Copy_Click(object sender, RoutedEventArgs e)
        {
            hexcolor2 = ((SolidColorBrush)green_Copy.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;
        }

        private void blue_Copy_Click(object sender, RoutedEventArgs e)
        {
            hexcolor2 = ((SolidColorBrush)blue_Copy.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;
        }

        private void yellow_Copy_Click(object sender, RoutedEventArgs e)
        {
            hexcolor2 = ((SolidColorBrush)yellow.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;
        }

        private void white_Copy_Click(object sender, RoutedEventArgs e)
        {
            hexcolor2 = ((SolidColorBrush)white_Copy.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;
        }

        private void black_Copy_Click(object sender, RoutedEventArgs e)
        {
            hexcolor2 = ((SolidColorBrush)black_Copy.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;
        }

        private void red_Copy_Click(object sender, RoutedEventArgs e)
        {
            hexcolor2 = ((SolidColorBrush)red_Copy.Background).Color.ToString();
            this.Visibility = Visibility.Hidden;
        }
    }
}
