using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventHandler(object sender, UIEvent e);

public interface IEvent
{
    /// The Event key
    object type { get; set; }

    /// An arbitrary data payload
    object data { get; set; }
}
public class UIEvent : EventArgs
{
    public string type;
    public System.Object param;
    public int pointerId = -1;
    public Vector3 position;
    public UIEvent()
    {

    }

    public UIEvent(string type, System.Object param = null)
    {
        this.type = type;
        this.param = param;
    }

    private static EasyPool<UIEvent> pool = new EasyPool<UIEvent>();

    public static UIEvent Get(string type, System.Object param = null, int pointerId = -1, Vector3 position = new Vector3())
    {
        UIEvent evt = pool.Get();
        evt.type = type;
        evt.param = param;
        evt.pointerId = pointerId;
        evt.position = position;
        return evt;
    }

    public static void Remove(UIEvent evt)
    {
        pool.Remove(evt);
    }

    public partial class EventType
    {
        public static string TAG = "TAG";

        public static string START = "START";

        public static string END = "END";

        public static string COMPLETE = "COMPLETE";

        public static string CLICK = "CLICK";

        public static string MOUSE_PRESS = "MOUSE_PRESS";

        public static string MOUSE_DOWN = "MOUSE_DOWN";

        public static string MOUSE_DOWN_D1 = "MOUSE_DOWN_D1";

        public static string MOUSE_OVER = "MOUSE_OVER";

        public static string MOUSE_UP = "MOUSE_UP";

        public static string MOUSE_ENTER = "MOUSE_ENTER";

        public static string MOUSE_OUT = "MOUSE_OUT";

        public static string BECOME_VISIBLE = "BECOME_VISIBLE";

        public static string BECOME_INVISIBLE = "BECOME_INVISIBLE";

        public static string COLLISION_EXIT = "COLLISION_EXIT";

        public static string COLLISION_ENTER = "COLLISION_ENTER";

        public static string BEGIN_DRAG = "BEGIN_DRAG";

        public static string END_DRAG = "END_DRAG";

        public static string ON_DRAG = "ON_DRAG";

        public static string ON_SCROLL = "ON_SCROLL";

        public static string VALUE_CHANGE = "VALUE_CHANGE";

        public static string PRESS_LONG = "PRESS_LONG";

        public static string PRESS_SHORT = "PRESS_SHORT";
    }
}
