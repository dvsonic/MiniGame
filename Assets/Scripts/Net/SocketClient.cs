using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Google.Protobuf;
using System.IO;
using Message;
using System.Collections.Generic;

public class SocketClient:Singleton<SocketClient>
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer;
    MemoryStream sendMemoryStream = new MemoryStream();
    public Action<MsgPacket> OnRecv;
    Dictionary<E_NET_MSG_ID, List<Action<MsgPacket>>> callBackDic = new Dictionary<E_NET_MSG_ID, List<Action<MsgPacket>>>();

    public bool Connected;
    float lastBeat;

    public void RegisterCallback(E_NET_MSG_ID cmdID, Action<MsgPacket> callback)
    {
        if(!callBackDic.ContainsKey(cmdID))
        {
            callBackDic[cmdID] = new List<Action<MsgPacket>>();
        }
        callBackDic[cmdID].Add(callback);
    }
    public void UnRegisterCallback(E_NET_MSG_ID cmdID,Action<MsgPacket> callback)
    {
        if(callBackDic.ContainsKey(cmdID))
        {
            var list = callBackDic[cmdID];
            list.Remove(callback);
        }
    }

    public void Update()
    {
        if (stream != null && stream.DataAvailable)
        {
            ReceiveData();
        }
    }

    void HeartBeat()
    {
        /*if (Connected)
        {
            C2SHeartBeatReq req = new C2SHeartBeatReq();
            SendCmd(E_NET_MSG_ID.C2SChatHeartBeatReq, req);
        }*/
    }

    public void ConnectToServer(string ipAddress, int port)
    {
        try
        {
            client = new TcpClient();
            client.Connect(ipAddress, port);

            stream = client.GetStream();
            receiveBuffer = new byte[client.ReceiveBufferSize];
            Connected = true;
            Debug.Log("Connected to server");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to connect to server: " + e.Message);
        }
    }

    private void ReceiveData()
    {
        BinaryReader reader = new BinaryReader(stream);
        MsgPacket packet = new MsgPacket();
        packet.UnSerialize(reader);
        if (callBackDic.ContainsKey((E_NET_MSG_ID)packet.Cmd))
        {
            var cbList = callBackDic[(E_NET_MSG_ID)packet.Cmd];
            for(int i=0;i<cbList.Count;i++)
            {
                cbList[i](packet);
            }
        }
    }

    public void SendCmd(E_NET_MSG_ID cmd, IMessage msg)
    {
        if (client != null && client.Connected)
        {
            var bytes = msg.ToByteArray();
            MsgPacket packet = new MsgPacket();
            packet.Cmd = (uint)cmd;
            packet.Length = bytes.Length;
            packet.Data = bytes;
            sendMemoryStream.Seek(0L, SeekOrigin.Begin);
            BinaryWriter writer = new BinaryWriter(sendMemoryStream);
            packet.Serialize(writer);
            var totalBytes = sendMemoryStream.GetBuffer();
            stream.Write(totalBytes, 0, (int)sendMemoryStream.Length);
            //Debugger.Log("SendCmd;" + cmd + "|" + bytes.Length+"|"+totalBytes.Length+"|"+sendMemoryStream.Length);
        }
    }

    public void Close()
    {
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
        Connected = false;
    }
}
