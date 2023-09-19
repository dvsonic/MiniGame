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
        lockObj.SetActive(false);
        btnSelect = transform.Find("BtnSelect").GetComponent<Button>();
        btnSelect.onClick.AddListener(OnSelect);
    }
    CharacterCfg cfg;
    public void SetValue(CharacterCfg charInfo)
    {
        cfg = charInfo;
        icon.sprite = ResourceManager.Instance.Load<Sprite>("Assets/Res/Image/Icon/" + cfg.ID + ".png");
    }

    public void OnSelect()
    {
        EventSys.FireEvent("EVENT_SELECT_CHARACTER", cfg.ID);
    }
}

