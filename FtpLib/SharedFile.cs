using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpLib
{
    [Serializable]
    public class SharedFile
    {
        private String name { get; set; }
        private byte[] content { get; set; }
    
        public SharedFile(String path)
        {
            content = File.ReadAllBytes(path);
            name = path.Substring(path.LastIndexOf("\\"));
        }

        public void Save(String path)
        {
            if (!path.Substring(path.Length - 1).Equals("\\"))
            {
                path = path + "\\";
            }
            path = path + name;
            File.WriteAllBytes(path, content);
        }
    }

}
