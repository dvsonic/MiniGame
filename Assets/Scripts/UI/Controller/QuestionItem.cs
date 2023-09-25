using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionItem : MonoBehaviour
{
    Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    QuestionCfg cfg;
    public void SetValue(QuestionCfg cfg)
    {
        this.cfg = cfg;
        gameObject.transform.localScale = Vector3.one;
        gameObject.GetComponent<RectTransform>().anchoredPosition = cfg.GetAnchoredPos();
    }

    public void OnClick()
    {
        if(cfg != null)
        {
            EventSys.FireEvent("EVENT_AUTO_ASK", cfg.Content);
            GameObject.Destroy(gameObject);
        }
    }
}

