using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Communication;

namespace Server
{
    /* cette classe est une base de donnée local contenant:
     * - les utilisateurs
     * - les topics
     * - les utilisateurs connectés
     * et des fonctions pour récupérer ou ajouter des données.
     */
    public static class BDD
    {
        private static List<Topic> listTopics = new List<Topic>();
        private static Hashtable listUsers = new Hashtable();
        private static List<User> connectedUsers = new List<User>();

        public static List<Topic> ListTopics { get => listTopics; set => listTopics = value; }
        public static Hashtable ListUsers { get => listUsers; set => listUsers = value; }
        public static List<User> ConnectedUsers { get => connectedUsers; set => connectedUsers = value; }
        

        public static void addTopic(Topic topic)
        {
            ListTopics.Add(topic);
        }

        public static Topic getTopic(int topicId)
        {
            for(int i=0;i<ListTopics.Count;i++)
            {
                if(ListTopics[i].Id==topicId)
                {
                    return ListTopics[i];
                }
            }
            return null;
        }

        public static string getTopicName(int topicId)
        {
            Topic tpc = getTopic(topicId);
            if (tpc!=null)
                return getTopic(topicId).Name;
            return null;
        }


        public static void addUser(User usr, TcpClient c)
        {
            ListUsers.Add(usr, c);
        }

        public static bool isConnected(int userId)
        {
            foreach(User user in ConnectedUsers)
            {
                if(user.Id == userId)
                {
                    return true;
                }
            }
            return false;
        }

        public static User getUser(int userId)
        {
            if(ListUsers.Count==0)
            {
                return null;
            }
            else
            {
                foreach(User usr in ListUsers.Keys)
                {
                    if (usr.Id == userId)
                    {
                        return usr;
                    }
                }
                return null;
            }
            
        }
        public static User getUser(string username)
        {
            if (ListUsers.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (User usr in ListUsers.Keys)
                {
                    if (usr.Username == username)
                    {
                        return usr;
                    }
                }
                return null;
            }

        }

        public static void connectUser(string username)
        {
            User usr = getUser(username);
            if(!connectedUsers.Contains(usr)) // si l'utilisateur n'est pas déjà connecté, on l'ajoute à la liste des connectés
                connectedUsers.Add(usr);
        }
        public static void disconnectUser(string username)
        {
            User usr = getUser(username);
            if (connectedUsers.Contains(usr)) // si l'utilisateur n'est pas déjà connecté, on l'ajoute à la liste des connectés
            {
                usr.ListTopic.Clear();
                connectedUsers.Remove(usr);
            }
        }

        public static User getConnectedUser(int userId)
        {
            if (connectedUsers.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (User usr in connectedUsers)
                {
                    if (usr.Id == userId)
                    {
                        return usr;
                    }
                }
                return null;
            }
        }


            public static User getConnectedUser(string username)
            {
                if (connectedUsers.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (User usr in connectedUsers)
                    {
                        if (usr.Username == username)
                        {
                            return usr;
                        }
                    }
                    return null;
                }

            }

        public static bool userExist(User user)
        {
            for (int i=0;i<ListUsers.Count;i++)
            {
                if (user == ListUsers[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
