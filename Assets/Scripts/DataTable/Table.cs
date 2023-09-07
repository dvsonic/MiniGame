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
        string[] props = content.Split("\t");
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
                    field.SetValue(this, int.Parse(props[i]));
                }
                else if (field.FieldType == typeof(float))
                {
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