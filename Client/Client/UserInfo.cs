using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class UserInfo
    {
        public string Name = "", Pass = "";
        string DefaultKEY = "9D9A7EF4361F02B0AA73F59EFE41FA2CA043903E1C5D6E66C200A26CECAB32CAD771C4FF1C384ED48C3CB8BAB2E4F621";
        string filelocation = "Data/GeneralInfo.catalan";

        public UserInfo()
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filelocation))) return;
            else
            {
                using (StreamReader x = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), filelocation)))
                {
                    Name = AES.Decrypt(x.ReadLine(), DefaultKEY);
                    Pass = AES.Decrypt(x.ReadLine(), DefaultKEY);
                }
            }
        }

        public void CreateNewUser(string name, string pass)
        {
            string encrypt_password = AES.Encrypt(pass, DefaultKEY);
            string encrypt_username = AES.Encrypt(name, DefaultKEY);

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Data"))) { Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data")); }

            using (StreamWriter x = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), filelocation)))
            {
                x.WriteLine(encrypt_username);
                x.WriteLine(encrypt_password);
            }

            Name = name;
            Pass = pass;
        }

        public bool CheckValid(string name, string pass)
        {
            return ((name == Name) && (pass == Pass));
        }
    }
}
