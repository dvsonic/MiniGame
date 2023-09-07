
//这个是自动生成的代码
using System;
using System.Collections.Generic;


    public enum E_NET_MSG_ID : uint
    {
        None = 0,

		#region Ai
		AI = ((uint)Message.NetMessage.Types.EProtocol.Types.Proto.Ai)<<16,
		C2SChatAskReq = AI + ((uint)Message.AI.Types.Proto.C2SchatAskReq),
		S2CChatAnswerRes = AI + ((uint)Message.AI.Types.Proto.S2CchatAnswerRes),
		S2CChatAskNotify = AI + ((uint)Message.AI.Types.Proto.S2CchatAskNotify),
		S2CChatAnswerNotify = AI + ((uint)Message.AI.Types.Proto.S2CchatAnswerNotify),
		C2SChatHeartBeatReq = AI + ((uint)Message.AI.Types.Proto.C2SchatHeartBeatReq),
		S2CChatHeartBeatRes = AI + ((uint)Message.AI.Types.Proto.S2CchatHeartBeatRes),
		C2SChatLoginReq = AI + ((uint)Message.AI.Types.Proto.C2SchatLoginReq),
		S2CChatLoginRes = AI + ((uint)Message.AI.Types.Proto.S2CchatLoginRes),
		C2SChangeCharacterReq = AI + ((uint)Message.AI.Types.Proto.C2SchangeCharacterReq),
		S2CChangeCharacterRes = AI + ((uint)Message.AI.Types.Proto.S2CchangeCharacterRes),
		#endregion



    }


public class MsgFactory
{
    public const int C_CAP = 200;
    public Dictionary<E_NET_MSG_ID, Func<E_NET_MSG_ID, Google.Protobuf.IMessage>> _map;
    public Dictionary<Type, E_NET_MSG_ID> _type_2_id;

    public MsgFactory(int cap)
    {
        _map = new Dictionary<E_NET_MSG_ID, Func<E_NET_MSG_ID, Google.Protobuf.IMessage>>(cap);
        _type_2_id = new Dictionary<Type, E_NET_MSG_ID>(cap);
    }

    public MsgFactory() : this(C_CAP)
    {
    }

    public void Add(E_NET_MSG_ID cmd, Func<E_NET_MSG_ID, Google.Protobuf.IMessage> func)
    {
        _map[cmd] = func;
    }

    public void Add(E_NET_MSG_ID cmd, Type t, Func<E_NET_MSG_ID, Google.Protobuf.IMessage> func)
    {
        _map[cmd] = func;
        _type_2_id[t] = cmd;
    }

    public MsgFactory Add<T>(E_NET_MSG_ID id) where T : class, Google.Protobuf.IMessage, new()
    {
        _map[id] = (msg_id) =>
        {
            return new T();
        };
        _type_2_id[typeof(T)] = id;
        return this;
    }

    public bool TryGetMsgId(Type t, out E_NET_MSG_ID msg_id)
    {
        if (t == null)
        {
            msg_id = default;
            return false;
        }
        return _type_2_id.TryGetValue(t, out msg_id);
    }

    public Google.Protobuf.IMessage Create(E_NET_MSG_ID net_msg_key)
    {
        _map.TryGetValue(net_msg_key, out var func);
        if (func == null)
        {
            return null;
        }
        return func(net_msg_key);
    }
}

    public static class MsgFactoryBuilder
    {
        public static MsgFactory CreateFactory()
        {
			MsgFactory ret = new MsgFactory(10);

			ret.Add<Message.C2SChatAskReq>(E_NET_MSG_ID.C2SChatAskReq);
			ret.Add<Message.S2CChatAnswerRes>(E_NET_MSG_ID.S2CChatAnswerRes);
			ret.Add<Message.S2CChatAskNotify>(E_NET_MSG_ID.S2CChatAskNotify);
			ret.Add<Message.S2CChatAnswerNotify>(E_NET_MSG_ID.S2CChatAnswerNotify);
			ret.Add<Message.C2SChatHeartBeatReq>(E_NET_MSG_ID.C2SChatHeartBeatReq);
			ret.Add<Message.S2CChatHeartBeatRes>(E_NET_MSG_ID.S2CChatHeartBeatRes);
			ret.Add<Message.C2SChatLoginReq>(E_NET_MSG_ID.C2SChatLoginReq);
			ret.Add<Message.S2CChatLoginRes>(E_NET_MSG_ID.S2CChatLoginRes);
			ret.Add<Message.C2SChangeCharacterReq>(E_NET_MSG_ID.C2SChangeCharacterReq);
			ret.Add<Message.S2CChangeCharacterRes>(E_NET_MSG_ID.S2CChangeCharacterRes);

            return ret;
        }
    }

