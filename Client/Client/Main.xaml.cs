using System;
using System.Collections.Generic;
using System.IO;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        UserInfo user = new UserInfo();
        string SERVER_IP = "", password = "";
        string CurrentFriendUserName;
        public static bool NeedtoSendDrawing, NeedtoSendRecord;
        int unreadcount;

        TcpClient client;
        NetworkStream nwStream;

        Thread task;
        List<MessageData> UserConvDetail = new List<MessageData>();
        List<string> UserOnlList = new List<string>();
        FileInfo[] files;
        int count = 0;
        public Main(string x, string pass)
        {
            unreadcount = 0;
            NeedtoSendDrawing = NeedtoSendRecord = false;
            SERVER_IP = x;
            InitializeComponent();
            ConnectServer();

            CurrentFriendUserName = "";

            password = pass;

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Data","Message"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Message"));
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Data","Download"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Download"));

            DirectoryInfo d = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Data","Message"));
            FileInfo[] Files = d.GetFiles("*.mess");
            foreach (FileInfo file in Files)
            {
                MessageData tmp = new MessageData(file.Name, password, "");
                Control.PreviewMessageControl tmppreview = new Control.PreviewMessageControl();
                tmppreview.SetName(tmp.username);
                tmppreview.SetText(tmp.lastmess, tmp.lasttime);
                tmppreview.SetRead();

                tmppreview.PreviewMouseLeftButtonDown += Tmppreview_PreviewMouseLeftButtonDown;

                UserConvDetail.Add(tmp);
                WPFPreview.Children.Add(tmppreview);
            }

            #region Emoji Add Button
            string baseDir =
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Emoji\\";
            DirectoryInfo di = new DirectoryInfo(baseDir);

            files = di.GetFiles();
            foreach (FileInfo fi in files)
            {

                Button butt = new Button();
                butt.Content = new Image
                {
                    Stretch = Stretch.None,
                    Source = new BitmapImage(new Uri(baseDir + fi.ToString())),
                    VerticalAlignment = VerticalAlignment.Center,
                };
                butt.Name = '_' + count.ToString();
                butt.Padding = new Thickness(0);
                butt.Click += Butt_Click;
                wp1.Children.Add(butt);
                count++;
            }
            #endregion
        }
        #region Emoji Helper Function
        private void Butt_Click(object sender, RoutedEventArgs e)
        {
            WPFEmojiBoard.IsOpen = false;
            Button butt = sender as Button;
            string s = butt.Name.Remove(0, 1);
            int number = Int32.Parse(s);
            sendText("EMOJ" + scaleto32char(CurrentFriendUserName), files[number].ToString());
            AddEmojiMessageFrom("Me", CurrentFriendUserName, DateTime.Now, files[number].ToString());
        }
        #endregion

        private void Tmppreview_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Control.PreviewMessageControl x in WPFPreview.Children)
            {
                if (x.focused) { x.SetunFocus(); break; }
            }
            string tmp_username = (sender as Control.PreviewMessageControl).username;

            if ((sender as Control.PreviewMessageControl).unread) RefreshUnreadNumber(--unreadcount);

            (sender as Control.PreviewMessageControl).SetFocus();
            CurrentFriendUserName = tmp_username;
            WPFWriteBox.Dispatcher.Invoke(() => { WPFWriteBox.Text = CurrentFriendUserName; });

            foreach (MessageData x in UserConvDetail)
            {
                x.focused = false;
                if (x.username == tmp_username)
                {
                    WPFConversation.Children.Clear();
                    foreach (MessageData.Mess tmpmess in x.Cov)
                    {
                        if (tmpmess.fromwho == "Me") WPFConversation.Children.Add(x.createNewFriendmessControl(tmpmess));
                        else WPFConversation.Children.Add(x.createNewMymessControl(tmpmess));
                    }
                    x.focused = true;
                }
            }
        }

        private void PreviewMessageButton_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {

        }

        void ConnectServer()
        {
            client = new TcpClient(SERVER_IP, 5000);
            nwStream = client.GetStream();

            task = new Thread(new ThreadStart(Listener));
            task.Start();

            sendText("SIGN" + scaleto32char(user.Name), "");
        }

        #region Send - Receive
        void Listener()
        {
            while (true)
            {
                try
                {
                    byte[] typbuffer = new byte[4];
                    int bytesRead = nwStream.Read(typbuffer, 0, typbuffer.Length);
                    string _typbuffer = Encoding.UTF8.GetString(typbuffer, 0, typbuffer.Length);

                    byte[] namebuffer = new byte[32];
                    bytesRead = nwStream.Read(namebuffer, 0, 32);
                    string _namebuffer = Encoding.UTF8.GetString(namebuffer, 0, namebuffer.Length);
                    _namebuffer = scaleback(_namebuffer);
                    if (_typbuffer == "SIGN")
                    {
                        WPFPreview.Dispatcher.Invoke(() =>
                        {
                            foreach (Control.PreviewMessageControl x in WPFPreview.Children)
                            {
                                if (x.username == _namebuffer) { x.SetOnl(); break; }
                            }
                        });
                        UserOnlList.Add(_namebuffer);
                    } else if (_typbuffer == "TEXT")
                    {
                        byte[] lengthbuffer = new byte[4];
                        bytesRead = nwStream.Read(lengthbuffer, 0, 4);
                        int totalbyte = BitConverter.ToInt32(lengthbuffer, 0);

                        byte[] textbuffer = new byte[totalbyte];
                        bytesRead = nwStream.Read(textbuffer, 0, totalbyte);
                        string _textbuffer = Encoding.UTF8.GetString(textbuffer);

                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            AddTextMessageFrom(_namebuffer, "Me", (DateTime.Now.ToString("hh:mm:ss")), _textbuffer);
                            // Gửi - Nhận - Time - Content
                        });
                    } else if (_typbuffer == "IMAG")
                    {
                        byte[] lengthbuffer = new byte[4];
                        bytesRead = nwStream.Read(lengthbuffer, 0, 4);
                        int totalbyte = BitConverter.ToInt32(lengthbuffer, 0);

                        // Read until get ENOUGH
                        byte[] imagbuffer = new byte[totalbyte];
                        int tmpp = 0;
                        while (tmpp < totalbyte)
                        {
                            byte[] tmpbuffer = new byte[totalbyte];
                            bytesRead = nwStream.Read(tmpbuffer, 0, totalbyte - tmpp);
                            Buffer.BlockCopy(tmpbuffer, 0, imagbuffer, tmpp, bytesRead);
                            tmpp += bytesRead;
                        }
                        // End read
                        //MessageBox.Show(totalbyte + " - " + bytesRead);
                        BitmapImage finalcontent = ToImage(imagbuffer);

                        DateTime tmp = DateTime.Now;
                        Save(finalcontent, @".\Data\Download\" + "Me" + tmp.ToString("hhmmss") + ".png");
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            //AddImageMessageFrom(sender, "Me", tmp, @".\Data\Download\" + "Me" + tmp.ToString("hhmmss") + ".png");
                            AddImageMessageFrom(_namebuffer, "Me", tmp, Path.Combine(Directory.GetCurrentDirectory(), "Data", "Download", "Me" + tmp.ToString("hhmmss") + ".png"));
                        });
                    } else if (_typbuffer == "EMOJ")
                    {
                        byte[] lengthbuffer = new byte[4];
                        bytesRead = nwStream.Read(lengthbuffer, 0, 4);
                        int totalbyte = BitConverter.ToInt32(lengthbuffer, 0);

                        byte[] textbuffer = new byte[totalbyte];
                        bytesRead = nwStream.Read(textbuffer, 0, totalbyte);
                        string _textbuffer = Encoding.UTF8.GetString(textbuffer);

                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            AddEmojiMessageFrom(_namebuffer, "Me", (DateTime.Now), _textbuffer);
                        });
                    } else if (_typbuffer == "FILE")
                    {
                        byte[] lengthbuffer = new byte[4];
                        bytesRead = nwStream.Read(lengthbuffer, 0, 4);
                        int totalbyte = BitConverter.ToInt32(lengthbuffer, 0);

                        // Read until get ENOUGH
                        byte[] filebuffer = new byte[totalbyte];
                        int tmpp = 0;
                        while (tmpp < totalbyte)
                        {
                            byte[] tmpbuffer = new byte[totalbyte];
                            bytesRead = nwStream.Read(tmpbuffer, 0, totalbyte - tmpp);
                            Buffer.BlockCopy(tmpbuffer, 0, filebuffer, tmpp, bytesRead);
                            tmpp += bytesRead;
                        }
                        // End read

                        DateTime tmp = DateTime.Now;
                        File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Download", tmp.ToString("hhmmss") + ".wav"), filebuffer);
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            AddTextMessageFrom(_namebuffer, "Me", tmp.ToString("hh:mm:ss"), _namebuffer + " vừa gửi một file ghi âm "+"'"+tmp.ToString("hhmmss") + ".wav" +"'"+ " cho bạn.");
                        });
                    } else if (_typbuffer == "LOGO")
                    {
                        //MessageBox.Show(_namebuffer);
                        WPFPreview.Dispatcher.Invoke(() =>
                        {
                            foreach (Control.PreviewMessageControl x in WPFPreview.Children)
                            {
                                if (x.username == _namebuffer) { x.SetOff(); break; }
                            }
                        });
                        UserOnlList.Remove(_namebuffer);
                    }
                }
                catch
                {
                    if (!client.Connected) break;
                }
            }
        }
        /*void Listener()
        {
            while (true)
            {
                try
                {
                    #region Nghe đến khi nào DataAvailable = false 
                    List<byte> fastup = new List<byte>();
                    byte[] buffer;
                    byte[] tbuffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(tbuffer, 0, client.ReceiveBufferSize);
                    byte[] type = new byte[4];
                    Buffer.BlockCopy(tbuffer, 0, type, 0, 4);
                    int totalbytesRead = bytesRead;

                    if (Encoding.UTF8.GetString(type) == "FILE" || Encoding.UTF8.GetString(type) == "IMAG")
                    {
                        for (int i = 0; i < bytesRead; ++i) fastup.Add(tbuffer[i]);

                        int totalbytesMustRead = BitConverter.ToInt32(tbuffer, 36);

                        while (totalbytesRead < totalbytesMustRead)
                        {
                            if (!nwStream.DataAvailable) continue;
                            tbuffer = new byte[client.ReceiveBufferSize];
                            bytesRead = nwStream.Read(tbuffer, 0, client.ReceiveBufferSize);
                            // Combine two array
                            for (int i = 0; i < bytesRead; ++i) fastup.Add(tbuffer[i]);
                            totalbytesRead += bytesRead;
                        }

                        buffer = fastup.ToArray();
                    }
                    else
                    {
                        buffer = tbuffer;
                    }
                    #endregion

                    //MessageBox.Show("Số byte: " + totalbytesRead.ToString());

                    switch (Encoding.UTF8.GetString(type))
                    {
                        case "SIGN":
                            string recev = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            string friend = recev.Substring(4);
                            // Start code edit WPFPreview vào đây
                            WPFPreview.Dispatcher.Invoke(() =>
                            {
                                foreach (Control.PreviewMessageControl x in WPFPreview.Children)
                                {
                                    if (x.username == friend) { x.SetOnl(); break; }
                                }
                            });
                            WPFWriteBox.Dispatcher.Invoke(() =>
                            {
                                WPFWriteBox.Text = recev;
                            });
                            // End code edit WPFPreview
                            UserOnlList.Add(friend);
                            break;
                        case "TEXT":
                            recev = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            recev = recev.Substring(4);
                            string sender = "";
                            while (recev[0] != '%')
                            {
                                sender += (recev[0]);
                                recev = recev.Substring(1);
                            }
                            recev = recev.Substring(1);
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                AddTextMessageFrom(sender, "Me", (DateTime.Now.ToString("hh:mm:ss")), recev);
                            });
                            break;
                        case "IMAG":
                            byte[] receivebyte = new byte[32]; // Lấy từ server
                            Buffer.BlockCopy(buffer, 4, receivebyte, 0, 32);
                            sender = scaleback(Encoding.UTF8.GetString(receivebyte));

                            byte[] content = new byte[totalbytesRead - 40];
                            Buffer.BlockCopy(buffer, 40, content, 0, totalbytesRead - 40);
                            BitmapImage finalcontent = ToImage(content);
                            //Save(finalcontent, @".\IMG.png");
                            DateTime tmp = DateTime.Now;
                            Save(finalcontent, @".\Data\Download\" + "Me" + tmp.ToString("hhmmss") + ".png");
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                    //AddImageMessageFrom(sender, "Me", tmp, @".\Data\Download\" + "Me" + tmp.ToString("hhmmss") + ".png");
                                    AddImageMessageFrom(sender, "Me", tmp, Path.Combine(Directory.GetCurrentDirectory(), "Data", "Download", "Me" + tmp.ToString("hhmmss") + ".png"));
                            });
                            break;
                        case "FILE":
                            receivebyte = new byte[32]; // Lấy từ server
                            Buffer.BlockCopy(buffer, 4, receivebyte, 0, 32);
                            sender = scaleback(Encoding.UTF8.GetString(receivebyte));

                            content = new byte[totalbytesRead - 40];
                            Buffer.BlockCopy(buffer, 40, content, 0, totalbytesRead - 40);
                            tmp = DateTime.Now;
                            File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Download", tmp.ToString("hhmmss") + ".wav"), content);
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                AddTextMessageFrom(sender, "Me", tmp.ToString("hh:mm:ss"), sender + " vừa gửi một file ghi âm cho bạn.");
                            });
                            break;
                        case "EMOJ":
                            recev = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            recev = recev.Substring(4);
                            sender = "";
                            while (recev[0] != '%')
                            {
                                sender += (recev[0]);
                                recev = recev.Substring(1);
                            }
                            recev = recev.Substring(1);
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                AddEmojiMessageFrom(sender, "Me", (DateTime.Now), recev);
                            });
                            break;
                        case "LOGO":
                            recev = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            friend = recev.Substring(4);
                            // Start code edit WPFPreview vào đây
                            WPFPreview.Dispatcher.Invoke(() =>
                            {
                                foreach (Control.PreviewMessageControl x in WPFPreview.Children)
                                {
                                    if (x.username == friend) { x.SetOff(); break; }
                                }
                            });
                            WPFWriteBox.Dispatcher.Invoke(() =>
                            {
                                WPFWriteBox.Text = recev;
                            });
                            // End code edit WPFPreview
                            UserOnlList.Remove(friend);
                            break;
                        default:
                            // MessageBox.Show("?!?!?");
                            break;
                    }
                }
                catch
                {
                    try
                    {
                        int n = client.ReceiveBufferSize;
                    }
                    catch
                    {
                        break;
                    }
                }
            }

        }*/

        void sendText(string x, string content)
        {
            byte[] headerbuffer = Encoding.UTF8.GetBytes(x);

            if (content != "")
            {
                byte[] contentbuffer = Encoding.UTF8.GetBytes(content);
                byte[] bytesToSend = new byte[headerbuffer.Length + 4 + contentbuffer.Length];
                Buffer.BlockCopy(headerbuffer, 0, bytesToSend, 0, headerbuffer.Length);
                Buffer.BlockCopy(contentbuffer, 0, bytesToSend, 40, contentbuffer.Length);
                bytesToSend[39] = (byte)((contentbuffer.Length) >> 24);
                bytesToSend[38] = (byte)((contentbuffer.Length) >> 16);
                bytesToSend[37] = (byte)((contentbuffer.Length) >> 8);
                bytesToSend[36] = (byte)((contentbuffer.Length));
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            } else nwStream.Write(headerbuffer, 0, headerbuffer.Length);
        }

        string scaleto32char(string x)
        {
            string tmp = x;
            while (tmp.Length < 32) tmp += ' ';
            return tmp;
        }

        string scaleback(string x)
        {
            string tmp = "";
            foreach (char xx in x)
            {
                if (xx == ' ') break;
                tmp += xx;
            }
            return tmp;
        }
        void sendImage(string receiver, string path)
        {
            byte[] type = new byte[4];
            type = Encoding.UTF8.GetBytes("IMAG");
            byte[] _receiver = new byte[32];
            receiver = scaleto32char(receiver);
            _receiver = Encoding.UTF8.GetBytes(scaleto32char(receiver));
            byte[] img = File.ReadAllBytes(path);

            byte[] total = new byte[40 + img.Length];
            Buffer.BlockCopy(type, 0, total, 0, 4);
            Buffer.BlockCopy(_receiver, 0, total, 4, _receiver.Length);
            total[39] = (byte)((img.Length) >> 24);
            total[38] = (byte)((img.Length) >> 16);
            total[37] = (byte)((img.Length) >> 8);
            total[36] = (byte)((img.Length));
            Buffer.BlockCopy(img, 0, total, 40, img.Length);

            //MessageBox.Show(total.Length.ToString() + " - " + _receiver.Length.ToString() + "- " + type.Length.ToString());
            nwStream.Write(total, 0, total.Length);

            AddImageMessageFrom("Me",scaleback(receiver),DateTime.Now,path);
            //MessageBox.Show(path);
        }

        void sendRecord(string receiver, string path)
        {
            byte[] type = new byte[4];
            type = Encoding.UTF8.GetBytes("FILE");
            byte[] _receiver = new byte[32];
            receiver = scaleto32char(receiver);
            _receiver = Encoding.UTF8.GetBytes(scaleto32char(receiver));
            byte[] img = File.ReadAllBytes(path);

            byte[] total = new byte[40 + img.Length];
            Buffer.BlockCopy(type, 0, total, 0, 4);
            Buffer.BlockCopy(_receiver, 0, total, 4, _receiver.Length);
            total[39] = (byte)((img.Length) >> 24);
            total[38] = (byte)((img.Length) >> 16);
            total[37] = (byte)((img.Length) >> 8);
            total[36] = (byte)((img.Length));
            Buffer.BlockCopy(img, 0, total, 40, img.Length);

            //MessageBox.Show(total.Length.ToString() + " - " + _receiver.Length.ToString() + "- " + type.Length.ToString());
            nwStream.Write(total, 0, total.Length);
            AddTextMessageFrom("Me", scaleback(receiver), DateTime.Now.ToString("hh:mm:ss"), "Bạn đã gửi một tệp ghi âm.");
        }
        #endregion

        #region WPF Control
        private void WPFNewConv_Click(object sender, RoutedEventArgs e)
        {
            WPFNewConvDialog_DropList.Items.Clear();
            foreach (string x in UserOnlList) WPFNewConvDialog_DropList.Items.Add(x);
            foreach(Control.PreviewMessageControl x in WPFPreview.Children) WPFNewConvDialog_DropList.Items.Remove(x.username);
            WPFNewConvDialog.IsOpen = true;
        }

        private void WPFNewConvDialog_Accept_Click(object sender, RoutedEventArgs e)
        {
            MessageData tmp = new MessageData("", password, WPFNewConvDialog_DropList.Text);
            Control.PreviewMessageControl tmppreview = new Control.PreviewMessageControl();
            tmppreview.SetName(tmp.username);
            tmppreview.SetText(tmp.lastmess, tmp.lasttime);
            tmppreview.SetOnl();
            tmppreview.SetFocus();

            tmp.focused = true;
            CurrentFriendUserName = WPFNewConvDialog_DropList.Text;
            foreach (MessageData x in UserConvDetail) x.focused = false;
            WPFConversation.Children.Clear();

            tmppreview.PreviewMouseLeftButtonDown += Tmppreview_PreviewMouseLeftButtonDown;

            UserConvDetail.Add(tmp);
            WPFPreview.Children.Insert(0, tmppreview);

            WPFNewConvDialog.IsOpen = false;
        }

        private void WPFNewConvDialog_Close_Click(object sender, RoutedEventArgs e)
        {
            WPFNewConvDialog.IsOpen = false;
        }

        private void WPFWriteBox_GotFocus(object sender, RoutedEventArgs e)
        {
            WPFWriteBox.Text = "";
        }

        private void WPFWriteBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                if (CurrentFriendUserName == "") return;
                DateTime currenttime = DateTime.Now;
                AddTextMessageFrom("Me", CurrentFriendUserName, currenttime.ToString("hh:mm:ss"), WPFWriteBox.Text);
                sendText("TEXT" + scaleto32char(CurrentFriendUserName), WPFWriteBox.Text);

                WPFWriteBox.Text = "";
            }
        }
        private void WPFMarkAllRead_Click(object sender, RoutedEventArgs e)
        {
            unreadcount = 0; RefreshUnreadNumber(unreadcount);
            WPFPreview.Dispatcher.Invoke(() =>
            {
                foreach (Control.PreviewMessageControl x in WPFPreview.Children) x.SetRead();
            });
        }
        private void WPFImageSendButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                sendImage(CurrentFriendUserName, filename);
                WPFOtherFormSendDialog.IsOpen = false;
            }
        }
        private void WPFOtherFormSendDialog_Send_Click(object sender, RoutedEventArgs e)
        {
            if (NeedtoSendDrawing)
            {
                NeedtoSendDrawing = false;
                sendImage(CurrentFriendUserName, Path.Combine(Directory.GetCurrentDirectory(), "canvas.PNG"));
                WPFOtherFormSendDialog.IsOpen = false;
            }
            if (NeedtoSendRecord)
            {
                NeedtoSendRecord = false;
                sendRecord(CurrentFriendUserName, Path.Combine(Directory.GetCurrentDirectory(), "record.wav"));
                WPFOtherFormSendDialog.IsOpen = false;
            }
        }
        private void WPFRecordSendButton_Click(object sender, RoutedEventArgs e)
        {
            WPFOtherFormSendDialog.IsOpen = true;
            Record_MainWindow abc = new Record_MainWindow();
            abc.Show();
        }

        private void WPFDrawingSendButton_Click(object sender, RoutedEventArgs e)
        {
            WPFOtherFormSendDialog.IsOpen = true;
            Drawing_MainWindow abc = new Drawing_MainWindow();
            abc.Show();
        }

        private void WPFOtherFormSendDialog_Close_Click(object sender, RoutedEventArgs e)
        {
            WPFOtherFormSendDialog.IsOpen = false;
        }

        private void WPFAdditionButton_Click(object sender, RoutedEventArgs e)
        {
            WPFOtherFormSendDialog.IsOpen = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            sendText("LOGO" + scaleto32char(user.Name),"");
            foreach (MessageData x in UserConvDetail) x.SaveWork();
            client.GetStream().Close();
            client.Close();
            Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void WPFEmojiButton_Click(object sender, RoutedEventArgs e)
        {
            WPFEmojiBoard.IsOpen = true;
        }
        #endregion

        #region Helper Functions 
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        void RefreshUnreadNumber(int num)
        {
            WPFUnreadNumber.Dispatcher.Invoke(() =>
            {
                WPFUnreadNumber.Content = "Inbox (" + num.ToString() + ")";
            });
        }
        void AddTextMessageFrom(string sender, string receiver, string time, string content)
        {
            if (sender != "Me")
            {
                bool processed = false;
                foreach (MessageData x in UserConvDetail)
                {
                    if (x.username == sender)
                    {
                        x.AddMess(time, "TEXT", sender, content);
                        if (x.focused)
                        {
                            WPFConversation.Dispatcher.Invoke(() =>
                            {
                                if (x.Cov[x.Cov.Count - 1].fromwho == "Me") WPFConversation.Children.Add(x.createNewFriendmessControl(x.Cov[x.Cov.Count - 1]));
                                else WPFConversation.Children.Add(x.createNewMymessControl(x.Cov[x.Cov.Count - 1]));
                                WPFConversation_Scroll.ScrollToBottom();
                            });
                            WPFPreview.Dispatcher.Invoke(() =>
                            {
                                foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                                {
                                    if (y.username == sender)
                                    {
                                        y.SetText(content, time.Substring(0, 5));
                                        y.SetRead();
                                        RefreshUnreadNumber(unreadcount);
                                        break;
                                    }
                                }
                            });
                        }
                        else
                        {
                            WPFPreview.Dispatcher.Invoke(() =>
                            {
                                foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                                {
                                    if (y.username == sender)
                                    {
                                        y.SetText(content, time.Substring(0, 5));
                                        RefreshUnreadNumber(++unreadcount);
                                        break;
                                    }
                                }
                            });
                        }
                        processed = true;
                        break;
                    }
                }
                if (!processed)
                {
                    // Tạo 1 Message Data mới và 1 Control.PreviewMessageControl mới
                    MessageData tmp = new MessageData("", password, sender);
                    tmp.AddMess(time, "TEXT", sender, content);
                    WPFConversation.Children.Clear();
                    WPFConversation.Children.Add(tmp.createNewMymessControl(tmp.Cov[0]));
                    tmp.focused = true; //
                    Control.PreviewMessageControl tmppreview = new Control.PreviewMessageControl();
                    tmppreview.SetName(tmp.username);
                    tmppreview.SetText(tmp.lastmess, tmp.lasttime);
                    tmppreview.SetOnl();
                    tmppreview.SetFocus();
                    CurrentFriendUserName = sender; //
                    WPFConversation.Children.Clear();//
                    WPFConversation.Children.Add(tmp.createNewMymessControl(tmp.Cov[0]));//

                    foreach (MessageData x in UserConvDetail) x.focused = false;//

                    tmppreview.PreviewMouseLeftButtonDown += Tmppreview_PreviewMouseLeftButtonDown;

                    UserConvDetail.Add(tmp);
                    WPFPreview.Dispatcher.Invoke(() =>
                    {
                        foreach (Control.PreviewMessageControl x in WPFPreview.Children) x.SetunFocus(); // 
                        WPFPreview.Children.Insert(0, tmppreview);
                    });
                }
            }
            else
            {
                foreach (MessageData x in UserConvDetail)
                {
                    if (x.username == receiver)
                    {
                        x.AddMess(time, "TEXT", sender, content);
                        WPFConversation.Dispatcher.Invoke(() =>
                        {
                            if (x.Cov[x.Cov.Count - 1].fromwho == "Me") WPFConversation.Children.Add(x.createNewFriendmessControl(x.Cov[x.Cov.Count - 1]));
                            else WPFConversation.Children.Add(x.createNewMymessControl(x.Cov[x.Cov.Count - 1]));
                            WPFConversation_Scroll.ScrollToBottom();
                        });
                        WPFPreview.Dispatcher.Invoke(() =>
                        {
                            foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                            {
                                if (y.username == sender)
                                {
                                    y.SetText(content, time.Substring(0, 5));
                                    y.SetRead();
                                    RefreshUnreadNumber(unreadcount);
                                    break;
                                }
                            }
                        });
                        break;
                    }
                }
            }
        }

        void AddImageMessageFrom(string sender, string receiver, DateTime time, string Img)
        {
            if (sender != "Me")
            {
                bool processed = false;
                foreach (MessageData x in UserConvDetail)
                {
                    if (x.username == sender)
                    {
                        x.AddMess(time.ToString("hh:mm:ss"), "IMAG", sender,Img);
                        if (x.focused)
                        {
                            WPFConversation.Dispatcher.Invoke(() =>
                            {
                                if (x.Cov[x.Cov.Count - 1].fromwho == "Me") WPFConversation.Children.Add(x.createNewFriendmessControl(x.Cov[x.Cov.Count - 1]));
                                else WPFConversation.Children.Add(x.createNewMymessControl(x.Cov[x.Cov.Count - 1]));
                                WPFConversation_Scroll.ScrollToBottom();
                            });
                            WPFPreview.Dispatcher.Invoke(() =>
                            {
                                foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                                {
                                    if (y.username == sender)
                                    {
                                        y.SetText(receiver.ToString() + time.ToString("hhmmss") + ".png", time.ToString("hh:mm"));
                                        y.SetRead();
                                        RefreshUnreadNumber(unreadcount);
                                        break;
                                    }
                                }
                            });
                        }
                        else
                        {
                            WPFPreview.Dispatcher.Invoke(() =>
                            {
                                foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                                {
                                    if (y.username == sender)
                                    {
                                        y.SetText(receiver.ToString() + time.ToString("hhmmss") + ".png", time.ToString("hh:mm"));
                                        RefreshUnreadNumber(++unreadcount);
                                        break;
                                    }
                                }
                            });
                        }
                        processed = true;
                        break;
                    }
                }
                if (!processed)
                {
                    // Tạo 1 Message Data mới và 1 Control.PreviewMessageControl mới
                    MessageData tmp = new MessageData("", password, sender);
                    tmp.AddMess(time.ToString("hh:mm:ss"), "IMAG", sender, Img);
                    tmp.focused = true;
                    Control.PreviewMessageControl tmppreview = new Control.PreviewMessageControl();
                    tmppreview.SetName(tmp.username);
                    tmppreview.SetText(tmp.lastmess, tmp.lasttime);
                    tmppreview.SetOnl();
                    tmppreview.SetFocus();
                    CurrentFriendUserName = sender; //
                    WPFConversation.Children.Clear();
                    WPFConversation.Children.Add(tmp.createNewMymessControl(tmp.Cov[0]));

                    foreach (MessageData x in UserConvDetail) x.focused = false;//
                    tmppreview.PreviewMouseLeftButtonDown += Tmppreview_PreviewMouseLeftButtonDown;

                    UserConvDetail.Add(tmp);
                    WPFPreview.Dispatcher.Invoke(() =>
                    {
                        foreach (Control.PreviewMessageControl x in WPFPreview.Children) x.SetunFocus(); // 
                        WPFPreview.Children.Insert(0, tmppreview);
                    });
                }
            }
            else
            {
                foreach (MessageData x in UserConvDetail)
                {
                    if (x.username == receiver)
                    {
                        x.AddMess(time.ToString("hh:mm:ss"), "IMAG", sender, Img);
                        WPFConversation.Dispatcher.Invoke(() =>
                        {
                            if (x.Cov[x.Cov.Count - 1].fromwho == "Me") WPFConversation.Children.Add(x.createNewFriendmessControl(x.Cov[x.Cov.Count - 1]));
                            else WPFConversation.Children.Add(x.createNewMymessControl(x.Cov[x.Cov.Count - 1]));
                            WPFConversation_Scroll.ScrollToBottom();
                        });
                        WPFPreview.Dispatcher.Invoke(() =>
                        {
                            foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                            {
                                if (y.username == sender)
                                {
                                    y.SetText("Bạn đã gửi một tấm ảnh!", time.ToString("hh:mm"));
                                    y.SetRead();
                                    RefreshUnreadNumber(unreadcount);
                                    break;
                                }
                            }
                        });
                        break;
                    }
                }
            }
        }

        void AddEmojiMessageFrom(string sender, string receiver, DateTime time, string Img)
        {
            if (sender != "Me")
            {
                bool processed = false;
                foreach (MessageData x in UserConvDetail)
                {
                    if (x.username == sender)
                    {
                        x.AddMess(time.ToString("hh:mm:ss"), "EMOJ", sender, Img);
                        if (x.focused)
                        {
                            WPFConversation.Dispatcher.Invoke(() =>
                            {
                                if (x.Cov[x.Cov.Count - 1].fromwho == "Me") WPFConversation.Children.Add(x.createNewFriendmessControl(x.Cov[x.Cov.Count - 1]));
                                else WPFConversation.Children.Add(x.createNewMymessControl(x.Cov[x.Cov.Count - 1]));
                                WPFConversation_Scroll.ScrollToBottom();
                            });
                            WPFPreview.Dispatcher.Invoke(() =>
                            {
                                foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                                {
                                    if (y.username == sender)
                                    {
                                        y.SetText("[Emoji]", time.ToString("hh:mm"));
                                        y.SetRead();
                                        RefreshUnreadNumber(unreadcount);
                                        break;
                                    }
                                }
                            });
                        }
                        else
                        {
                            WPFPreview.Dispatcher.Invoke(() =>
                            {
                                foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                                {
                                    if (y.username == sender)
                                    {
                                        y.SetText("[Emoji]", time.ToString("hh:mm"));
                                        RefreshUnreadNumber(++unreadcount);
                                        break;
                                    }
                                }
                            });
                        }
                        processed = true;
                        break;
                    }
                }
                if (!processed)
                {
                    // Tạo 1 Message Data mới và 1 Control.PreviewMessageControl mới
                    MessageData tmp = new MessageData("", password, sender);
                    tmp.AddMess(time.ToString("hh:mm:ss"), "EMOJ", sender, Img);
                    tmp.focused = true;
                    Control.PreviewMessageControl tmppreview = new Control.PreviewMessageControl();
                    tmppreview.SetName(tmp.username);
                    tmppreview.SetText(tmp.lastmess, tmp.lasttime);
                    tmppreview.SetOnl();

                    foreach (MessageData x in UserConvDetail) x.focused = false;//
                    tmppreview.PreviewMouseLeftButtonDown += Tmppreview_PreviewMouseLeftButtonDown;
                    CurrentFriendUserName = sender; //
                    WPFConversation.Children.Clear();
                    WPFConversation.Children.Add(tmp.createNewMymessControl(tmp.Cov[0]));

                    UserConvDetail.Add(tmp);
                    WPFPreview.Dispatcher.Invoke(() =>
                    {
                        foreach (Control.PreviewMessageControl x in WPFPreview.Children) x.SetunFocus(); // 
                        WPFPreview.Children.Insert(0, tmppreview);
                    });
                }
            }
            else
            {
                foreach (MessageData x in UserConvDetail)
                {
                    if (x.username == receiver)
                    {
                        x.AddMess(time.ToString("hh:mm:ss"), "EMOJ", sender, Img);
                        WPFConversation.Dispatcher.Invoke(() =>
                        {
                            if (x.Cov[x.Cov.Count - 1].fromwho == "Me") WPFConversation.Children.Add(x.createNewFriendmessControl(x.Cov[x.Cov.Count - 1]));
                            else WPFConversation.Children.Add(x.createNewMymessControl(x.Cov[x.Cov.Count - 1]));
                            WPFConversation_Scroll.ScrollToBottom();
                        });
                        WPFPreview.Dispatcher.Invoke(() =>
                        {
                            foreach (Control.PreviewMessageControl y in WPFPreview.Children)
                            {
                                if (y.username == sender)
                                {
                                    y.SetText("[Emoji]", time.ToString("hh:mm"));
                                    y.SetRead();
                                    RefreshUnreadNumber(unreadcount);
                                    break;
                                }
                            }
                        });
                        break;
                    }
                }
            }
        }

        public void Save(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
        #endregion

        #region Drag Move
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
    }
}
