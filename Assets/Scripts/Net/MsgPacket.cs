using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Google.Protobuf;

public class MsgPacket
{
    public uint Cmd;
    public int Length;
    public byte[] Data;
    public IMessage PB;
    static MsgFactory factory = MsgFactoryBuilder.CreateFactory();
    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Cmd);
        if (Data != null)
        {
            writer.Write(Length);
            writer.Write(Data);
        }
    }

    public int UnSerialize(BinaryReader reader)
    {
        Cmd = reader.ReadUInt32();
        PB = factory.Create((E_NET_MSG_ID)Cmd);
        Length = reader.ReadInt32();
        if (Length > 0)
        {
            Data = reader.ReadBytes(Length);
            PB.MergeFrom(Data);
        }
        return Length + 4 + 4;
    }
}
