using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneItem : MonoBehaviour
{
    Image img;
    Button btn;
    TMPro.TextMeshProUGUI tf;
    GameObject MaskObj;
    private void Awake()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        tf = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        MaskObj = transform.Find("Mask").gameObject;
    }
    string scene;
    int level;
    public void SetValue(string scene,int level)
    {
        if (img == null)
            Awake();
        this.scene = scene;
        this.level = level;
        if(img != null)
            img.sprite = ResourceManager.Instance.Load<Sprite>("Assets/Res/Image/Scene/" + scene + ".png");
        if (tf != null)
            tf.text = level.ToString();
    }

    int curLevel;
    public void SetCurLevel(int curLevel)
    {
        this.curLevel = curLevel;
        if(MaskObj != null)
            this.MaskObj.SetActive(curLevel < level);
    }

    public bool IsUnLock()
    {
        return curLevel >= level;
    }

    public void OnClick()
    {
        if (IsUnLock())
            EventSys.FireEvent("EVENT_CLICK_SCENE", this.level);
        else
            UIManager.Instance.ShowFloatingText("当前章节暂未解锁!");
    }
}

