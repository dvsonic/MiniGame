using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[UnityEngine.AddComponentMenu("")]
public class PhotoPanelController : UIBaseController
{
	private PhotoPanelView m_View;

	public static string GetPrefabPath()
	{
		return "Assets/Prefabs/PhotoPanel.prefab";
	}

	protected override void OnUIInit()
	{
		base.OnUIInit();
		m_View = CreateView(typeof(PhotoPanelView)) as PhotoPanelView;
		m_View.MaskImg.gameObject.SetActive(false);
		m_View.BtnBack.onClick.AddListener(OnBack);
		m_View.BtnMask.onClick.AddListener(OnClickMask);
		EventSys.ListenEvent("EVENT_CLICK_PHOTO", ShowMask);
	}

    private void ShowMask(object[] args)
    {
		string photo = args[0] as string;
		m_View.MaskImg.gameObject.SetActive(true);
		m_View.MaskImg.sprite = ResourceManager.Instance.Load<Sprite>("Assets/Res/Image/Photo/" + photo + ".png");
    }

    public override void OnOpen(params object[] param)
    {
        base.OnOpen(param);
		string curChar = param[0] as string;
		var affinity = CharacterModel.Instance.GetAffinity(curChar);
		List<TableBase> lst = TableManager.Instance.GetTable(TableManager.TableEnum.Affinity);
		for(int i=0;i<lst.Count;i++)
        {
			AffinityCfg cfg = lst[i] as AffinityCfg;
			if(cfg.ID == curChar)
            {
				GameObject obj = GameObject.Instantiate<GameObject>(m_View.ImgPreview, m_View.Content);
				var item = obj.AddComponent<PhotoItem>();
				item.SetValue(cfg.Photo, cfg.Affinity <= affinity.Item1);
			}
        }
	}

	void OnBack()
    {
		UIManager.Instance.ClosePanel(this);
    }
	void OnClickMask()
    {
		m_View.MaskImg.gameObject.SetActive(false);
    }

    protected override void OnUIDestory()
	{
		EventSys.UnListenEvent("EVENT_CLICK_PHOTO", ShowMask);
		base.OnUIDestory();
	}
}