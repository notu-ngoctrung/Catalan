using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static List<ClientHandler> ClientList = new List<ClientHandler>();
        public static List<string> ClientNameList = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            //trash();
        }

        void trash()
        {
            OriLabel tmp = new OriLabel("zenny_nguyen gửi cho rezolden một tin nhắn.");
            WPFLog.Children.Add(tmp.x);
        }
        TcpListener listen;
        private void WPFStart_Click(object sender, RoutedEventArgs e)
        {
            Thread task;
            if (listen == null)
            {
                task = new Thread(new ThreadStart(Listener));
                task.Start();
                WPFStart.Background = new SolidColorBrush(Color.FromRgb(235, 35, 43));
                WPFStart.Content = "Stop Server";
            } else
            {
                if (ClientNameList.Count > 0)
                {
                    bug1.IsOpen = true;
                    return;
                }
                listen.Stop();
                WPFStart.Background = new SolidColorBrush(Color.FromRgb(21, 118, 210));
                WPFStart.Content = "Start Server";
            }
        }

        Thread langnghe;
        void Listener()
        {
            listen = new TcpListener(IPAddress.Parse(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString()), 5000);
            listen.Start();

            WPFLog.Dispatcher.Invoke(()=>
            {
                OriLabel tmp = (new OriLabel((DateTime.Now.ToString("hh:mm:ss") + " - Đã khởi động server! (IP: " + Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString() + "; PORT: 5000).")));
                WPFLog.Children.Insert(0,tmp.x);
            });
           
            while (true)
            {
                try
                {
                    TcpClient client = listen.AcceptTcpClient();
                    bool kt = true;
                    foreach(ClientHandler x in ClientList)
                    {
                        if (x.client.Client.RemoteEndPoint == client.Client.RemoteEndPoint)
                        {
                            kt = false;
                            break;
                        }
                    }
                    if (kt)
                    {
                        ClientList.Add(new ClientHandler(client, this));
                        langnghe = new Thread(new ThreadStart(ClientList[ClientList.Count - 1].receiver));
                        langnghe.Start();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return;
                }
            }        
        }

        #region DragMove Close Minimize Window 
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

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (ClientNameList.Count > 0)
            {
                bug1.IsOpen = true;
                return;
            }
            listen.Stop();
            Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion

        private void close_newreg_popup_1_Click(object sender, RoutedEventArgs e)
        {
            bug1.IsOpen = false;
        }
    }
}
