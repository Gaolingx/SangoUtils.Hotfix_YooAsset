using SangoUtils.Patchs_YooAsset.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    public class PatchService : Singleton<PatchService>
    {
        protected override void Awake()
        {
            base.Awake();
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

        public PatchConfig GetConfig()
        {
            return _currentPatchConfig;
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
        public string HostServerToken { get; set; }
        public bool appendTimeTicks { get; set; }
        public string PackageName { get; set; }
        public EPlayMode PlayMode { get; set; }
        public EDefaultBuildPipeline BuildPipeline { get; set; }
        public string GameRootObjectName { get; set; }
        public string GameRootParentTransformName { get; set; }
        public string HotUpdateDllName { get; set; }
        public List<string> AOTMetaAssemblyNames { get; set; }
    }
}