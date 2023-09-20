using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updateer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResourceManager.Instance.LoadAndInstantiateAsync<GameObject>("Prefab4Update", null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
