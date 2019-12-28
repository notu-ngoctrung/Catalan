using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Server
{
    public class ClientHandler
    {
        public TcpClient client;
        public NetworkStream nwStream;
        private MainWindow win = null;

        public string username;
        
        public ClientHandler(TcpClient a, MainWindow b)
        {
            client = a;
            win = b;
        }
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public void receiver()
        {
            nwStream = client.GetStream();
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
                        username = _namebuffer;

                        win.WPFLog.Dispatcher.Invoke(() =>
                        {
                            OriLabel tmp = new OriLabel(DateTime.Now.ToString("hh:mm:ss") + " - " + username + " vừa mới đăng nhập.");
                            win.WPFLog.Children.Insert(0, tmp.x);
                        });
                        win.WPFOnList.Dispatcher.Invoke(() =>
                        {
                            OriLabel tmp = new OriLabel(username);
                            win.WPFOnList.Children.Insert(0, tmp.x);
                        });
                        for (int i = 0; i < MainWindow.ClientNameList.Count; ++i) sendtexttoX("SIGN" + scaleto32char(MainWindow.ClientNameList[i]),"", username);
                        MainWindow.ClientNameList.Add(username);
                        sendtexttoAll("SIGN" + scaleto32char(username));
                    } else if (_typbuffer == "TEXT")
                    {
                        byte[] lengthbuffer = new byte[4];
                        bytesRead = nwStream.Read(lengthbuffer, 0, 4);
                        int totalbyte = BitConverter.ToInt32(lengthbuffer, 0);

                        byte[] textbuffer = new byte[totalbyte];
                        bytesRead = nwStream.Read(textbuffer, 0, totalbyte);
                        string _textbuffer = Encoding.UTF8.GetString(textbuffer);

                        win.WPFLog.Dispatcher.Invoke(() =>
                        {
                            OriLabel tmp = new OriLabel(DateTime.Now.ToString("hh:mm:ss") + " - " + username + " gửi " + _namebuffer + " một tin nhắn.");
                            win.WPFLog.Children.Insert(0, tmp.x);
                        });
                        sendtexttoX("TEXT" + scaleto32char(username), _textbuffer, _namebuffer);
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
                            bytesRead = nwStream.Read(tmpbuffer, 0, totalbyte-tmpp);
                            Buffer.BlockCopy(tmpbuffer, 0, imagbuffer, tmpp, bytesRead);
                            tmpp += bytesRead;
                        }
                        // End read
                        //MessageBox.Show(totalbyte + " - " + bytesRead);
                        
                        byte[] buffer = new byte[40 + totalbyte];
                        Buffer.BlockCopy(typbuffer, 0, buffer, 0, 4);
                        Buffer.BlockCopy(lengthbuffer, 0, buffer, 36, 4);
                        Buffer.BlockCopy(imagbuffer, 0, buffer, 40, totalbyte);
                        win.WPFLog.Dispatcher.Invoke(() =>
                        {
                            OriLabel tmp = new OriLabel(DateTime.Now.ToString("hh:mm:ss") + " - " + username + " gửi " + _namebuffer + " một hình ảnh.");
                            win.WPFLog.Children.Insert(0, tmp.x);
                        });

                        byte[] receivebyte = Encoding.UTF8.GetBytes(scaleto32char(username));
                        Buffer.BlockCopy(receivebyte, 0, buffer, 4, receivebyte.Length);
                        sendbytetoX(buffer, _namebuffer);
                    } else if (_typbuffer == "EMOJ")
                    {
                        byte[] lengthbuffer = new byte[4];
                        bytesRead = nwStream.Read(lengthbuffer, 0, 4);
                        int totalbyte = BitConverter.ToInt32(lengthbuffer, 0);

                        byte[] textbuffer = new byte[totalbyte];
                        bytesRead = nwStream.Read(textbuffer, 0, totalbyte);
                        string _textbuffer = Encoding.UTF8.GetString(textbuffer);

                        win.WPFLog.Dispatcher.Invoke(() =>
                        {
                            OriLabel tmp = new OriLabel(DateTime.Now.ToString("hh:mm:ss") + " - " + username + " gửi " + _namebuffer + " một tin nhắn.");
                            win.WPFLog.Children.Insert(0, tmp.x);
                        });

                        sendtexttoX("EMOJ" + scaleto32char(username), _textbuffer, _namebuffer);
                    } else if (_typbuffer == "FILE")
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

                        byte[] buffer = new byte[40 + totalbyte];
                        Buffer.BlockCopy(typbuffer, 0, buffer, 0, 4);
                        Buffer.BlockCopy(lengthbuffer, 0, buffer, 36, 4);
                        Buffer.BlockCopy(imagbuffer, 0, buffer, 40, totalbyte);
                        win.WPFLog.Dispatcher.Invoke(() =>
                        {
                            OriLabel tmp = new OriLabel(DateTime.Now.ToString("hh:mm:ss") + " - " + username + " gửi " + _namebuffer + " bản ghi âm.");
                            win.WPFLog.Children.Insert(0, tmp.x);
                        });

                        byte[] receivebyte = Encoding.UTF8.GetBytes(scaleto32char(username));
                        Buffer.BlockCopy(receivebyte, 0, buffer, 4, receivebyte.Length);
                        sendbytetoX(buffer, _namebuffer);
                    } else if (_typbuffer == "LOGO")
                    {
                        win.WPFLog.Dispatcher.Invoke(() =>
                        {
                            OriLabel tmp = new OriLabel(DateTime.Now.ToString("hh:mm:ss") + " - " + username + " vừa rời khỏi hệ thống.");
                            win.WPFLog.Children.Insert(0, tmp.x);
                        });
                        win.WPFOnList.Dispatcher.Invoke(() =>
                        {
                            for(int i = 0; i < win.WPFOnList.Children.Count; ++i)
                            {
                                if ((win.WPFOnList.Children[i] as Label).Content.ToString() == username)
                                {
                                    win.WPFOnList.Children.RemoveAt(i);
                                    break;
                                }
                            }
                        });
                        MainWindow.ClientNameList.Remove(username);
                        sendtexttoAll("LOGO" + scaleto32char(username));
                    }
                } catch
                {
                    if (!client.Connected)
                    {
                        foreach (ClientHandler x in MainWindow.ClientList)
                            if (x.username == username) { x.username = ""; break; }

                        username = "";
                        return;
                    }
                }
            }
        }


        void sendtexttoAll(string ori)
        {
            foreach (ClientHandler x in MainWindow.ClientList)
            {
                try
                {
                    if (x.username != username)
                    {
                        byte[] bytesToSend = Encoding.UTF8.GetBytes(ori);
                        x.nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                }
                catch
                {

                }
            }
        }

        void sendtexttoX(string header, string content, string desname)
        {
            foreach(ClientHandler x in MainWindow.ClientList)
            {
                if (x.username == desname)
                {
                    byte[] headerbuffer = Encoding.UTF8.GetBytes(header);

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
                        x.nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                    else x.nwStream.Write(headerbuffer, 0, headerbuffer.Length);
                    
                }
            }
        }

        void sendbytetoX(byte[] tmp,string desname)
        {
            //MessageBox.Show(desname);
            foreach (ClientHandler x in MainWindow.ClientList)
            {
                if (x.username == desname)
                {
                    x.nwStream.Write(tmp, 0, tmp.Length);
                    break;
                }
            }
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
    }
}
