/*
 * Key: 9D9A7EF4361F02B0AA73F59EFE41FA2CA043903E1C5D6E66C200A26CECAB32CAD771C4FF1C384ED48C3CB8BAB2E4F621 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserInfo user = new UserInfo();
        string filelocation = "Data/GeneralInfo.catalan";
       
        public MainWindow()
        {
            InitializeComponent();
            Newreg_Popup_1.IsOpen = false;
        }

        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            Username.Text = "";
            Username.GotFocus -= Username_GotFocus;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private bool CheckUsernameValid(string s)
        {
            for (int i = 0; i < s.Count() - 1; ++i)
                if (s[i] == '%') return false;
            return true;
        }
        private void Newreg_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), filelocation)))
            {
                Newreg_Popup_2.IsOpen = true;
            } else if (Username.Text=="Nhập tên người dùng" || Password.Password.ToString() == "" || Username.Text=="" || !CheckUsernameValid(Username.Text))
            {
                Newreg_Popup_1.IsOpen = true;
            } else
            {
                user.CreateNewUser(Username.Text, Password.Password.ToString());
                Newreg_Popup_3.IsOpen = true;
            }
        }
        private void Signin_Click(object sender, RoutedEventArgs e)
        {
            CheckforAccess();
        }

        private void CheckforAccess()
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filelocation)) || !user.CheckValid(Username.Text, Password.Password.ToString()))
            {
                Signin_Popup_1.IsOpen = true;
            }
            else
            {
                try
                {
                    TcpClient client = new TcpClient();
                    client.Connect(HostIP.Text, 5000);
                    Signin_Popup_2.IsOpen = true;
                    Main abc = new Main(HostIP.Text, user.Pass);
                    abc.Show();
                    Close();
                }
                catch
                {
                    Signin_Popup_3.IsOpen = true;
                }
            }
        }

        
        private void close_newreg_popup_1_Click(object sender, RoutedEventArgs e)
        {
            Newreg_Popup_1.IsOpen = false;
        }

        private void close_newreg_popup_2_Click(object sender, RoutedEventArgs e)
        {
            Newreg_Popup_2.IsOpen = false;
        }

        private void close_newreg_popup_3_Click(object sender, RoutedEventArgs e)
        {
            Newreg_Popup_3.IsOpen = false;
        }

        private void close_signin_popup_2_Click(object sender, RoutedEventArgs e)
        {
            Signin_Popup_2.IsOpen = false;
            Close();
        }

        private void Password_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key) CheckforAccess();
        }


        private void Username_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key) CheckforAccess();
        }

        private void HostIP_GotFocus(object sender, RoutedEventArgs e)
        {
            HostIP.Text = "";
            HostIP.GotFocus -= HostIP_GotFocus;
        }

        private void close_signin_popup_1_Click(object sender, RoutedEventArgs e)
        {
            Signin_Popup_1.IsOpen = false;
        }

        #region DragMove Window 
        private Point startPoint;
        private void Drag_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(Drag);
        }

        private void Drag_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var currentPoint = e.GetPosition(Drag);
            if (e.LeftButton == MouseButtonState.Pressed &&
                Drag.IsMouseCaptured &&
                (Math.Abs(currentPoint.X - startPoint.X) >
                    SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(currentPoint.Y - startPoint.Y) >
                    SystemParameters.MinimumVerticalDragDistance))
            {
                // Prevent Click from firing
                Drag.ReleaseMouseCapture();
                DragMove();
            }
        }
        #endregion

        private void close_signin_popup_3_Click(object sender, RoutedEventArgs e)
        {
            Signin_Popup_3.IsOpen = false;
        }
    }
}
