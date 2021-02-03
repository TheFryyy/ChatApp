using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;

namespace Client
{
    class Client
    {
        TcpClient comm = new TcpClient();
        private string hostname;
        private int port;
        private string username = null;

        public Client(string h, int p)
        {
            hostname = h;
            port = p;
        }

        public void start()
        {
            comm = new TcpClient(hostname, port); // TCP connection
            Console.WriteLine("Connection established");

            while(true)
            {
                String beginning = "Welcome to the ChatBox, what do you want to do ?\n 1- sign in\n 2- list topics\n 3- create topic\n 4- connect to topic";
                Console.WriteLine(beginning);
                String choice = Console.ReadLine();

                if (choice == "1")
                {
                    string currentName = this.username;
                    Net.sendRequest(comm.GetStream(), 1);
                    
                    Console.WriteLine("username");
                    string username = Console.ReadLine();
                    Console.WriteLine("password");
                    string password = Console.ReadLine();
                    Net.sendUser(comm.GetStream(), new User(username, password));
                    

                }
                else if (choice == "2")
                {
                    Net.sendRequest(comm.GetStream(), 2);
                    Console.WriteLine("topicName");
                    string topicName = Console.ReadLine();
                    List<Topic> listTopic = null;
                    while(listTopic==null)
                    {
                        listTopic = Net.rcvListTopic(comm.GetStream());
                    }
                    for (int i = 0; i < listTopic.Count; i++)
                    {
                        Console.WriteLine(listTopic[i].Id + "- " +listTopic[i].Name);
                    }
                }
                else if (choice == "3")
                {
                    Net.sendRequest(comm.GetStream(), 3);
                    Console.WriteLine("Topic Name");
                    string topicName = Console.ReadLine();
                    Net.sendTopic(comm.GetStream(), new Topic(topicName));
                }
                else if (choice == "4")
                {
                    
                    Net.sendRequest(comm.GetStream(), 4);
                    //Console.WriteLine("Topic id");
                    //int topicId = Console.Read();
                    //Net.sendTopicId(comm.GetStream(), topicId);
                    //Topic topic = null;
                    //while (topic == null)
                    //{
                    //    topic = Net.rcvTopic(comm.GetStream());
                    //}
                    //Console.WriteLine(topic.ToString());
                    //Console.WriteLine("\n\n 1- comment\n");
                    //string subChoice = Console.ReadLine();
                    //if(subChoice=="1")
                    {
                        string com = "";
                        while(true)
                        {
                            Thread ctThread = new Thread(getMessage);
                            ctThread.Start();
                            Console.Write("Enter your comment\n");
                            string comment = Console.ReadLine();
                            Message msg = new Message(username, comment, 1);
                            Net.sendMsg(comm.GetStream(), msg);
                            //string message = Net.rcvString(comm.GetStream());
                            //if (message != null)
                            //{
                            //    com += message + "\n";
                            //    Console.Clear();
                            //    Console.WriteLine(com);
                            //}
                        }
                        
                    }
                }
            }
            
                        
        }

        public void getMessage()
        {
            string com = "";
            while (true)
            {
                
                if(Net.rcvString(comm.GetStream()) != null)
                com += Net.rcvString(comm.GetStream()) + "\n";
                Console.Clear();
                Console.WriteLine(com);
            }
            
        }

    }
}
