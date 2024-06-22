using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class HotFixConfig : MonoBehaviour
{
    public string HostServerIP;
    public string AppId;
    public string AppVersion;
    public string PackageName;
    public bool AppendTimeTicks;
    public EPlayMode PlayMode;
    public EDefaultBuildPipeline BuildPipeline;
    public string GameRootObjectName;
    public string GameRootParentTransformName;
    public string HotUpdateDllName;
    public List<string> AOTMetaAssemblyNames;
}
