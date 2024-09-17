using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class SangoPatchConfig : MonoBehaviour
    {
        //The CDN PathDef as following, you can change it in PatchOperationOP_InitializePackage.GetHostServerURL()
        [SerializeField]
        [Tooltip("{hostServerIP}/CDN/Editor/Unity/{appID}/Patch/PC/{appVersion}")]
        private string _hostServerIP = "https://";
        [SerializeField]
        private string _appID;
        [SerializeField]
        private string _appVersion;
        [SerializeField]
        private bool _appendTimeTicks = true;
        [SerializeField]
        private int _timeout = 60;
        [SerializeField]
        private string _packageName = "DefaultPackage";
        [SerializeField]
        private EPlayMode _playMode = EPlayMode.HostPlayMode;
        [SerializeField]
        private EDefaultBuildPipeline _buildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline;
        [SerializeField]
        private GameObject _sangoPatchWnd;

        [SerializeField]
        private string _gameRootObjectName;
        [SerializeField]
        private string _gameRootParentTransformName;
        [SerializeField]
        private string _hotUpdateDllName;
        [SerializeField]
        private List<string> _AOTMetaAssemblyNames;
        [SerializeField]
        private UnityEvent _onUpdaterDone;


        public string HostServerIP { get => _hostServerIP; }
        public string AppID { get => _appID; }
        public string AppVersion { get => _appVersion; }
        public bool AppendTimeTicks { get => _appendTimeTicks; }
        public int Timeout { get => _timeout; }
        public string PackageName { get => _packageName; }
        public EPlayMode PlayMode { get => _playMode; }
        public EDefaultBuildPipeline BuildPipeline { get => _buildPipeline; }
        public GameObject SangoPatchWnd { get => _sangoPatchWnd; }

        public string GameRootObjectName { get => _gameRootObjectName; }
        public string GameRootParentTransformName { get => _gameRootParentTransformName; }
        public string HotUpdateDllName { get => _hotUpdateDllName; }
        public List<string> AOTMetaAssemblyNames { get => _AOTMetaAssemblyNames; }

        public UnityEvent OnUpdaterDone { get => _onUpdaterDone; }

        internal string PackageVersion { get; set; }
        internal ResourceDownloaderOperation ResourceDownloaderOperation { get; set; }
    }
}
