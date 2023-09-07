using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Object = UnityEngine.Object;

public class ProcessUIEditor : EditorWindow
{
    public static ProcessUIEditor Instance;
    public Rect _windowRect;
    public Rect _contentRect;
    public float _leftWidth = 200f;
    public float _topHeight = 60f;
    public float _titleHeight = 35f;
    public float _heightLeft;
    public bool IsSpectatorMode;
    public bool IsShowExtra;
    public bool IsNewPrefab;

    UIItemVariable m_CurItemVar;
    UIItemReference m_CurItemRef;
    GameObject m_CurRoot;

    public UnityEngine.Object LastSelectObject;

    const string ViewCodePath = "Assets/Scripts/UI/View";
    public bool IsPrefabMode;
    public bool IsRunningAll = false;
    public Dictionary<string, string> VarNameDict = new Dictionary<string, string>();
    public int PosA;
    public int PosB;
    public string ChangeListNumber;

    public string ControlCodePath = "Assets/Scripts/UI/Controller";
    public string PrefabPath;


    [MenuItem("Tool/GenerateUICode")]
    public static void Open()
    {
        if (Instance != null)
        {
            Instance.Show();
            return;
        }
        Instance = EditorWindow.GetWindow<ProcessUIEditor>();
        Instance.titleContent = new GUIContent("ProcessUI Editor");

        Instance.Show();
    }
    public GameObject GetRootGameObject(GameObject obj)
    {
        Transform result = null;
        result = obj.transform;
        while (result.parent != null)
        {
            if (result.GetComponent<UIItemReference>())
            {
                break;
            }
            if (result.parent.name == "UIRoot (Environment)")
            {
                break;
            }

            result = result.parent;
        }
        return result.gameObject;
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            return;
        }

        var selectObject = Selection.objects;
        if (selectObject.Length == 0 || IsRunningAll)
        {
            return;
        }

        if (Instance == null)
        {
            Instance = this;
        }

        if (LastSelectObject != selectObject[0])
        {
            LastSelectObject = selectObject[0];

            IsPrefabMode = UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(selectObject[0]);

            if (IsPrefabMode)
            {
                m_CurRoot = GetRootGameObject(selectObject[0] as GameObject);
                if (m_CurRoot.name == "UIRoot")
                {
                    m_CurRoot = null;
                }

            }
            else
            {
                m_CurRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(selectObject[0]);
                //m_CurRoot = selectObject[0] as GameObject;
            }
        }

        if (m_CurRoot != null)
        {
            PrefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(m_CurRoot);
            var curSelect = (selectObject[0] as GameObject);

            m_CurItemRef = m_CurRoot.GetComponent<UIItemReference>();
            bool isExist = m_CurItemRef != null && m_CurItemRef.IsExistItem(curSelect.transform);

            if (isExist)
            {
                m_CurItemVar = m_CurItemRef.GetItem(curSelect.transform);
            }
            else
            {
                _heightLeft = 20f;
                if (GUI.Button(GetGUIRect(20, 150, 18), "Add UIVariable"))
                {
                    if (m_CurItemRef == null)
                    {
                        m_CurItemRef = m_CurRoot.AddComponent<UIItemReference>();
                    }

                    m_CurItemVar = m_CurItemRef.GetItem((selectObject[0] as GameObject).transform);
                    m_CurItemVar.Components = new Object[] { selectObject[0] as GameObject };
                }
                _heightLeft += 10;

                if (m_CurItemRef != null)
                {
                    _heightLeft += 10;
                    if (GUI.Button(GetGUIRect(20, 150, 18), "ExportView"))
                    {
                        GenCSCodeFile();
                    }

                    _heightLeft += 10;
                    PrefabPath = EditorGUI.TextField(GetGUIRect(20, 300, 18, false), PrefabPath);

                    _heightLeft += 30;
                    if (GUI.Button(GetGUIRect(20, 300, 18), "Remove All Child Reference"))
                    {
                        RemoveAllChildReference(selectObject[0] as GameObject);
                    }

                    _heightLeft += 40;

                    IsNewPrefab = EditorGUI.Toggle(GetGUIRect(20, 260, 18), "IsNewPrefab", IsNewPrefab);

                    if (IsNewPrefab)
                    {
                        _heightLeft += 20;
                        ControlCodePath = EditorGUI.TextField(GetGUIRect(20, 300, 18, false), ControlCodePath);

                        _heightLeft += 20;

                        if (GUI.Button(GetGUIRect(20, 150, 18), "Export New Controller"))
                        {
                            GenControlCode();
                        }
                    }

                    _heightLeft += 20;

                }
                return;
            }


            _windowRect = new Rect(0, 0, position.width, position.height);
            _contentRect = new Rect(_leftWidth, _topHeight, position.width, position.height);

            EditorGUI.LabelField(new Rect(5, 20, _windowRect.xMax, _titleHeight), new GUIContent(m_CurRoot.name + " -> " + selectObject[0].name));

            GUI.Box(new Rect(5, Instance._topHeight, Instance._leftWidth, Instance._windowRect.height), "");

            System.Type type = m_CurItemVar.GetType();
            FieldInfo[] fileds = type.GetFields();

            _heightLeft = _topHeight;

            for (int j = 0; j < fileds.Length - 1; j++)
            {
                FieldInfo filed = fileds[j];
                _heightLeft += 5;

                Vector2 size = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).label.CalcSize(new GUIContent(filed.Name));
                EditorGUI.LabelField(GetGUIRect(10, size.x, size.y, size.x > 100), filed.Name);
                Rect rect = GetGUIRect(60, 150, 18);
                DrawNormalValue(filed, rect, m_CurItemVar);
            }

