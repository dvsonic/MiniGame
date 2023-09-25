using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

[UnityEngine.AddComponentMenu("")]
public class StoryPanelController : UIBaseController
{
	private StoryPanelView m_View;

	public static string GetPrefabPath()
	{
		return "Assets/Prefabs/StoryPanel.prefab";
	}

	protected override void OnUIInit()
	{
		base.OnUIInit();
		m_View = CreateView(typeof(StoryPanelView)) as StoryPanelView;
		m_View.BtnNext.onClick.AddListener(OnNext);
	}

	List<string> storyList;
	int curIdx;
    public override void OnOpen(params object[] param)
    {
		storyList = param[0] as List<string>;
		if (storyList == null)
			return;
		curIdx = 0;
		m_View.TFContent.text = "";
		GetComponent<Image>().DOFade(1, 2).From(0).OnComplete(() => OnNext());
	}

    void OnNext()
    {
		if(curIdx >0)
        {
			var seq = DOTween.Sequence();
			seq.Append(m_View.TFContent.DOFade(0, 1).OnComplete(()=>SwitchText()));
			seq.Append(m_View.TFContent.DOFade(1, 1));
			seq.PlayForward();
        }
		else
        {
			SwitchText();
			m_View.TFContent.DOFade(1, 1).From(0);
		}
    }

	void SwitchText()
    {
		if(storyList != null && curIdx<storyList.Count)
        {
			m_View.TFContent.text = storyList[curIdx];
			curIdx++;
        }
        else
        {
			UIManager.Instance.ClosePanel(this);
        }
    }

	protected override void OnUIDestory()
	{
		base.OnUIDestory();
	}
}