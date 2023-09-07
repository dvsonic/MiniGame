using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : Singleton<ResourceManager>
{
    public GameObject Load<GameObject>(string resPath)
    {
        var handler = Addressables.LoadAssetAsync<GameObject>(resPath);
        return handler.WaitForCompletion();
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
        Addressables.LoadAssetAsync<TObject>(resPath).Completed += h =>
         {
             if(h.Status == AsyncOperationStatus.Succeeded)
             {
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
