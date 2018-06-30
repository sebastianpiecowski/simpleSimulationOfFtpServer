using FtpLib;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
namespace ClientForm
{
    public partial class Form1 : Form
    {
        IFormatter formatter;
        NetworkStream strm;
        public Form1()
        {
            InitializeComponent();
            String serverIp = IPAddress.Loopback.ToString();
            TcpClient client = new TcpClient(serverIp, 5670);
            status.Text = "Connected";
            formatter = new BinaryFormatter();
            strm = client.GetStream();
            button1_Click(button1, EventArgs.Empty);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            formatter.Serialize(strm, "1");
            String[] listOfFiles = (String[])formatter.Deserialize(strm);
            listBox1.Items.Clear();
            foreach(String item in listOfFiles)
            {
                listBox1.Items.Add(item);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            String item=null;
            try { 
                item= listBox1.SelectedItem.ToString();
                formatter.Serialize(strm, "2");
                SharedFile downloaded = null;
                try
                {
                    formatter.Serialize(strm, item);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Somethings goes wrong, " + exception.Message);
                }
                finally
                {
                    downloaded = (SharedFile)formatter.Deserialize(strm);
                }
                MessageBox.Show("File downloaded");
                DialogResult result1 = MessageBox.Show("Do you want to save file?",
                "File saving",
                MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    using (var folderDialog = new FolderBrowserDialog())
                    {
                        if (folderDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (downloaded != null)
                            {
                                downloaded.Save(folderDialog.SelectedPath);
                                MessageBox.Show("File saved");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("File not saved");
                }
            }
            catch(NullReferenceException nullException)
            {
                MessageBox.Show("Please select item you want to download");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    SharedFile file = new SharedFile(fileDialog.FileName);

                    formatter.Serialize(strm, "3");
                    try
                    {
                        try { 
                            formatter.Serialize(strm, file);
                            MessageBox.Show("File uploaded");
                        }
                        catch(Exception exc)
                        {
                            MessageBox.Show("Somethings goes wrong" + exc.Message);
                        }
                        button1_Click(button1, EventArgs.Empty);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Something goes wrong, " + exception.Message);
                    }
                }
            }
        }
    }
}
