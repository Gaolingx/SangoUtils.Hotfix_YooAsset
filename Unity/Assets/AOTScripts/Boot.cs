using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    public static Boot Instance { get; private set; }

    private void Start()
    {
        Debug.Log("Æô¶¯³É¹¦");
        Instance = this;
        DontDestroyOnLoad(this);
        InitRoot();
    }
    private void InitRoot()
    {
        HotFixService hotFixService = GetComponent<HotFixService>();
        hotFixService.InitService();
    }
}
