using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public static int CANVAS_WIDTH = 1920;
    public static int CANVAS_HEIGHT = 1080;

    private Transform uiRoot;
    public Transform UIRoot
    {
        get
        {
            if (uiRoot == null)
                uiRoot = GameObject.Find("UIRoot").transform;
            return uiRoot;
        }
    }

    public void OpenPanel<T>(params object[]args) where T:UIBaseController
    {
        Type t = typeof(T);
        MethodInfo getPathMethod = t.GetMethod("GetPrefabPath");
        if(getPathMethod != null)
        {
            string path = (string)getPathMethod.Invoke(null, null);
            ResourceManager.Instance.LoadAndInstantiateAsync<GameObject>(path, (obj) =>
            {
                UIBaseController c = obj.GetComponent<T>();
                if(c == null)
                    c = obj.AddComponent<T>();
                c.OnOpen(args);
            });
        }
    }

    public void ClosePanel(UIBaseController controller)
    {
        if (controller != null)
            controller.Close();
        GameObject.Destroy(controller.gameObject);
    }

    private void OnPrefabCreated(AsyncOperationHandle obj)
    {

    }

    public void ShowFloatingText(string content)
    {
        ResourceManager.Instance.LoadAndInstantiateAsync<GameObject>("Assets/Prefabs/FloatingText.prefab", (obj) =>
        {
            obj.transform.SetParent(UIRoot);
            obj.transform.localPosition = Vector3.zero;
            obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = content;
            obj.GetComponent<RectTransform>().DOAnchorPosY(200, 1).OnComplete(()=>OnFloatingComplete(obj));
        });
    }

    void OnFloatingComplete(GameObject obj)
    {
        ResourceManager.Instance.DestroyInstance(obj);
    }
}

