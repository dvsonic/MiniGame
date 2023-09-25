using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIBaseController:MonoBehaviour
{

    private void Awake()
    {
        OnUIInit();
    }
    protected virtual void OnUIInit()
    {

    }

    protected UIBaseView CreateView(Type type)
    {
        UIBaseView view = Activator.CreateInstance(type) as UIBaseView;
        var root = UIManager.Instance.UIRoot;
        if (type == typeof(StoryPanelView))
            view.Init(UIManager.Instance.TopLayer, transform);
        else
            view.Init(UIManager.Instance.NormalLayer, transform);
        return view;
    }

    public virtual void OnOpen(params object[] param)
    {

    }

    public virtual void Close()
    {
        OnUIDestory();
    }

    protected virtual void OnUIDestory()
    {

    }
}
