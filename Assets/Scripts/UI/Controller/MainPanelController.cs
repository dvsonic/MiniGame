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
    bool updateAffinity;

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

        m_View.BtnExpand.onClick.AddListener(OnSwitch);

        writer = m_View.TFAnswer.gameObject.AddComponent<TypeWriter>();

        SocketClient.Instance.RegisterCallback(E_NET_MSG_ID.S2CChatAnswerRes, OnAskRsp);
        SocketClient.Instance.RegisterCallback(E_NET_MSG_ID.S2CEnterChatperRes, OnEnterRsp);

        EventSys.ListenEvent("EVENT_AFFINITY_CHANGE", OnAffinityChange);
        EventSys.ListenEvent("EVENT_AFFINITY_EVENT", OnAffinityEvent);
        EventSys.ListenEvent("EVENT_CLICK_SCENE", OnChangeScene);
        EventSys.ListenEvent("EVENT_AUTO_ASK", OnAutoAsk);
    }

    private void OnSwitch()
    {
        C2SEnterChapterReq req = new C2SEnterChapterReq();
        req.Chapter = tempSelectLevel;
        SocketClient.Instance.SendCmd(E_NET_MSG_ID.C2SEnterChapterReq, req);
    }

    void OnEnterRsp(MsgPacket packet)
    {
        var res = packet.PB as S2CEnterChatperRes;
        if (res != null)
        {
            this.SwitchMode(true,res.Chapter);
            CharacterModel.Instance.UpdateCharacter(curChar, res.Chapter, res.CurAffinity);
        }
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
        RefreshPhotos(affinity.Item1);
        if (!CharacterModel.Instance.IsInChatper)
        {
            RefreshSceneList();
        }
        else
        {
            var cfg = TableManager.Instance.GetAffinity(curChar, CharacterModel.Instance.SelectLevel, CharacterModel.Instance.GetAffinity(curChar).Item1);
            if (cfg != null)
            {
                if (!string.IsNullOrEmpty(cfg.Option))
                {
                    EventSys.FireEvent("EVENT_AFFINITY_EVENT", cfg);
                }
                if (cfg.Story != 0)
                {
                    UIManager.Instance.OpenPanel<StoryPanelController>(cfg.Story);
                }
                if (!string.IsNullOrEmpty(cfg.QuestionList))
                {
                    GenerateQuestion(cfg.GetQuestionAry());
                }
                ResourceManager.Instance.LoadSprite2Image(m_View.BG, "Assets/Res/Image/Scene/" + cfg.Scene + ".png");
            }
        }
    }

    void RefreshPhotos(int curAffinity)
    {
        int totalCnt = 0;
        int unlockCnt = 0;
        List<TableBase> lst = TableManager.Instance.GetTable(TableManager.TableEnum.Affinity);
        for (int i = 0; i < lst.Count; i++)
        {
            var cfg = lst[i] as AffinityCfg;
            if (cfg.ID == curChar && !string.IsNullOrEmpty(cfg.Photo))
            {
                totalCnt++;
                if (CharacterModel.Instance.GetMaxLevel(curChar)>cfg.Level)
                    unlockCnt++;
                else if(CharacterModel.Instance.GetMaxLevel(curChar) == cfg.Level)
                {
                    if (cfg.Affinity <= CharacterModel.Instance.GetAffinity(curChar).Item1)
                        unlockCnt++;
                }
            }
        }
        m_View.TFPhoto.text = unlockCnt + "/" + totalCnt;
    }

    void InitSceneList()
    {
        for(int i=m_View.Content.childCount-1;i>=0;i--)
            GameObject.Destroy(m_View.Content.GetChild(i));
        List<int> sceneList = new List<int>();
        List<TableBase> lst = TableManager.Instance.GetTable(TableManager.TableEnum.Affinity);
        for (int i = 0; i < lst.Count; i++)
        {
            var cfg = lst[i] as AffinityCfg;
            if (cfg.ID == curChar)
            {
                if (sceneList.IndexOf(cfg.Level) < 0)
                {
                    sceneList.Add(cfg.Level);
                    var obj = GameObject.Instantiate<GameObject>(m_View.SceneItem);
                    obj.transform.SetParent(m_View.Content);
                    obj.transform.localScale = Vector3.one;
                    var item = obj.AddComponent<SceneItem>();
                    item.SetValue(cfg.Scene,cfg.Level);
                }
            }
        }
    }

    void RefreshSceneList()
    {
        for(int i=0;i<m_View.Content.childCount;i++)
        {
            var item = m_View.Content.GetChild(i).GetComponent<SceneItem>();
            item.SetCurLevel(CharacterModel.Instance.GetMaxLevel(curChar));
        }
    }

    int tempSelectLevel;
    void OnChangeScene(object[] args)
    {
        tempSelectLevel = (int)args[0];
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
        req.UpdateAffinity = this.updateAffinity;
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
                if(updateAffinity)
                    CharacterModel.Instance.UpdateCharacter(curChar,res.CurLevel,res.CurAffinity);
                else
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
        if (updateAffinity)
        {
            SwitchMode(false, CharacterModel.Instance.GetMaxLevel(curChar));
        }
        else
        {
            UIManager.Instance.ClosePanel(this);
            UIManager.Instance.OpenPanel<CharacterSelectController>();
        }
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
        SwitchMode(false,CharacterModel.Instance.GetMaxLevel(curChar));
        InitSelectLevel();
    }

    void InitSelectLevel()
    {
        for (int i = m_View.Content.childCount-1; i >=0; i--)
        {
            var item = m_View.Content.GetChild(i).GetComponent<SceneItem>();
            if(item.IsUnLock())
            {
                item.OnClick();
                return;
            }
        }
    }

    void SwitchMode(bool updateAffinity,int selectLevel)
    {
        this.updateAffinity = updateAffinity;
        CharacterModel.Instance.SelectLevel = selectLevel;
        m_View.Avatar.gameObject.SetActive(!updateAffinity);
        m_View.SceneSelect.gameObject.SetActive(!updateAffinity);
        m_View.BtnExpand.gameObject.SetActive(!updateAffinity);
        m_View.AskContent.SetActive(false);
        m_View.AnswerContent.SetActive(false);
        m_View.ShortCut.SetActive(false);
        m_View.Input.gameObject.SetActive(!updateAffinity);
        m_View.BtnSend.gameObject.SetActive(!updateAffinity);
        if (updateAffinity)
        {
            CharacterModel.Instance.IsInChatper = true;
            var cfg = TableManager.Instance.GetAffinity(curChar, CharacterModel.Instance.SelectLevel, CharacterModel.Instance.GetAffinity(curChar).Item1);
            if (cfg != null)
                ResourceManager.Instance.LoadSprite2Image(m_View.BG, "Assets/Res/Image/Scene/" + cfg.Scene + ".png");
        }
        else
        {

            for(int i= m_View.QuestionContainer.childCount-1;i>=0;i--)
                GameObject.Destroy(m_View.QuestionContainer.GetChild(i).gameObject);
            CharacterModel.Instance.IsInChatper = false;
            CharacterModel.Instance.SelectLevel = CharacterModel.Instance.GetMaxLevel(curChar);
            ResourceManager.Instance.LoadSprite2Image(m_View.BG, "Assets/Res/Image/Background/" + curChar + "_empty.png");
        }
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

    void GenerateQuestion(string[] ary)
    {
        for (int i = m_View.QuestionContainer.childCount - 1; i >= 0; i--)
            GameObject.Destroy(m_View.QuestionContainer.GetChild(i).gameObject);
        for (int i=0;i<ary.Length;i++)
        {

            var cfg = TableManager.Instance.GetQuestionByID(int.Parse(ary[i]));
            if(cfg != null)
            {
                var obj = GameObject.Instantiate<GameObject>(m_View.QuestionItem);
                obj.transform.SetParent(m_View.QuestionContainer);
                var item = obj.AddComponent<QuestionItem>();
                item.SetValue(cfg);
            }
        }
    }

    void OnAutoAsk(params object[] param)
    {
        this.Ask(param[0] as string);
    }

    protected override void OnUIDestory()
	{
        m_View.ShortCut1.onClick.RemoveListener(OnShortCut1);
        m_View.ShortCut2.onClick.RemoveListener(OnShortCut2);
        m_View.ShortCut3.onClick.RemoveListener(OnShortCut3);
        EventSys.UnListenEvent("EVENT_AFFINITY_CHANGE", OnAffinityChange);
        EventSys.UnListenEvent("EVENT_AFFINITY_EVENT", OnAffinityEvent);
        EventSys.UnListenEvent("EVENT_CLICK_SCENE", OnChangeScene);
        EventSys.UnListenEvent("EVENT_AUTO_ASK", OnAutoAsk);
        base.OnUIDestory();
        SocketClient.Instance.UnRegisterCallback(E_NET_MSG_ID.S2CChatAnswerRes, OnAskRsp);
        SocketClient.Instance.UnRegisterCallback(E_NET_MSG_ID.S2CEnterChatperRes, OnEnterRsp);
    }
}