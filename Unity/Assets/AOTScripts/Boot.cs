using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HotFixService))]
public class Boot : MonoBehaviour
{
    public static Boot Instance { get; private set; }

    [SerializeField]
    private bool isDontDestroyOnLoad = true;

    private void Start()
    {
        Debug.Log("Application启动成功，执行热更流程...");

        Instance = this;

        if (isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(this);
        }
        InitRoot();
    }

    private void InitRoot()
    {
        HotFixService hotFixService = GetComponent<HotFixService>();
        hotFixService.InitService();
    }
}