            _heightLeft += 10;
            if (GUI.Button(GetGUIRect(20, 150, 18), "+"))
            {
                GUIUtility.keyboardControl = 0;
                if (m_CurItemVar.Components == null)
                {
                    m_CurItemVar.Components = new Component[0];
                }
                Array.Resize<string>(ref m_CurItemVar.VarNameArray, m_CurItemVar.Components.Length + 1);
                Array.Resize<Object>(ref m_CurItemVar.Components, m_CurItemVar.Components.Length + 1);
            }

            _heightLeft += 10;
            if (GUI.Button(GetGUIRect(20, 150, 18), "-"))
            {
                GUIUtility.keyboardControl = 0;
                Array.Resize<string>(ref m_CurItemVar.VarNameArray, m_CurItemVar.Components.Length - 1);
                Array.Resize<Object>(ref m_CurItemVar.Components, m_CurItemVar.Components.Length - 1);
            }

            _heightLeft += 60;
            if (GUI.Button(GetGUIRect(20, 150, 18), "Reset"))
            {
                GUIUtility.keyboardControl = 0;
                Array.Resize<string>(ref m_CurItemVar.VarNameArray, 0);
                Array.Resize<Object>(ref m_CurItemVar.Components, 0);
            }

            _heightLeft += 10;
            if (GUI.Button(GetGUIRect(20, 150, 18), "Remove This UIVariable"))
            {
                GUIUtility.keyboardControl = 0;
                m_CurItemRef.RemoveItem(m_CurItemVar);
                m_CurItemVar = null;
            }


            _heightLeft += 10;
            if (GUI.Button(GetGUIRect(20, 150, 18), "Export View"))
            {
                GenCSCodeFile();
            }

            _heightLeft += 30;
            if (GUI.Button(GetGUIRect(15, 180, 18), "Remove All Child Reference"))
            {
                RemoveAllChildReference(selectObject[0] as GameObject);
            }

            GUI.Box(new Rect(240, Instance._topHeight, Instance._leftWidth, Instance._windowRect.height), "");
            _heightLeft = _topHeight + 22;

