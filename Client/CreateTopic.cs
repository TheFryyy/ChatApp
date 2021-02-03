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
    
    public partial class CreateTopic : Form
    {
        string username;
        TcpClient comm;

        public string Username { get => username; set => username = value; }

        public CreateTopic(TcpClient comm)
        {
            this.comm = comm;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // envoie les informations du topic que l'on veut créer au serveur
        {
            Net.sendMessage(comm.GetStream(), new Communication.Message(MessageType.createTopic,textBox1.Text));
            this.Close();
        }

        public void ChangeUsername(string username)
        {
            this.Username = username;
        }

        private void CreateTopic_Load(object sender, EventArgs e)
        {

        }
    }
}
