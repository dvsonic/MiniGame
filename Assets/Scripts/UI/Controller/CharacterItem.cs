using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterItem:MonoBehaviour
{
    Image icon;
    GameObject lockObj;
    Button btnSelect;
    private void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        lockObj = transform.Find("Lock").gameObject;
        btnSelect = transform.Find("BtnSelect").GetComponent<Button>();
        btnSelect.onClick.AddListener(OnSelect);
    }
    CharacterCfg cfg;
    public void SetValue(CharacterCfg charInfo)
    {
        cfg = charInfo;
    }

    public void OnSelect()
    {
        EventSys.FireEvent("EVENT_SELECT_CHARACTER", cfg.ID);
    }
}

