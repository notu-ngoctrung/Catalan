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

namespace Client.Control
{
    /// <summary>
    /// Interaction logic for FriendmessControl.xaml
    /// </summary>
    public partial class FriendmessControl : UserControl
    {
        private string imagename, type;
        public FriendmessControl()
        {
            InitializeComponent();
            text.Text = imagename = type= "";
        }
        public void SetText(string x)
        {
            text.Text = x;
        }

        public void SetImage(BitmapImage x, string name)
        {
            imagename = name; if (x.Width <= 370)
            {
                image.Height = x.Height;
                image.Width = x.Width;
            }
            else
            {
                image.Width = 370;
                image.Height = x.Height * 370.0 / x.Width;
            }
            image.Source = x;
        }
        public void SetType(string x)
        {
            type = x;
        }
        public void SetTime(string x)
        {
            time.Text = x;
        }
        public string GetText()
        {
            return text.Text;
        }

        public string GetImage()
        {
            return imagename;
        }

        public string GetTime()
        {
            return time.Text;
        }
        public string Gettype()
        {
            return type;
        }
    }
}
