using System;
using UnityEngine;
using Message;

[UnityEngine.AddComponentMenu("")]
public class LoginPanelController : UIBaseController
{
	private LoginPanelView m_View;

	public static string GetPrefabPath()
	{
		return "Assets/Prefabs/LoginPanel.prefab";
	}

	protected override void OnUIInit()
	{
		base.OnUIInit();
		m_View = CreateView(typeof(LoginPanelView)) as LoginPanelView;

		m_View.BtnLogin.onClick.AddListener(OnLogin);
		SocketClient.Instance.RegisterCallback(E_NET_MSG_ID.S2CChatLoginRes, OnLoginCB);
		string nick = PlayerPrefs.GetString("LastLogin");
		if (string.IsNullOrEmpty(nick))
			m_View.InputName.text = "";
		else
			m_View.InputName.text = nick;
	}

    private void OnLoginCB(MsgPacket packet)
    {
		S2CChatLoginRes res = packet.PB as S2CChatLoginRes;
        if (res != null)
        {
			CharacterModel.Instance.InitAffinity(res.Profile);
			PlayerPrefs.SetString("LastLogin", m_View.InputName.text);
			int showed = PlayerPrefs.GetInt("ShowStartStory" + m_View.InputName.text, 0);
			if (showed == 0)
			{
				UIManager.Instance.OpenPanel<StoryPanelController>(TableManager.Instance.GetStoryByID(0));
				PlayerPrefs.SetInt("ShowStartStory" + m_View.InputName.text, 1);
			}
			UIManager.Instance.ClosePanel(this);
            UIManager.Instance.OpenPanel<CharacterSelectController>(res.Profile);
        }
    }

    private void OnLogin()
    {
		string ip = m_View.InputIP.text;
#if UNITY_EDITOR
		ip = "10.21.25.115";
#endif
		SocketClient.Instance.ConnectToServer(ip, 38438);
		if(SocketClient.Instance.Connected)
        {
			if (m_View.InputName.text != "")
			{
				C2SChatLoginReq req = new C2SChatLoginReq();
				req.NickName = m_View.InputName.text;
				SocketClient.Instance.SendCmd(E_NET_MSG_ID.C2SChatLoginReq, req);
			}
			else
				UIManager.Instance.ShowFloatingText("请输入用户名!");
        }
    }

    protected override void OnUIDestory()
	{
        m_View.BtnLogin.onClick.RemoveListener(OnLogin);
        SocketClient.Instance.UnRegisterCallback(E_NET_MSG_ID.S2CChatLoginRes, OnLoginCB);
        base.OnUIDestory();
    }
}