            for (int j = fileds.Length - 1; j < fileds.Length; j++)
            {
                FieldInfo filed = fileds[j];
                _heightLeft += 5;

                Vector2 size = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).label.CalcSize(new GUIContent(filed.Name));
                EditorGUI.LabelField(GetGUIRect(250, size.x, size.y, size.x > 100), filed.Name);
                Rect rect = GetGUIRect(310, 150, 18);
                DrawNormalValue(filed, rect, m_CurItemVar);
            }

            _heightLeft += 10;

            EditorGUI.LabelField(GetGUIRect(250, 200, 18), "Quick Fill Component Value ↓");

            var comps = curSelect.GetComponents<Component>();

            foreach (var comp in comps)
            {
                _heightLeft += 10;
                var compType = comp.GetType().Name;

                if (GUI.Button(GetGUIRect(260, 150, 18), compType))
                {
                    string name = GUI.GetNameOfFocusedControl();
                    if (name.Contains("Component"))
                    {
                        var index = name.Substring(10);
                        switch (compType)
                        {
                            case "Transform":
                                m_CurItemVar.Components[int.Parse(index)] = m_CurItemVar.Target;
                                break;
                            default:
                                m_CurItemVar.Components[int.Parse(index)] = m_CurItemVar.Target.GetComponent(compType);
                                break;
                        }
                    }
                    GUIUtility.keyboardControl = 0;
                }
            }

            _heightLeft += 10;

            if (GUI.Button(GetGUIRect(260, 150, 18), "GameObject"))
            {
                string name = GUI.GetNameOfFocusedControl();
                if (name.Contains("Component"))
                {
                    var index = name.Substring(10);
                    m_CurItemVar.Components[int.Parse(index)] = m_CurItemVar.Target.gameObject;
                }
                GUIUtility.keyboardControl = 0;
            }
        }
    }

    Rect GetGUIRect(float leftPos, float width, float height, bool addHeight = true)
    {
        Rect rect = new Rect(leftPos, _heightLeft, width, height);
        if (addHeight)
        {
            _heightLeft += height;
        }
        return rect;
    }


    public string GetVarNameByDict(string key, int compIndex, UIItemVariable item)
    {
        var result = "";
        if (VarNameDict.Count > 0)
        {
            VarNameDict.TryGetValue(key + compIndex + item.Target.GetHashCode(), out result);
        }

        if (string.IsNullOrEmpty(result))
        {
            if (item.VarNameArray.Length > compIndex)
            {
                if (string.IsNullOrEmpty(item.VarNameArray[compIndex]))
                {
                    return item.Target.name;
                }
                else
                {
                    return item.VarNameArray[compIndex];
                }
            }
        }

        if (string.IsNullOrEmpty(result))
        {
            if (item.Components.Length == 1)
            {
                return item.Target.name;
            }
            else
            {
                return key;
            }
        }

        return result;
    }

    private void DrawNormalValue(System.Reflection.FieldInfo filed, Rect rect, object o)
    {
        switch (filed.FieldType.ToString())
        {
            case "UnityEngine.Object[]":
                Object[] objArray = filed.GetValue(o) as Object[];
                if (objArray == null)
                {
                    return;
                }

                List<string> compArray = new List<string>();
                foreach (var comp in objArray)
                {
                    if (comp == null)
                    {
                        compArray.Add("");
                    }
                    else
                    {
                        compArray.Add(comp.GetType().Name);
                    }
                }
                for (int j = 0; j < compArray.Count; j++)
                {
                    GUI.SetNextControlName(filed.Name + j);
                    rect = GetGUIRect(10, 180, 18, false);
                    compArray[j] = EditorGUI.TextField(rect, (string)compArray[j]);
                    GetGUIRect(175, 20, 18);
                    _heightLeft += 2;
                }
                //filed.SetValue(o, objArray);

                break;
            case "UnityEngine.Transform":
                Transform target = (Transform)filed.GetValue(o);
                EditorGUI.LabelField(rect, target == null ? "Notice: Is NULL!" : target.name);
                break;
            case "System.String":
                var strRect = GetGUIRect(10, rect.width, 20, true);
                filed.SetValue(o, EditorGUI.TextField(strRect, (string)filed.GetValue(o)));
                break;
            case "System.String[]":
                string[] array = filed.GetValue(o) as string[];
                if (array == null)
                    array = new string[] { };
                for (int j = 0; j < array.Length; j++)
                {
                    GUI.SetNextControlName(filed.Name + j);
                    rect = GetGUIRect(250, 180, 18, false);
                    array[j] = EditorGUI.TextField(rect, (string)array[j]);
                    GetGUIRect(175, 20, 18);
                    _heightLeft += 2;
                }
                filed.SetValue(o, array);
                break;

            default:
                if (filed.FieldType.BaseType.ToString() == "System.Enum")
                {
                    filed.SetValue(o, EditorGUI.EnumPopup(rect, (System.Enum)filed.GetValue(o)));
                }
                else
                {
                    EditorGUI.LabelField(rect, "Not Deal Object.");
                }
                break;
        }
    }

    private void GenCSCodeFile()
    {
        GenViewCodeFile();
    }

    private void GenViewCodeFile()
    {
        GameObject prefab = m_CurItemRef.gameObject;
        var newPrefab = m_CurItemRef.gameObject;
        if (!IsPrefabMode)
        {
            prefab = PrefabUtility.GetCorrespondingObjectFromSource(m_CurItemRef.gameObject);
            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
            bool isSucceed = false;
            PrefabUtility.SaveAsPrefabAsset(newPrefab, path, out isSucceed);
            Debug.Log(path + " Save Succeed：" + isSucceed);
        }
        else if (IsRunningAll)
        {
            PrefabUtility.SavePrefabAsset(m_CurItemRef.gameObject);
        }
        else if (IsPrefabMode)
        {
            EditorUtility.SetDirty(prefab);
        }

        m_CurItemRef = prefab.GetComponent<UIItemReference>();

        AssetDatabase.Refresh();

        string realDiskCSFilePath = ViewCodePath + "/" + Path.GetFileNameWithoutExtension(prefab.name) + "View.cs";

        string codeStr = "//This file is auto generated by Tool/Auto Gen Code\n";

        if (IsSpectatorMode)
        {
            codeStr += "#if SPECTATOR_MODE\n";
        }

        codeStr += "using UnityEngine;\nusing UnityEngine.UI;\nusing TMPro;\n\n\tpublic class " + Path.GetFileNameWithoutExtension(prefab.name) + "View : UIBaseView\n    {\n";

        if (m_CurItemRef.itemVarList.Count > 0)
        {
            foreach (var item in m_CurItemRef.itemVarList)
            {
                for (int i = 0; i < item.Components.Length; i++)
                {
                    var typeName = item.Components[i].GetType().Name;

                    var targetName = GetVarNameByDict(typeName + item.Target.name, i, item);

                    codeStr += "\t\tpublic " + typeName + " " + targetName + ";\n";
                }
            }

            codeStr += "\t\tprotected override void OnInit(Transform holder)\n\t\t{\n";
            codeStr += "\t\t\tvar itemRef = holder.GetComponent<UIItemReference>();\n";
            codeStr += "\t\t\tif(itemRef == null) return;\n";
            codeStr += "\t\t\tvar itemVarList = itemRef.itemVarList;\n";
            codeStr += "\t\t\tif(itemVarList == null || itemVarList.Count == 0) return;\n";

            for (int i = 0; i < m_CurItemRef.itemVarList.Count; i++)
            {
                var item = m_CurItemRef.itemVarList[i];
                if (item.Components == null)
                {
                    continue;
                }

                for (int j = 0; j < item.Components.Length; j++)
                {
                    var typeName = item.Components[j].GetType().Name;

                    var targetName = GetVarNameByDict(typeName + item.Target.name, j, item);

                    switch (typeName)
                    {
                        case "Transform":
                            codeStr += "\t\t\t" + targetName + " = itemVarList[" + i + "].Target;\n";
                            break;
                        case "GameObject":
                            codeStr += "\t\t\t" + targetName + " = itemVarList[" + i + "].Target.gameObject;\n";
                            break;
                        default:
                            codeStr += "\t\t\t" + targetName + " = itemVarList[" + i + "].Target.GetComponent<"
                                + typeName + ">();\n";
                            break;
                    }
                }
            }
        }

        codeStr += "\t\t}\n\t}";
        if (IsSpectatorMode)
        {
            codeStr += "\n#endif //SPECTATOR_MODE";
        }
        try
        {
            StreamWriter sw;
            sw = new StreamWriter(new FileStream(realDiskCSFilePath, FileMode.Create, FileAccess.ReadWrite));

            sw.Write(codeStr);
            sw.Close();
            sw.Dispose();
            //Debug.Log(realDiskCSFilePath + " Save Succeed");

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        var comps = newPrefab.GetComponents<UIItemReference>();

        if (comps.Length > 1)
        {
            for (int i = 1; i < comps.Length; i++)
            {
                UnityEngine.Object.DestroyImmediate(comps[i]);
            }
        }

        if (!IsRunningAll)
        {
            AssetDatabase.Refresh();
            VarNameDict.Clear();
        }
    }

    private void RemoveAllChildReference(GameObject root)
    {
        var childs = root.GetComponentsInChildren<Transform>(true);
        List<UIItemVariable> removeList = new List<UIItemVariable>();
        foreach (var itemVar in m_CurItemRef.itemVarList)
        {
            foreach (var child in childs)
            {
                if (itemVar.Target == child)
                {
                    removeList.Add(itemVar);
                }
            }
        }

        foreach (var item in removeList)
        {
            m_CurItemRef.itemVarList.Remove(item);
        }
    }

    private void UpdatePrefabReference()
    {
        string realDiskCSFilePath = ViewCodePath + "/" + m_CurItemRef.gameObject.name + "View.cs";
        if (!File.Exists(realDiskCSFilePath))
        {
            return;
        }

        var lines = File.ReadAllLines(realDiskCSFilePath);
        if (lines == null)
        {
            return;
        }

        int lineOffset = 8;
        var itemList = m_CurItemRef.itemVarList;
        for (int itemIndex = 0; itemIndex < itemList.Count; itemIndex++)
        {
            var compArray = itemList[itemIndex].Components;
            if (compArray == null || compArray.Length == 0)
            {
                lineOffset -= 1;
                continue;
            }

            itemList[itemIndex].VarNameArray = new string[compArray.Length];

            for (int compIndex = 0; compIndex < compArray.Length; compIndex++)
            {
                if (itemIndex + lineOffset + compIndex > lines.Length)
                {
                    return;
                }

                var lineValue = lines[itemIndex + lineOffset + compIndex].Split(' ');

                if (lineValue.Length > 2 && (lineValue[2].Length - 1) > 0)
                {
                    var viewName = lineValue[2].Substring(0, lineValue[2].Length - 1);

                    if (viewName == itemList[itemIndex].Target.name)
                    {
                        continue;
                    }
                    else
                    {
                        itemList[itemIndex].VarNameArray[compIndex] = viewName;
                    }
                }
            }
            if (compArray.Length > 1)
            {
                lineOffset += compArray.Length - 1;
            }

        }
    }

    public void GenControlCode()
    {
        GameObject prefab = m_CurItemRef.gameObject;
        string realDiskCSFilePath = ControlCodePath + "/" + Path.GetFileNameWithoutExtension(prefab.name) + "Controller.cs";

        string codeStr = "using UnityEngine;\n\n\t[UnityEngine.AddComponentMenu(\"\")]\n\tpublic class " + Path.GetFileNameWithoutExtension(prefab.name) + "Controller : UIBaseController\n\t{\n";

        codeStr += "\t\tprivate " + Path.GetFileNameWithoutExtension(prefab.name) + "View m_View;\n\n";

        codeStr += "\t\tpublic static string GetPrefabPath()\n\t\t{\n\t\t\treturn " + '\"' + PrefabPath + '\"' + ";\n\t\t}\n\n";

        codeStr += "\t\tprotected override void OnUIInit()\n\t\t{\n\t\t\tbase.OnUIInit();\n\t\t\tm_View = CreateView(typeof(" + Path.GetFileNameWithoutExtension(prefab.name) + "View)) as " + Path.GetFileNameWithoutExtension(prefab.name) + "View;\n\t\t}\n\n";

        codeStr += "\t\tprotected override void OnUIDestory()\n\t\t{\n\t\t\tbase.OnUIDestory();\n\t\t}\n\t}";


        try
        {
            StreamWriter sw;
            sw = new StreamWriter(new FileStream(realDiskCSFilePath, FileMode.Create, FileAccess.ReadWrite));

            sw.Write(codeStr);
            sw.Close();
            sw.Dispose();
            Debug.Log(realDiskCSFilePath + " Save Succeed");

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
