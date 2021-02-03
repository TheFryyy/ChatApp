using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    // ensemble des types de messages pouvant être envoyé.
    public enum MessageType { signin, login, logout, createTopic, connectTopic, sendMessage, getTopics, username, rcvMessage, broadcast, privateMsg, listUsers, disconnectTopic };
    [Serializable]
    public class Message
    {
        //Un message est composé d'un type de message et d'un texte qui sera traité en fonction de son type.
        // pour envoyer plusieurs info on sépare par un élément et on utilise la fonction Split().
        private MessageType type;
        private string msg;

        public Message(MessageType type, string msg)
        {
            this.Type = type;
            this.Msg = msg;
        }

        
        public string Msg { get => msg; set => msg = value; }
        public MessageType Type { get => type; set => type = value; }
    }
    
}
