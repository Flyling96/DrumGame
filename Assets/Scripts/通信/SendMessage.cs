using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

public class SendMessage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Socket sendsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    IPEndPoint ipendpiont = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 81);

    public void Send(string str)
    {
        try
        {
            sendsocket.Connect(ipendpiont);
            sendsocket.Send(Encoding.UTF8.GetBytes(str));
        }
        catch(Exception e)
        {
            print(e.ToString());
        }
    }


    public string Recieve()
    {
        try
        {
            byte[] ServerData = new byte[4096];
            int receiveNumber = sendsocket.Receive(ServerData);
            string str = Encoding.UTF8.GetString(ServerData, 0, receiveNumber);
            return str;
        }
        catch(Exception e)
        {
            string str = "接收过程出错";
            print(e.ToString());
            return str;
        }
    }
}
