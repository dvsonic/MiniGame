using System;
using UnityEngine;
using Message;
using Google.Protobuf;
using TMPro;
using System.Collections.Generic;

[UnityEngine.AddComponentMenu("")]
public class MainPanelController : UIBaseController
{
	private MainPanelView m_View;
    private AudioSource m_Audio;

	public static string GetPrefabPath()
	{
		return "Assets/Prefabs/MainPanel.prefab";
	}

    TypeWriter writer;
	protected override void OnUIInit()
	{
		base.OnUIInit();
		m_View = CreateView(typeof(MainPanelView)) as MainPanelView;
		m_View.BtnBack.onClick.AddListener(OnBack);
		m_View.BtnSend.onClick.AddListener(OnSend);
        m_View.BtnPhoto.onClick.AddListener(OnPhoto);
        m_Audio = gameObject.AddComponent<AudioSource>();
        m_View.ShortCut.SetActive(false);
        m_View.ShortCut1.onClick.AddListener(OnShortCut1);
        m_View.ShortCut2.onClick.AddListener(OnShortCut2);
        m_View.ShortCut3.onClick.AddListener(OnShortCut3);

        m_View.SceneSelect.SetActive(false);
        m_View.BtnExpand.onClick.AddListener(OnExpand);

        writer = m_View.TFAnswer.gameObject.AddComponent<TypeWriter>();

        SocketClient.Instance.RegisterCallback(E_NET_MSG_ID.S2CChatAnswerRes, OnAskRsp);

        EventSys.ListenEvent("EVENT_AFFINITY_CHANGE", OnAffinityChange);
        EventSys.ListenEvent("EVENT_AFFINITY_EVENT", OnAffinityEvent);
        EventSys.ListenEvent("EVENT_CLICK_SCENE", OnChangeScene);
    }

    private void OnExpand()
    {
        m_View.SceneSelect.SetActive(!m_View.SceneSelect.activeSelf);
    }
    private void OnPhoto()
    {
        UIManager.Instance.OpenPanel<PhotoPanelController>(this.curChar);
    }

    private void OnShortCut1()
    {
        this.Ask(m_View.ShortCut1.GetComponentInChildren<TextMeshProUGUI>().text);
        m_View.ShortCut.SetActive(false);
    }
    private void OnShortCut2()
    {
        this.Ask(m_View.ShortCut2.GetComponentInChildren<TextMeshProUGUI>().text);
        m_View.ShortCut.SetActive(false);
    }
    private void OnShortCut3()
    {
        this.Ask(m_View.ShortCut3.GetComponentInChildren<TextMeshProUGUI>().text);
        m_View.ShortCut.SetActive(false);
    }

    private void OnAffinityChange(object[] args)
    {
        var affinity = CharacterModel.Instance.GetAffinity(curChar);
        m_View.TFAffinity.text = affinity.Item1 + "/" + affinity.Item2;
        var cfg = TableManager.Instance.GetAffinity(curChar, affinity.Item1);
        if (cfg != null)
            ResourceManager.Instance.LoadSprite2Image(m_View.BG,"Assets/Res/Image/Scene/" + cfg.Scene + ".png");
        RefreshPhotos(affinity.Item1);
        RefreshSceneList(affinity.Item1);
    }

    void RefreshPhotos(int curAffinity)
    {
        int totalCnt = 0;
        int unlockCnt = 0;
        List<TableBase> lst = TableManager.Instance.GetTable(TableManager.TableEnum.Affinity);
        for (int i = 0; i < lst.Count; i++)
        {
            var cfg = lst[i] as AffinityCfg;
            if (cfg.ID == curChar)
            {
                totalCnt++;
                if (curAffinity >= cfg.Affinity)
                    unlockCnt++;
            }
        }
        m_View.TFPhoto.text = unlockCnt + "/" + totalCnt;
    }

    void InitSceneList()
    {
        while (m_View.Content.childCount > 0)
            GameObject.Destroy(m_View.Content.GetChild(0));
        List<string> sceneList = new List<string>();
        List<TableBase> lst = TableManager.Instance.GetTable(TableManager.TableEnum.Affinity);
        for (int i = 0; i < lst.Count; i++)
        {
            var cfg = lst[i] as AffinityCfg;
            if (cfg.ID == curChar)
            {
                if (sceneList.IndexOf(cfg.Scene) < 0)
                {
                    sceneList.Add(cfg.Scene);
                    var obj = GameObject.Instantiate<GameObject>(m_View.SceneItem);
                    obj.transform.SetParent(m_View.Content);
                    var item = obj.AddComponent<SceneItem>();
                    item.SetValue(cfg.Scene, cfg.Affinity);
                }
            }
        }
    }

