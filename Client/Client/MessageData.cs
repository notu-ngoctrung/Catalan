using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Client
{
    class MessageData
    {
        public string username, filename, lastmess, lasttime;
        private string password;
        public bool focused;
        public struct Mess
        {
            public string time, type, fromwho, content;
            public Mess(string _time, string _type, string _fromwho,string _content)
            {
                time = _time;
                type = _type;
                fromwho = _fromwho;
                content = _content;
            }
        }

        public List<Mess> Cov = new List<Mess>();
        public MessageData(string name, string pass, string tmp)
        {
            focused = false;
            password = pass;
            if (tmp != "")
            {
                username = tmp;
                filename = username + ".mess";
                lastmess = "Các bạn vừa được kết nối!";
                lasttime = DateTime.Now.ToString("hh:mm");
            } else
            {
                filename = name;
                using (StreamReader x = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Message", filename)))
                {
                    /* Username */
                    username = x.ReadLine();
                    username = AES.Decrypt(username, password);

                    /* Messenge: Time - Type - From who? - Content */
                    string time, type, content, fromwho;
                    while ((time = x.ReadLine()) != null)
                    {
                        type = x.ReadLine();
                        fromwho = x.ReadLine();
                        content = x.ReadLine();

                        time = AES.Decrypt(time, password);
                        type = AES.Decrypt(type, password);
                        fromwho = AES.Decrypt(fromwho, password);
                        content = AES.Decrypt(content, password);

                        AddMess(time, type, fromwho, content);
                    }
                }
            }
        }

        public void SaveWork()
        {
            using (StreamWriter x = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Data","Message", filename)))
            {
                x.WriteLine(AES.Encrypt(username, password));

                /* Messenge: Time - Type - From who? - Content */
                foreach (var xx in Cov)
                {
                    x.WriteLine(AES.Encrypt(xx.time, password));
                    x.WriteLine(AES.Encrypt(xx.type, password));
                    x.WriteLine(AES.Encrypt(xx.fromwho, password));
                    x.WriteLine(AES.Encrypt(xx.content, password));
                }
            }
        }
        public Control.FriendmessControl createNewFriendmessControl(Mess mes)
        {
            Control.FriendmessControl mess = new Control.FriendmessControl();
            if (mes.type=="EMOJ")
            {
                BitmapImage tmp = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Emoji", mes.content), UriKind.Absolute));
                mess.SetImage(tmp, mes.content);
            } else if (mes.type == "IMAG")
            {
                //BitmapImage tmp = new BitmapImage(new Uri(@"P:\Projects\Catalan\Catalan\Client\Client\bin\Debug\Data\Download\" + mes.content, UriKind.RelativeOrAbsolute));
                //BitmapImage tmp = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Download", mes.content), UriKind.Absolute));
                BitmapImage tmp = new BitmapImage(new Uri(mes.content, UriKind.Absolute));
                mess.SetImage(tmp, mes.content);
            }
            else mess.SetText(mes.content);
            mess.SetType(mes.type);
            mess.SetTime(mes.time);
            return mess;
        }

        public Control.MymessControl createNewMymessControl(Mess mes)
        {
            Control.MymessControl mess = new Control.MymessControl();
            if (mes.type == "EMOJ")
            {
                BitmapImage tmp = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Emoji", mes.content), UriKind.Absolute));
                mess.SetImage(tmp, mes.content);
            }
            else if (mes.type == "IMAG")
            {
                BitmapImage tmp = new BitmapImage(new Uri(mes.content,UriKind.Absolute));
                mess.SetImage(tmp, mes.content);
            }
            else mess.SetText(mes.content);
            mess.SetType(mes.type);
            mess.SetTime(mes.time);
            return mess;
        }
        public void AddMess(string time, string type, string fromwho, string content)
        {
            Cov.Add(new Mess(time, type, fromwho, content));
            lastmess = content;
            lasttime = time;
        }
    }
}
