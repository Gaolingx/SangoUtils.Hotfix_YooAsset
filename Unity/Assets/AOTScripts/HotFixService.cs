using SangoUtils.Patchs_YooAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YooAsset;

[RequireComponent(typeof(HotFixConfig))]
public class HotFixService : MonoBehaviour
{
    public static HotFixService Instance { get; private set; }
    
    public SangoPatchRoot sangoPatchRoot;

    public void InitService()
    {
        Instance = this;
        InitPatchService();
    }

    private void InitPatchService()
    {
        if (sangoPatchRoot != null)
        {
            PatchService.Instance.SetConfig(GetPatchConfig());
            sangoPatchRoot.OnInit();
            PatchService.Instance.Initialize();
        }
    }

    private PatchConfig GetPatchConfig()
    {
        PatchConfig patchConfig = new PatchConfig();
        HotFixConfig hotFixConfig = GetComponent<HotFixConfig>();
        patchConfig.HostServerIP = hotFixConfig.HostServerIP;
        patchConfig.AppId = hotFixConfig.AppId;
        patchConfig.AppVersion = hotFixConfig.AppVersion;
        patchConfig.PackageName = hotFixConfig.PackageName;
        patchConfig.appendTimeTicks = hotFixConfig.AppendTimeTicks;
        patchConfig.PlayMode = hotFixConfig.PlayMode;
        patchConfig.BuildPipeline = hotFixConfig.BuildPipeline;
        patchConfig.GameRootObjectName = hotFixConfig.GameRootObjectName;
        patchConfig.HotUpdateDllName = hotFixConfig.HotUpdateDllName;
        patchConfig.AOTMetaAssemblyNames = hotFixConfig.AOTMetaAssemblyNames;
        return patchConfig;
    }
}
