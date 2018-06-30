using FtpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ftpServer
{
    class Client
    {
        public static void Run()
        {
            // DO ZROBIENIA
            //SERWER NAJPIERW ODBIERA STATUS OD KLIENTA CO CHCE ZROBIC (1-SCIAGNAC PLIK, 2-WSTAWIC PLIK, 3-LISTA DOSTEPNYCH PLIKOW NA SERWERZE)
            //ODPOWIEDNIE DZIALANIE, ZASTANOWIC SIE NAD LOCKOWANIEM PLIKOW PODCZAS ODCZYTU CZY ZAPISU
            //
            Console.WriteLine("Connecting to server");
            object locker = new object();
            String serverIp = IPAddress.Loopback.ToString();
            TcpClient client = new TcpClient(serverIp, 5670);
            Console.WriteLine("Connected");
            IFormatter formatter = new BinaryFormatter();
            NetworkStream strm = client.GetStream();
            bool flag = true;
            while (flag) {
                Menu();
                string choosen = Console.ReadLine();
                switch (choosen)
                {
                    case "1":
                        formatter.Serialize(strm, choosen);
                        String listOfFiles = (String)formatter.Deserialize(strm);
                        Msg(listOfFiles);
                        break;
                    case "2":
                        formatter.Serialize(strm, choosen);
                        Msg("type which file you want to download?:");
                        String file = Console.ReadLine();
                        SharedFile downloaded = null;
                        try {  
                            formatter.Serialize(strm, file);
                        }
                        catch (Exception e)
                        {
                            Msg(e.Message);
                        }
                        finally
                        {
                             downloaded= (SharedFile)formatter.Deserialize(strm);
                        }
                        Msg("file downloaded");
                        Msg("where you want to save file?:");
                        String savePath = Console.ReadLine();
                        if (downloaded != null) { 
                        downloaded.Save(savePath);
                        }
                        Msg("File saved");
                        break;
                    case "3":
                        formatter.Serialize(strm, choosen);
                        Msg("type which file you want to send:");
                        String filePath = Console.ReadLine();
                        SharedFile toUpload = new SharedFile(filePath);
                        formatter.Serialize(strm, toUpload);
                        Msg("File sended");
                        break;
                    default:
                        flag = false;
                        strm.Close();
                        client.Close();
                        Environment.Exit(1);
                        break;
                }
            }
          
        }
        private static void Menu()
        {
            Console.WriteLine("LIST OF FILES: 1");
            Console.WriteLine("TO DOWNLOAD FILE: 2");
            Console.WriteLine("TO UPLOAD FILE: 3");
            Console.WriteLine("TO QUIT: 4");
        }
        private static void Msg(String msg)
        {
            Console.WriteLine(msg);
        }
    }
}
