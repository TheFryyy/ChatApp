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
    public partial class Login : Form
    {
        TcpClient comm;
        Home home;
        public Login(TcpClient comm,Home home)
        {
            InitializeComponent();
            this.comm = comm;
            this.home = home;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (home.Username != null) // check si on est connecté
                label3.Text = "you are already connected !";
            else
                label3.Text = "";

            label1.Text = "username";
            label2.Text = "password";
            button1.Text = "login";
        }

        // on check si l'utilisateur existe déjà ou est déjà connecté (c'est le serveur qui nous le dit)
        public void rcvMsg(Communication.Message message) 
        {
            if (message.Msg == "ok")
            {
                home.Username = textBox1.Text;
                this.Close();
            }
            else if (message.Msg == "no")
            {
                label3.Text = "This user does not exist !";
            }
            else if (message.Msg == "connected")
            {
                label3.Text = "This user is already connected !";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (home.Username == null) // si on est pas déjà connecté, on demande à se connecter et on attend la réponse de serveur (rcvMsg())
            {
                Net.sendMessage(comm.GetStream(), new Communication.Message(MessageType.login, textBox1.Text + " " + textBox2.Text));
            }
            
        }
    }
}
