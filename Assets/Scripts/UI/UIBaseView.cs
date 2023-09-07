using UnityEngine;
using UnityEngine.UI;

public class UIBaseView
{
    public void Init(Transform rootTrans, Transform holder)
    {
        if (holder == null)
        {
            return;
        }
        holder.SetParent(rootTrans, false);
        var rect = holder.GetComponent<RectTransform>();
        rect.anchoredPosition3D = Vector3.zero;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        OnInit(holder);
    }
    protected virtual void OnInit(Transform holder)
    {
    }
}