using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResourceManager : Singleton<ResourceManager>
{
    public void LoadSprite2Image(Image img,string resPath)
    {
        if (assetsCache.ContainsKey(resPath))
            img.sprite = assetsCache[resPath] as Sprite;
        else
        {
            Addressables.LoadAssetAsync<Sprite>(resPath).Completed += h =>
            {
                if (h.Status == AsyncOperationStatus.Succeeded)
                {
                    assetsCache[resPath] = h.Result;
                    img.sprite = h.Result;
                }

            };
        }
    }
    Dictionary<string, object> assetsCache = new Dictionary<string, object>();
    public TObject Load<TObject>(string resPath)
    {
        if (assetsCache.ContainsKey(resPath))
            return (TObject)assetsCache[resPath];
        var handler = Addressables.LoadAssetAsync<TObject>(resPath);
        var obj = handler.WaitForCompletion();
        assetsCache[resPath] = obj;
        return obj;
    }

    public GameObject LoadAndInstantiate(string resPath, Vector3 pos, Quaternion rot)
    {
        var prefab = this.Load<GameObject>(resPath);
        return GameObject.Instantiate<GameObject>(prefab, pos, rot);
    }

    public delegate void LoadCallBack(GameObject obj);
    public delegate void LoadBytesCallBack(object obj);

    public void LoadAsync<TObject>(string resPath, LoadBytesCallBack callback)
    {
        if(assetsCache.ContainsKey(resPath))
        {
            if (callback != null)
                callback(assetsCache[resPath]);
        }
        Addressables.LoadAssetAsync<TObject>(resPath).Completed += h =>
         {
             if(h.Status == AsyncOperationStatus.Succeeded)
             {
                 assetsCache[resPath] = h.Result;
                 if(callback != null)
                 {
                     callback(h.Result);
                 }
             }
 
         };
    }
    public void LoadAndInstantiateAsync<GameObject>(string resPath,LoadCallBack callback)
    {
        Addressables.InstantiateAsync(resPath).Completed += h =>
        {
            if(h.Status == AsyncOperationStatus.Succeeded )
            {
                if(callback != null)
                    callback(h.Result);
            }
            else
            {
                Debug.LogError("[Addressable]LoadAsset:" +resPath+"-"+ h.Status);
            }
        };

    }

    public void DestroyInstance(GameObject obj)
    {
        Addressables.ReleaseInstance(obj);
    }
}
