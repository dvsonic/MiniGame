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
        view.Init(UIManager.Instance.UIRoot, transform);
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
