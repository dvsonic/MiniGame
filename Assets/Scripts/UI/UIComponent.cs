using UnityEngine;
using UnityEngine.EventSystems;
public class UIComponent : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, IPointerClickHandler
{
    public bool PassEvent2Down = false;

    public event EventHandler OnMOUSE_OVER;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnMOUSE_OVER != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.MOUSE_OVER);
            OnMOUSE_OVER(this.gameObject, e);
            UIEvent.Remove(e);
        }
    }

    public event EventHandler OnMOUSE_DOWN;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (OnMOUSE_DOWN != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.MOUSE_DOWN, eventData);
            OnMOUSE_DOWN(this.gameObject, e);
            UIEvent.Remove(e);
        }
        else if (PassEvent2Down)
        {
            PassEvent(eventData, ExecuteEvents.pointerDownHandler);
        }
    }

    public event EventHandler OnMOUSE_UP;

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (OnMOUSE_UP != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.MOUSE_UP, eventData);
            OnMOUSE_UP(this.gameObject, e);
            UIEvent.Remove(e);
        }
        else if (PassEvent2Down)
        {
            PassEvent(eventData, ExecuteEvents.pointerUpHandler);
        }
    }

    public event EventHandler OnMOUSE_OUT;

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnMOUSE_OUT != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.MOUSE_OUT);
            OnMOUSE_OUT(this.gameObject, e);
            UIEvent.Remove(e);
        }
    }

    public event EventHandler OnBEGIN_DRAG;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBEGIN_DRAG != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.BEGIN_DRAG, eventData);
            OnBEGIN_DRAG(this.gameObject, e);
            UIEvent.Remove(e);
        }
        else if (PassEvent2Down)
        {
            PassEvent(eventData, ExecuteEvents.beginDragHandler);
        }
    }

    public event EventHandler OnEND_DRAG;

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEND_DRAG != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.END_DRAG, eventData);
            OnEND_DRAG(this.gameObject, e);
            UIEvent.Remove(e);
        }
        else if (PassEvent2Down)
        {
            PassEvent(eventData, ExecuteEvents.endDragHandler);
        }
    }

    public event EventHandler On_DRAG;

    public void OnDrag(PointerEventData eventData)
    {
        if (On_DRAG != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.ON_DRAG, eventData);
            On_DRAG(this.gameObject, e);
            UIEvent.Remove(e);
        }
        else if (PassEvent2Down)
        {
            PassEvent(eventData, ExecuteEvents.dragHandler);
        }
    }

    public event EventHandler On_SCROLL;

    public void OnScroll(PointerEventData eventData)
    {
        if (On_SCROLL != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.ON_SCROLL, eventData);
            On_SCROLL(this.gameObject, e);
            UIEvent.Remove(e);
        }
    }

    public event EventHandler On_CLICK;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (On_CLICK != null)
        {
            UIEvent e = UIEvent.Get(UIEvent.EventType.CLICK, eventData);
            On_CLICK(this.gameObject, e);
            UIEvent.Remove(e);
        }
    }

    GameObject screenResponse;
    public void SetScreenComponent(GameObject t)
    {
        this.screenResponse = t;
    }

    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
    where T : IEventSystemHandler
    {
        if (!PassEvent2Down || screenResponse == null)
            return;
        GameObject nextGo = ExecuteEvents.GetEventHandler<T>(screenResponse);
        //Debug.Log(current.name + "  " + nextGo.name);
        ExecuteEvents.Execute(nextGo, data, function);
    }

    public void PassBeginDrag(PointerEventData data)
    {
        PassEvent(data, ExecuteEvents.beginDragHandler);
    }

    public void PassDrag(PointerEventData data)
    {
        PassEvent(data, ExecuteEvents.dragHandler);
    }

    public void PassEndDrag(PointerEventData data)
    {
        PassEvent(data, ExecuteEvents.endDragHandler);
    }
}
