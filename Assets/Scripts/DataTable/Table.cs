using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

public class TableBase
{
    public bool Load(string[] title,string content)
    {
        string[] props = content.Split(",");
        if (title.Length != props.Length)
            return false;
        Type curType = this.GetType();
        for(int i=0;i<title.Length;i++)
        {
            FieldInfo field = curType.GetField(title[i]);
            if (field != null)
            {
                if (field.FieldType == typeof(int))
                {
                    if (props[i] == "")
                        field.SetValue(this, 0);
                    else
                        field.SetValue(this, int.Parse(props[i]));
                }
                else if (field.FieldType == typeof(float))
                {
                    if (props[i] == "")
                        field.SetValue(this, 0);
                    else
                        field.SetValue(this, float.Parse(props[i]));
                }
                else
                {
                    field.SetValue(this, props[i]);
                }
            }
        }
        return true;
    }
}
public class CharacterCfg:TableBase
{
    public string ID;
    public string NickName;
    public int MaxAffinity;
    public string Desc;
    public string Profile1;
    public string Profile2;
    public string Profile3;
    public string Profile4;
}

public class AffinityCfg:TableBase
{
    public string ID;
    public int Level;
    public int Affinity;
    public string Scene;
    public string Photo;
    public string Option;
    public int Story;

    public string[] GetOptionAry()
    {
        return Option.Split(";");
    }
}

public class SentimentCfg : TableBase
{
    public int ID;
    public int Value;
}

public class StoryCfg : TableBase
{
    public int ID;
    public string Content;
    public List<string> GetStoryList()
    {
        if (string.IsNullOrEmpty(Content))
            return new List<string>();
        else
            return new List<string>(Content.Split(";"));
    }
}