using System;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    public enum TableEnum
    {
        Global,
        Character,
    }
    private Dictionary<TableEnum, List<TableBase>> allDic = new Dictionary<TableEnum, List<TableBase>>();
    public void Init()
    {
        ResourceManager.Instance.LoadAsync<TextAsset>("Assets/Config/Character.csv", (obj) =>
        {
            TextAsset asset = obj as TextAsset;
            if (asset != null && !string.IsNullOrEmpty(asset.text))
            {
                OnTableLoaded(TableEnum.Character, typeof(CharacterCfg), asset.text);
            }
        });
    }

    void OnTableLoaded(TableEnum key, Type t, string content)
    {
        if (allDic.ContainsKey(key))
            return;
        allDic[key] = new List<TableBase>();
        string[] lines = content.Split("\r\n");
        string[] title = lines[0].Split("\t");
        for (int i = 1; i < lines.Length; i++)
        {
            var item = System.Activator.CreateInstance(t);
            TableBase tableItem = item as TableBase;
            if (tableItem.Load(title, lines[i]))
            {
                allDic[key].Add(tableItem);
            }
        }
    }

    public List<TableBase> GetTable(TableEnum t)
    {
        if (allDic.ContainsKey(t))
            return allDic[t];
        else
            return null;
    }

    public CharacterCfg GetCharacterByID(string id)
    {
        List<TableBase> lst = GetTable(TableEnum.Character);
        if(lst != null)
        {
            for(int i=0;i<lst.Count;i++)
            {
                if ((lst[i] as CharacterCfg).ID == id)
                    return lst[i] as CharacterCfg;
            }
        }
        return null;
    }
}