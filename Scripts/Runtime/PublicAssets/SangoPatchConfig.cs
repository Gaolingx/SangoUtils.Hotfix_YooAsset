using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    public class SangoPatchConfig
    {
        public string HostServerIP { get; set; }
        public string AppID { get; set; }
        public string AppVersion { get; set; }
        public bool AppendTimeTicks { get; set; }
        public int Timeout { get; set; }
        public int DownloadingMaxNum { get; set; }
        public int FailedTryAgain { get; set; }

        public string PackageName { get; set; }
        public EPlayMode PlayMode { get; set; }
        public EDefaultBuildPipeline BuildPipeline { get; set; }

        public GameObject SangoPatchWnd { get; set; }
        public string GameRootObjectName { get; set; }
        public Transform GameRootParentTransform { get; set; }
        public List<string> HotUpdateDllList { get; set; }
        public List<string> AOTMetaAssemblyNames { get; set; }

        public UnityEvent OnUpdaterDone { get; set; }
    }
}
