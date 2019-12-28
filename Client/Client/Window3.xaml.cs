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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
            foreach (FontFamily F in Fonts.SystemFontFamilies)
            {
                ComboBoxItem cbitem = new ComboBoxItem();
                cbitem.FontFamily = F;
                cbitem.Content = F.Source;
                //tb1.Text += F.Source;
                cb1.Items.Add(cbitem);
            }
            foreach (FontFamily F in Fonts.SystemFontFamilies)
            {
                ComboBoxItem cbitem = new ComboBoxItem();
                cbitem.FontFamily = F;
                cbitem.Content = F.Source;
                //tb1.Text += F.Source;
                cb1.Items.Add(cbitem);
            }
        }
        //public static readonly DependencyProperty MyFontFamilyProperty =
        //DependencyProperty.Register("MyFontFamily",
        //typeof(FontFamily), typeof(Window1), new UIPropertyMetadata(null));

        private void cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbitem = (ComboBoxItem)cb1.SelectedValue;
            tb1.FontFamily = cbitem.FontFamily;
            tblck.FontFamily = cbitem.FontFamily;
        }

        private void slValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tb1.FontSize = 12 + slValue.Value;
            tblck.FontSize = 12 + slValue.Value;
        }
        public bool flag = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flag = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
