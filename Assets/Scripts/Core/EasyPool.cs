using System.Collections.Generic;
using UnityEngine;
public class EasyPool<T> where T : new()
{
    private readonly Stack<T> _unused = new Stack<T>();
    private readonly List<T> _removeDelayList = new List<T>();
    private int lastRemoveDelayFrame = 0;

    public T Get()
    {
        T a;
        if (_unused.Count == 0)
        {
            a = new T();
        }
        else
        {
            a = _unused.Pop();
        }

        return a;
    }

    public void Remove(T a)
    {
        _unused.Push(a);
    }

    public void RemoveDelay(T a)
    {
        _removeDelayList.Add(a);
        lastRemoveDelayFrame = Time.frameCount;
    }

    public void DoRemoveDelays()
    {
        if (Time.frameCount - lastRemoveDelayFrame > 5)
        {
            foreach (var v in _removeDelayList)
            {
                Remove(v);
            }
            _removeDelayList.Clear();
        }


    }
}