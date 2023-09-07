using System.Collections.Generic;

public class EventSys:Singleton<EventSys>
{

    public delegate void EventHandler(object[] args);
    private Dictionary<object, EventHandler> EventHandlerMap = new Dictionary<object, EventHandler>();

    private Dictionary<object, Dictionary<string, List<EventHandler>>> eventSet;
    private object NullObj = new object();

    private readonly Queue<List<EventHandler>> idleListQueue = new Queue<List<EventHandler>>();
    private readonly Queue<Dictionary<string, List<EventHandler>>> idleDicQueue = new Queue<Dictionary<string, List<EventHandler>>>();

    public EventSys()
    {
        eventSet = new Dictionary<object, Dictionary<string, List<EventHandler>>>();
    }

    private void ListenObjectEvent(object obj, string eventName, EventHandler callback)
    {
        if (obj == null) obj = NullObj;

        Dictionary<string, List<EventHandler>> events;
        if (!eventSet.TryGetValue(obj, out events))
        {
            events = GetIdleDic();
            eventSet[obj] = events;
        }

        List<EventHandler> handlers = null;
        if (!events.TryGetValue(eventName, out handlers))
        {
            handlers = GetIdleList();
            events[eventName] = handlers;
        }
#if UNITY_EDITOR
        foreach (var eventHandler in handlers)
        {
            if (System.Object.ReferenceEquals(eventHandler.Target, callback.Target))
            {
                UnityEngine.Debug.LogError("ListenObjectEvent Repeat");
            }
        }
#endif
        if (!handlers.Contains(callback))
            handlers.Add(callback);
    }

    private void UnListenObjectEvent(object obj, string eventName, EventHandler callback)
    {
        if (obj == null) obj = NullObj;

        Dictionary<string, List<EventHandler>> events;
        if (!eventSet.TryGetValue(obj, out events))
        {
            return;
        }

        List<EventHandler> handlers;
        if (!events.TryGetValue(eventName, out handlers))
        {
            return;
        }
        handlers.Remove(callback);

        if (handlers.Count <= 0)
        {
            if (events.Remove(eventName))
                Back(handlers);
        }

        //删除obj，防止其泄露
        if (events.Count <= 0)
        {
            if (eventSet.Remove(obj))
                Back(events); //回收
        }
    }

    private void FireObjectEvent(object obj, string eventName, object[] args)
    {
        if (obj == null)
            obj = NullObj;

        Dictionary<string, List<EventHandler>> events;
        if (!eventSet.TryGetValue(obj, out events))
        {
            return;
        }

        List<EventHandler> handlers;
        if (!events.TryGetValue(eventName, out handlers))
        {
            return;
        }

        {
            // handlers maybe changed(add or remove) by handle func
            for (int i = handlers.Count - 1; i >= 0 && i < handlers.Count; --i)
            {
                try
                {
                    handlers[i](args);
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError("FireObjectEvent: " + ex.Message + " : " + ex.StackTrace);
                }
            }
        }
    }

    public void ReleaseByObj(object obj)
    {
        if (obj == null)
        {
            return;
        }
        eventSet.Remove(obj);
    }


    public static void ListenEvent(string eventName, EventHandler callback)
    {
        if (EventSys.Instance == null)
        {
            return;
        }
        EventSys.Instance.ListenObjectEvent(null, eventName, callback);
    }

    public static void UnListenEvent(string eventName, EventHandler callback)
    {
        if (EventSys.Instance == null)
        {
            return;
        }

        EventSys.Instance.UnListenObjectEvent(null, eventName, callback);
    }

    public static void FireEvent(string eventName, params object[] args)
    {
        if (EventSys.Instance == null)
        {
            return;
        }

        EventSys.Instance.FireObjectEvent(null, eventName, args);
    }

    public static void FireEvent(string eventName)
    {
        if (EventSys.Instance == null)
        {
            return;
        }

        EventSys.Instance.FireObjectEvent(null, eventName, null);
    }

    /*public static void ListenEvent(object obj, string eventName, EventHandler callback)
    {
        if (EventSys.Instance == null)
        {
            return;
        }
        EventSys.Instance.ListenObjectEvent(obj, eventName, callback);
    }

    public static void ListenEvent(object obj, string eventName, EventHandler callback, object handlerKey)
    {
        if (EventSys.Instance == null)
        {
            return;
        }
        EventSys.Instance.EventHandlerMap[handlerKey] = callback;
        EventSys.Instance.ListenObjectEvent(obj, eventName, callback);
    }

    public static void UnListenEvent(object obj, string eventName, EventHandler callback)
    {
        if (EventSys.Instance == null)
        {
            return;
        }

        EventSys.Instance.UnListenObjectEvent(obj, eventName, callback);
    }*/

    public static void UnListenEventByKey(object obj, string eventName, object handlerKey)
    {
        if (EventSys.Instance == null)
        {
            return;
        }

        EventHandler callback = EventSys.Instance.EventHandlerMap[handlerKey];
        EventSys.Instance.EventHandlerMap.Remove(handlerKey);
        EventSys.Instance.UnListenObjectEvent(obj, eventName, callback);
    }

    /*public static void FireEvent(object obj, string eventName, params object[] args)
    {
        if (EventSys.Instance == null)
        {
            return;
        }

        EventSys.Instance.FireObjectEvent(obj, eventName, args);
    }

    public static void FireEvent(object obj, string eventName)
    {
        if (EventSys.Instance == null)
        {
            return;
        }

        EventSys.Instance.FireObjectEvent(obj, eventName, null);
    }*/

    private List<EventHandler> GetIdleList()
    {
        if (idleListQueue.Count > 0)
        {
            var it = idleListQueue.Dequeue();
            it.Clear();
            return it;
        }
        return new List<EventHandler>();
    }

    private void Back(List<EventHandler> list)
    {
        if (list == null)
        {
            return;
        }
        list.Clear();
        idleListQueue.Enqueue(list);
    }

    private Dictionary<string, List<EventHandler>> GetIdleDic()
    {
        if (idleDicQueue.Count > 0)
        {
            var it = idleDicQueue.Dequeue();
            foreach (var list in it.Values)
            {
                Back(list);
            }
            it.Clear();
            return it;
        }
        return new Dictionary<string, List<EventHandler>>();
    }

    private void Back(Dictionary<string, List<EventHandler>> dic)
    {
        if (dic == null)
        {
            return;
        }
        foreach (var list in dic.Values)
        {
            Back(list);
        }
        dic.Clear();
        idleDicQueue.Enqueue(dic);
    }
}
