using System;
using UnityEngine;
using Message;

[UnityEngine.AddComponentMenu("")]
public class CharacterSelectController : UIBaseController
{
	private CharacterSelectView m_View;
	string curChar;
	public static string GetPrefabPath()
	{
		return "Assets/Prefabs/CharacterSelect.prefab";
	}

	protected override void OnUIInit()
	{
		base.OnUIInit();
		m_View = CreateView(typeof(CharacterSelectView)) as CharacterSelectView;

		m_View.BtnEnter.onClick.AddListener(OnEnterChat);
		EventSys.ListenEvent("EVENT_SELECT_CHARACTER", OnCharacterSelect);

	}

    private void OnEnterChat()
    {
		C2SChangeCharacterReq req = new C2SChangeCharacterReq();
		req.CharName = curChar;
		SocketClient.Instance.SendCmd(E_NET_MSG_ID.C2SChangeCharacterReq, req);
		UIManager.Instance.ClosePanel(this);
		UIManager.Instance.OpenPanel<MainPanelController>(curChar);
    }

    private void OnCharacterSelect(object[] args)
    {
		curChar = args[0] as string;
		m_View.BG.sprite = ResourceManager.Instance.Load<Sprite>("Assets/Res/Image/Background/" + curChar + ".png");
		var charCfg = TableManager.Instance.GetCharacterByID(curChar);
		if (charCfg != null)
		{
			m_View.TFName.text = charCfg.NickName;
			m_View.TFDesc.text = charCfg.Desc;
			m_View.TFProfile1.text = charCfg.Profile1;
			m_View.TFProfile2.text = charCfg.Profile2;
			m_View.TFProfile3.text = charCfg.Profile3;
			m_View.TFProfile4.text = charCfg.Profile4;
            var affinity = CharacterModel.Instance.GetAffinity(curChar);
            m_View.TFFitness.text = affinity.Item1 + "/" + affinity.Item2;
        }

    }

    public override void OnOpen(params object[] param)
    {
        base.OnOpen(param);
		var allChar = TableManager.Instance.GetTable(TableManager.TableEnum.Character);
		CharacterItem defaultItem = null;
		for(int i =0;i< allChar.Count; i++)
        {
			var obj = GameObject.Instantiate<GameObject>(m_View.CharacterItem);
			obj.transform.SetParent(m_View.Content);
			obj.transform.localScale = Vector3.one;
			var charInfo = allChar[i] as CharacterCfg;
			var item = obj.AddComponent<CharacterItem>();
			item.SetValue(charInfo);
			if (defaultItem == null)
				defaultItem = item;
		}
		if (defaultItem != null)
			defaultItem.OnSelect();

    }

    protected override void OnUIDestory()
	{
        m_View.BtnEnter.onClick.RemoveListener(OnEnterChat);
        EventSys.UnListenEvent("EVENT_SELECT_CHARACTER", OnCharacterSelect);
        base.OnUIDestory();
	}
}