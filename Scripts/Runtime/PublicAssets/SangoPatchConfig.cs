using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    [CreateAssetMenu(fileName = "SangoPatchConfigAssets", menuName = "ScriptableObjects/Patchs_YooAsset/SangoPatchConfigAssets", order = 1)]
    public class SangoPatchConfig : ScriptableObject
    {
        public string HostServerIP;
        public string AppID;
        public string AppVersion;
        public bool AppendTimeTicks = true;
        public bool SkipCertificate = false;
        public int Timeout = 60;
        public int DownloadingMaxNum = 10;
        public int FailedTryAgain = 3;

        public string PackageName = "DefaultPackage";
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
        public EDefaultBuildPipeline BuildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline;

        public GameObject SangoPatchWnd;
        public string GameRootObjectName;
        public string GameRootParentTransform;
        public List<string> HotUpdateDllList;
        public List<string> AOTMetaAssemblyNames;

        public UnityEvent OnUpdaterDone;
    }
}
