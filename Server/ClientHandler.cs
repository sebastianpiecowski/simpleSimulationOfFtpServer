using FtpLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class ClientHandler
    {
        private Thread thread;
        private TcpClient client;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
            thread = new Thread(Run);
            thread.Start();
        }

        private void Run()
        {
            NetworkStream strm=null;
            String choosen = null;
            do
            {
                try
                {
                    strm = client.GetStream();
                    IFormatter formatter = new BinaryFormatter();
                    choosen = (String)formatter.Deserialize(strm);

                    switch (choosen)
                    {
                        case "1":
                            Console.WriteLine(choosen);
                            formatter.Serialize(strm, findAllStoredFiles());
                            break;
                        case "2":
                            Console.WriteLine(choosen);
                            try
                            {
                                String path = (String)formatter.Deserialize(strm);
                                Console.WriteLine(path);
                                SharedFile file = new SharedFile(path);

                                formatter.Serialize(strm, file);
                            }
                            catch (SerializationException se)
                            {
                                Console.WriteLine("Serialization exception" + se.Message);
                            }
                            break;
                        case "3":
                            Console.WriteLine(choosen);
                                try { 
                                SharedFile upload = (SharedFile)formatter.Deserialize(strm);
                                upload.Save("stored");
                                }
                                catch (SerializationException se)
                                {
                                    Console.WriteLine("Deserialization exception" + se.Message);
                                }
                                
                            break;
                    }
                }
                catch (Exception e)
                {
                    strm.Close();
                    Console.WriteLine(e.Message);
                    break;
                }
            } while (!choosen.Equals("4"));
            
        }

        private String[] findAllStoredFiles()
        {
            string[] storedFiles = Directory.GetFiles("stored");
           
            return storedFiles;
        }
    }
}
