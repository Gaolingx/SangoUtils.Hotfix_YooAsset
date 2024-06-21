using SangoUtils.Patchs_YooAsset.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    public class PatchService : MonoBehaviour
    {
        private static PatchService _instance;

        public static PatchService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(PatchService)) as PatchService;
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject("[" + typeof(PatchService).FullName + "]");
                        _instance = gameObject.AddComponent<PatchService>();
                        gameObject.hideFlags = HideFlags.HideInHierarchy;
                        DontDestroyOnLoad(gameObject);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (null != _instance && _instance != this)
            {
                Destroy(gameObject);
            }
        }

        private PatchConfig _currentPatchConfig;

        private CoroutineHandler _coroutine = null;

        public Action<bool> OnPatchCompleted { get; set; }

        public void Initialize()
        {
            _coroutine = StartOperation().Start();
        }

        public void SetConfig(PatchConfig patchConfig)
        {
            _currentPatchConfig = patchConfig;
        }


        private IEnumerator StartOperation()
        {
            YooAssets.Initialize();

            PatchOperation hotFixOperation = new PatchOperation(_currentPatchConfig);
            YooAssets.StartOperation(hotFixOperation);
            yield return hotFixOperation;

            ResourcePackage assetPackage = YooAssets.GetPackage(_currentPatchConfig.PackageName);
            YooAssets.SetDefaultPackage(assetPackage);

            EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.ClosePatchWindow));
            OnPatchCompleted?.Invoke(true);
        }
    }

    public class PatchConfig
    {
        public string HostServerIP { get; set; }
        public string AppId { get; set; }
        public string AppVersion { get; set; }
        public bool appendTimeTicks { get; set; }
        public string PackageName { get; set; }
        public EPlayMode PlayMode { get; set; }
        public EDefaultBuildPipeline BuildPipeline { get; set; }
        public string GameRootObjectName { get; set; }
        public string HotUpdateDllName { get; set; }
        public List<string> AOTMetaAssemblyNames { get; set; }
    }
}