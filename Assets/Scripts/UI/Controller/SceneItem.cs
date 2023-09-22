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
    int affinity;
    public void SetValue(string scene, int affinity)
    {
        if (img == null)
            Awake();
        this.scene = scene;
        this.affinity = affinity;
        if(img != null)
            img.sprite = ResourceManager.Instance.Load<Sprite>("Assets/Res/Image/Scene/" + scene + ".png");
        if (tf != null)
            tf.text = this.affinity.ToString();
    }

    int curAffinity;
    public void SetCurAffinity(int affinity)
    {
        curAffinity = affinity;
        if(MaskObj != null)
            this.MaskObj.SetActive(curAffinity < affinity);
    }

    public void OnClick()
    {
            EventSys.FireEvent("EVENT_CLICK_SCENE", this.scene);
    }
}

