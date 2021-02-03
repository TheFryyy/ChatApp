using Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class Home : Form
    {
        TcpClient comm = new TcpClient();
        string hostname;
        string username;
        int port;
        
        Signin signin;
        Login login;
        ConnectTopic connectTopic;
        CreateTopic createTopic;
        PrivateChat mp;

        public string Username { get => username; set => username = value; }
        public ConnectTopic ConnectTopic { get => connectTopic; set => connectTopic = value; }
        public CreateTopic CreateTopic { get => createTopic; set => createTopic = value; }

        public Home(string hostname, int port)
        {
            InitializeComponent();
            this.hostname = hostname;
            this.port = port;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            signin = new Signin(comm);
            signin.Show();
        }
        

        private void Home_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(Closing); // ajout d'une fonction à l'event de fermeture de fenêtre

            comm = new TcpClient(hostname, port); // connection TCP au serveur
            
            button1.Text = "create account";
            button2.Text = "login";
            button3.Text = "list topics";
            button4.Text = "create topic";
            button5.Text = "private message";

            new Thread(waitForMsg).Start(); // Thread en attente de messages de la part du serveur
        }

        private void button2_Click(object sender, EventArgs e)
        {
            login = new Login(comm, this);
            login.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(this.username!=null)
            {
                ConnectTopic = new ConnectTopic(comm);
                ConnectTopic.Show();
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(this.username!=null)
            {
                CreateTopic = new CreateTopic(comm);
                CreateTopic.Show();
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(this.username!=null)
            {
                mp = new PrivateChat(comm, username);
                mp.Show();
            }
            
        }
       

        private void waitForMsg() // On attend un message de la part du serveur et agit en fonction de son type
        {
            while (true)
            {
                if(this.username!=null && connectTopic!=null) //on met à jour le nom d'utilisateur pour notre fenetre de connexion à un topic
                {
                    connectTopic.Username = this.username;
                }
                if (this.username != null && createTopic != null)//on met à jour le nom d'utilisateur pour notre fenetre de création d'un topic
                {
                    createTopic.Username = this.username;
                }

                Communication.Message message = null;

                while (message == null) // On attend de recevoir un message
                {
                    message = Net.rcvMessage(comm.GetStream()); //type de requete
                }
                switch (message.Type) // redistribution des messages dans les bonnes fenetres 
                {
                    case MessageType.signin:
                        signin.rcvMsg(message);
                        break;
                        
                    case MessageType.login:
                        login.rcvMsg(message);
                        break;
                        
                    case MessageType.getTopics: 
                        connectTopic.rcvMsg(message);
                        break;
                    case MessageType.connectTopic:
                        connectTopic.rcvMsg(message);
                        break;

                    case MessageType.listUsers:
                        mp.rcvMsg(message);
                        break;
                    case MessageType.broadcast:
                        string[] broadcastMsg = message.Msg.Split('*');
                        foreach(TopicChat tc in connectTopic.ListTopic) // on recherche la bonne fenetre du bon topic pour lui envoyer le broadcast.
                        {
                            if(Int32.Parse(broadcastMsg[0])==tc.TopicId)
                            {
                                tc.getMessage(broadcastMsg[1]);
                            }
                        }
                        break;
                    case MessageType.privateMsg:
                        mp.rcvMsg(message);
                        break;

                    default:
                        break;
                }


            }
        }

        private void button6_Click(object sender, EventArgs e) // déconnecte l'utilisateur et ferme les fenetre ouvertes
        {
            if (this.username != null)
            {
                Net.sendMessage(comm.GetStream(), new Communication.Message(MessageType.logout, this.Username));
                this.username = null;
                if(signin!=null)
                    this.signin.Close();
                if (login != null)
                    this.login.Close();
                if (connectTopic != null)
                    this.connectTopic.Close();
                if (createTopic != null)
                    this.createTopic.Close();
                if (mp != null)
                    this.mp.Close();
            }
        }
        private void Closing(Object sender, FormClosingEventArgs e) // déconnecte l'utilisateur si on ferme la fenêtre
        {
            Net.sendMessage(comm.GetStream(), new Communication.Message(MessageType.logout, this.Username));
        }
    }
}
