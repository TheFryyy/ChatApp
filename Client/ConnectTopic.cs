using Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class ConnectTopic : Form
    {
        List<TopicChat> listTopic = new List<TopicChat>();
        TcpClient comm;
        string username;
        public string Username { get => username; set => username = value; }
        public List<TopicChat> ListTopic { get => listTopic; set => listTopic = value; }

        public ConnectTopic(TcpClient comm)
        {
            this.comm = comm;
            InitializeComponent();
        }

        private void ConnectTopic_Load(object sender, EventArgs e)
        {
            Net.sendMessage(comm.GetStream(), new Communication.Message(MessageType.getTopics, null)); // on dit au serveur qu'on veut récupérer la liste des topics
        }

        public void rcvMsg(Communication.Message message)
        {
            if(message.Type==MessageType.connectTopic) // On récupère le nom du topic pour le donner au form que l'on a créé pour ce topic
            {
                string[] idAndName = message.Msg.Split(' ');
                foreach(TopicChat tc in ListTopic)
                {
                    if(tc.TopicId == Int32.Parse(idAndName[0]))
                    {
                        tc.setTopicName(idAndName[1]);
                    }
                }
            }
            else if(message.Type==MessageType.getTopics) //on récupère et affiche la liste des topics
            {
                int count = 1;
                string text = "List Topic: " + Environment.NewLine;
                string[] listTopics = message.Msg.Split(' ');
                label1.Text = "topic ID";
                textBox2.Enabled = false;
                foreach (string topic in listTopics)
                {
                    if (topic != "")
                    {
                        text += count + "- " + topic + Environment.NewLine;
                        count++;
                    }

                }
                textBox2.Text = text;
            }
            
        }

        public void ChangeUsername(string username)
        {
            this.Username = username;
        }

        bool listTopicContain(int id)
        {
            foreach(TopicChat tc in listTopic)
            {
                if (tc.TopicId == id)
                    return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e) // On se connecte au topic demandé
        {
            if(!listTopicContain(Int32.Parse(textBox1.Text)))
            {
                TopicChat TopicInfo = new TopicChat(comm, username, Int32.Parse(textBox1.Text));
                listTopic.Add(TopicInfo); // ajout à la liste des topics sur lesquels on est connecté
                TopicInfo.Show();
                Net.sendMessage(comm.GetStream(), new Communication.Message(MessageType.connectTopic, Username + " " + textBox1.Text)); // précise au serveur le topic sur lequel l'utilisateur se connecte
            }
            
        }

        private void button2_Click(object sender, EventArgs e) // Rafraichit la liste des topics
        {
            Net.sendMessage(comm.GetStream(), new Communication.Message(MessageType.getTopics, null));
        }
    }
}
