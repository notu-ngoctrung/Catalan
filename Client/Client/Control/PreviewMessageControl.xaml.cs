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
    /// Interaction logic for PreviewMessageControl.xaml
    /// </summary>
    public partial class PreviewMessageControl : UserControl
    {
        public bool unread, focused;
        public string username; 
        
        public PreviewMessageControl()
        {
            InitializeComponent();
            unread = focused = false;
            AvaColor.Background = new SolidColorBrush(Color.FromRgb(102, 106, 126));
        }
        
        // Requirements: SetName - SetText
        // Additional Functions: SetFocus - SetOnl
        public void SetName(string name)
        {
            Name.Content = name;
            username = name;
            AvaText.Content = (name.Substring(0, 1)).ToUpper();
        }
        
        public void SetText(string x, string y)
        {
            LastMess.Text = x;
            if (!focused)
            {
                unread = true;
                LastMess.FontFamily = new FontFamily("r0c0i Linotte Semi Bold");
                LastMess.Foreground = new SolidColorBrush(Color.FromRgb(33,149,243));
            }
            Time.Content = y.Substring(0,5);
        }
        
        public void SetFocus()
        {
            focused = true;
            SetRead();
            BorderBack.Background = new SolidColorBrush(Color.FromRgb(47, 50, 66));
        }

        public void SetunFocus()
        {
            focused = false;
            BorderBack.Background = new SolidColorBrush(Color.FromRgb(43, 45, 60));
        }
        public void SetOnl()
        {
            AvaColor.Background = new SolidColorBrush(Color.FromRgb(32, 145, 238));
        }

        public void SetOff()
        {
            AvaColor.Background = new SolidColorBrush(Color.FromRgb(102, 106, 126));
        }

        public void SetRead()
        {
            unread = false;
            LastMess.FontFamily = new FontFamily("r0c0i Linotte Light");
            LastMess.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
