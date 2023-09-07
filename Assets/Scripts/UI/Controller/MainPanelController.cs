using System;
using UnityEngine;
using Message;
using Google.Protobuf;

[UnityEngine.AddComponentMenu("")]
public class MainPanelController : UIBaseController
{
	private MainPanelView m_View;
    private AudioSource m_Audio;

	public static string GetPrefabPath()
	{
		return "Assets/Prefabs/MainPanel.prefab";
	}

	protected override void OnUIInit()
	{
		base.OnUIInit();
		m_View = CreateView(typeof(MainPanelView)) as MainPanelView;
		m_View.BtnBack.onClick.AddListener(OnBack);
		m_View.BtnSend.onClick.AddListener(OnSend);
        m_Audio = gameObject.AddComponent<AudioSource>();

        SocketClient.Instance.RegisterCallback(E_NET_MSG_ID.S2CChatAnswerRes, OnAskRsp);
    }

    bool answered = true;
    private void OnSend()
    {
        if(answered && m_View.Input.text != "")
        {
            C2SChatAskReq req = new C2SChatAskReq();
            req.Type = ChatType.Text;
            req.StrValue = m_View.Input.text;
            SocketClient.Instance.SendCmd(E_NET_MSG_ID.C2SChatAskReq, req);
            Debug.Log("Send:" + req.StrValue);
            this.answered = false;
            m_View.TFAsk.text = req.StrValue;
            m_View.AskContent.SetActive(true);
            m_View.Input.text = "";
            m_View.AnswerContent.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            OnSend();
    }

    private void OnAskRsp(MsgPacket packet)
    {
        var res = packet.PB as S2CChatAnswerRes;
        if (res != null)
        {
            m_View.AnswerContent.SetActive(true);
            m_View.TFAnswer.text = res.StrValue;
            this.PlaySnd(res.SndValue);
            this.answered = true;
        }
    }

    private void OnBack()
    {
		UIManager.Instance.ClosePanel(this);
		UIManager.Instance.OpenPanel<CharacterSelectController>();
    }

    public override void OnOpen(params object[] param)
    {
        base.OnOpen(param);
		string charName = param[0] as string;
		CharacterCfg cfg = TableManager.Instance.GetCharacterByID(charName);
		if(cfg != null)
        {
			m_View.TFCharacter.text = cfg.NickName;
        }
		m_View.AskContent.SetActive(false);
		m_View.AnswerContent.SetActive(false);
    }

    void PlaySnd(ByteString sndValue)
    {
        float[] audioData = new float[sndValue.Length / 2]; // 假设音频为 16 位采样
        byte[] bytes = sndValue.ToByteArray();
        for (int i = 0; i < audioData.Length; i++)
        {
            short sampleValue = BitConverter.ToInt16(bytes, i * 2);
            audioData[i] = sampleValue / 32768f; // 归一化为 -1 到 1 之间的浮点数
        }
        AudioClip audioClip = AudioClip.Create("AudioClip", sndValue.Length, 1, 44100, false);
        audioClip.SetData(audioData, 0);
        m_Audio.clip = audioClip;
        m_Audio.Play();
    }

    protected override void OnUIDestory()
	{
		base.OnUIDestory();
        SocketClient.Instance.UnRegisterCallback(E_NET_MSG_ID.S2CChatAnswerRes, OnAskRsp);
    }
}