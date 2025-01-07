using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    [CreateAssetMenu(fileName = "PatchConfig", menuName = "ScriptableObjects/Patchs_YooAsset/SangoPatchConfig", order = 1)]
    public class SangoPatchConfig : ScriptableObject
    {
        public string HostServerIP;
        public string AppID;
        public string AppVersion;
        public bool AppendTimeTicks = true;
        public int Timeout = 60;
        public int DownloadingMaxNum = 10;
        public int FailedTryAgain = 3;

        public string PackageName = "DefaultPackage";
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
        public EDefaultBuildPipeline BuildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline;

        public GameObject SangoPatchWnd;
        public string GameRootObjectName;
        public Transform GameRootParentTransform;
        public List<string> HotUpdateDllList;
        public List<string> AOTMetaAssemblyNames;

        public UnityEvent OnUpdaterDone;
    }
}
