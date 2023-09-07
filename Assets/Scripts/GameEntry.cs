using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableManager.Instance.Init();
        UIManager.Instance.OpenPanel<LoginPanelController>();
    }

    // Update is called once per frame
    void Update()
    {
        SocketClient.Instance.Update();
    }

    private void OnDestroy()
    {
        SocketClient.Instance.Close();
    }
}
