using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoItem : MonoBehaviour
{
    Image icon;
    Button btnImage;
    GameObject lockIcon;
    private void Awake()
    {
        icon = GetComponent<Image>();
        btnImage = transform.Find("BtnImage").GetComponent<Button>();
        btnImage.onClick.AddListener(OnClick);
        lockIcon = transform.Find("Lock").gameObject;
    }
    string photo;
    bool visible;
    public void SetValue(string photo,bool visible)
    {
        this.photo = photo;
        this.visible = visible;
        lockIcon.SetActive(!visible);
        icon.sprite = ResourceManager.Instance.Load<Sprite>("Assets/Res/Image/Photo/" + photo + ".png");

    }

    public void OnClick()
    {
        if(this.visible)
            EventSys.FireEvent("EVENT_CLICK_PHOTO", this.photo);
    }
}

