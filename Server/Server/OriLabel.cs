using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Server
{
    public class OriLabel
    {
        public Label x;
        public OriLabel(string s)
        {
            x = new Label();
            x.Padding = new Thickness(12, 10, 0, 0);
            x.FontFamily = new FontFamily("r0c0i Linotte Light");
            x.FontSize = 18;
            x.Content = s;
        }
        
    }
}
