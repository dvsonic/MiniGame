using System;
using System.Collections.Generic;
using System.Linq;
using Message;
public class CharacterModel:Singleton<CharacterModel>
{
    public Dictionary<string, CharacterInfo> allDic = new Dictionary<string, CharacterInfo>();
    public int SelectLevel;
    public int curAffinity;
    public bool IsInChatper;
    
    public void InitAffinity(UserProfile profile)
    {
        for(int i=0;i<profile.AllCharacter.Count;i++)
        {
            var item = profile.AllCharacter[i];
            allDic[item.CharName] = item;
        }
    }
    public Tuple<int,int> GetAffinity(string id)
    {
        int curValue = 0;
        int maxValue = 0;
        if(IsInChatper)
        {
            curValue = curAffinity;
            maxValue = TableManager.Instance.GetMaxAffinity(id, SelectLevel);
        }
        else
        {
            if(allDic.ContainsKey(id))
            {
                var info = allDic[id];
                curValue = info.CharAffinity;
                maxValue = TableManager.Instance.GetMaxAffinity(id, info.CharLevel);
            }
        }

        return Tuple.Create<int, int>(curValue, maxValue);
    }

    public int GetMaxLevel(string id)
    {
        if (allDic.ContainsKey(id))
        {
            return allDic[id].CharLevel;
        }
        else
            return 1;
    }

    public void UpdateCharacter(string id,int curLevel,int curAffinity)
    {
        if(allDic.ContainsKey(id))
        {
            var info = allDic[id];
            this.curAffinity = curAffinity;
            if (info.CharLevel == curLevel)
                info.CharAffinity = curAffinity;
            if (info.CharLevel < curLevel)
            {
                info.CharLevel = curLevel;
                UIManager.Instance.ShowFloatingText("解锁章节:" + curLevel);
            }
            EventSys.FireEvent("EVENT_AFFINITY_CHANGE");
            var cfg = TableManager.Instance.GetAffinity(id, curLevel, curAffinity);
            if (!string.IsNullOrEmpty(cfg.Option))
            {
                EventSys.FireEvent("EVENT_AFFINITY_EVENT", cfg);
            }

        }
    }
}

