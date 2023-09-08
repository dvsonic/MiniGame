using System;
using System.Collections.Generic;
using System.Linq;
using Message;
public class CharacterModel:Singleton<CharacterModel>
{
    public Dictionary<string, int> CharAffinity = new Dictionary<string, int>();
    
    public void InitAffinity(UserProfile profile)
    {
        for(int i=0;i<profile.AllCharacter.Count;i++)
        {
            var item = profile.AllCharacter[i];
            CharAffinity[item.CharName] = item.CharAffinity;
        }
    }
    public Tuple<int,int> GetAffinity(string id)
    {
        int curValue = 0;
        int maxValue = 0;
        if (CharAffinity.ContainsKey(id))
            curValue = CharAffinity[id];
        var cfg = TableManager.Instance.GetCharacterByID(id);
        if (cfg != null)
            maxValue = cfg.MaxAffinity;
        return Tuple.Create<int, int>(curValue, maxValue);
    }

    public void UpdateAffinityBySenti(string id,int senti)
    {
        var affinity = GetAffinity(id);
        var curValue = Math.Max(0, Math.Min(affinity.Item2, affinity.Item1 + senti));
        CharAffinity[id] = curValue;
        EventSys.FireEvent("EVENT_AFFINITY_CHANGE");

        var lst = TableManager.Instance.GetTable(TableManager.TableEnum.Affinity);
        if(lst != null)
        {
            for(int i=0;i<lst.Count;i++)
            {
                var cfg = lst[i] as AffinityCfg;
                if (cfg.ID == id && cfg.Affinity == curValue)
                {
                    if (!string.IsNullOrEmpty(cfg.Option))
                    {
                        EventSys.FireEvent("EVENT_AFFINITY_EVENT", cfg);
                        break;
                    }
                }
            }
        }
    }
}