    void RefreshSceneList(int affinity)
    {
        for(int i=0;i<m_View.Content.childCount;i++)
        {
            var item = m_View.Content.GetChild(i).GetComponent<SceneItem>();
            item.SetCurAffinity(affinity);
        }
    }

    void OnChangeScene(object[] args)
    {
        string scene = args[0] as string;
        ResourceManager.Instance.LoadSprite2Image(m_View.BG, "Assets/Res/Image/Scene/" + scene + ".png");
    }

    private void OnAffinityEvent(object[] args)
    {
        AffinityCfg cfg = args[0] as AffinityCfg;
        if(cfg != null)
        {
            m_View.ShortCut.SetActive(true);
            string[] options = cfg.GetOptionAry();
            m_View.ShortCut1.gameObject.SetActive(options.Length>0);
            m_View.ShortCut2.gameObject.SetActive(options.Length > 1);
            m_View.ShortCut3.gameObject.SetActive(options.Length > 2);

            if(options.Length>0)
                m_View.ShortCut1.GetComponentInChildren<TextMeshProUGUI>().text = options[0];
            if (options.Length > 1)
                m_View.ShortCut2.GetComponentInChildren<TextMeshProUGUI>().text = options[1];
            if (options.Length > 2)
                m_View.ShortCut3.GetComponentInChildren<TextMeshProUGUI>().text = options[2];

        }
    }

    bool answered = true;
    private void OnSend()
    {
        if(answered && m_View.Input.text != "")
        {
            Ask(m_View.Input.text);
            m_View.Input.text = "";
        }
    }
    void Ask(string content)
    {
        C2SChatAskReq req = new C2SChatAskReq();
        req.Type = ChatType.Text;
        req.StrValue = content;
        SocketClient.Instance.SendCmd(E_NET_MSG_ID.C2SChatAskReq, req);
        this.answered = false;
        m_View.TFAsk.text = req.StrValue;
        m_View.AskContent.SetActive(true);
        m_View.AnswerContent.SetActive(false);
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
            if (res.StrValue != "")
            {
                m_View.AnswerContent.SetActive(true);
                if (writer != null)
                    writer.SetText(res.StrValue);
                else
                    m_View.TFAnswer.text = res.StrValue;
                CharacterModel.Instance.UpdateAffinityBySenti(curChar, res.SentiValue);
                m_View.Avatar.PlayBySenti(res.SentiValue);
                this.answered = true;
            }
            if(res.SndValue != null && res.SndValue.Length>0)
            {
                this.PlaySnd(res.SndValue);
            }
        }
    }

    private void OnBack()
    {
		UIManager.Instance.ClosePanel(this);
		UIManager.Instance.OpenPanel<CharacterSelectController>();
    }

    string curChar;
    public override void OnOpen(params object[] param)
    {
        base.OnOpen(param);
        curChar = param[0] as string;
		CharacterCfg cfg = TableManager.Instance.GetCharacterByID(curChar);
		if(cfg != null)
        {
			m_View.TFCharacter.text = cfg.NickName;
        }
		m_View.AskContent.SetActive(false);
		m_View.AnswerContent.SetActive(false);
        InitSceneList();
        OnAffinityChange(null);
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
        m_View.ShortCut1.onClick.RemoveListener(OnShortCut1);
        m_View.ShortCut2.onClick.RemoveListener(OnShortCut2);
        m_View.ShortCut3.onClick.RemoveListener(OnShortCut3);
        EventSys.UnListenEvent("EVENT_AFFINITY_CHANGE", OnAffinityChange);
        EventSys.UnListenEvent("EVENT_AFFINITY_EVENT", OnAffinityEvent);
        EventSys.UnListenEvent("EVENT_CLICK_SCENE", OnChangeScene);
        base.OnUIDestory();
        SocketClient.Instance.UnRegisterCallback(E_NET_MSG_ID.S2CChatAnswerRes, OnAskRsp);
    }
}