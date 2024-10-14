using SangoUtils.Patchs_YooAsset.Utils;
using System;
using System.Collections.Generic;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperation : GameAsyncOperation
    {
        private readonly SangoPatchConfig _currentPatchConfig;

        private event EventHandler<PatchUserEventArgs> _onPatchUserEventHandler;
        private event EventHandler<PatchOperationEventArgs> _onPatchOperationEventHandler;

        internal PatchOperationData PatchOperationData { get; set; }

        private readonly Dictionary<PatchOperationEventCode, PatchOperationOP_Base> _patchOperationOPs = new Dictionary<PatchOperationEventCode, PatchOperationOP_Base>();

        internal PatchOperation(SangoPatchConfig patchConfig)
        {
            _currentPatchConfig = patchConfig;
            EventBus_Patchs.PatchOperation = this;
            _onPatchUserEventHandler += C_OnPatchUserEvent;
            _onPatchOperationEventHandler += C_OnPatchOperationEvent;
            AddPatchOperationOPs();
        }

        internal void SendMessage(object sender, PatchUserEventArgs eventArgs)
        {
            _onPatchUserEventHandler?.Invoke(sender, eventArgs);
        }

        internal void SendMessage(object sender, PatchOperationEventArgs eventArgs)
        {
            _onPatchOperationEventHandler?.Invoke(sender, eventArgs);
        }

        protected override void OnStart()
        {
            SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.InitializePackage));
        }

        protected override void OnUpdate() { }

        protected override void OnAbort() { }

        private void AddPatchOperationOPs()
        {
            Add<PatchOperationOP_InitializePackage>();
            Add<PatchOperationOP_UpdatePackageVersion>();
            Add<PatchOperationOP_UpdatePackageManifest>();
            Add<PatchOperationOP_CreatePackageDownloader>();
            Add<PatchOperationOP_DownloadPackageFiles>();
            Add<PatchOperationOP_DownloadPackageOver>();
            Add<PatchOperationOP_ClearPackageCache>();
            Add<PatchOperationOP_UpdateLoadHotfixDll>();
            Add<PatchOperationOP_UpdaterDone>();

            PatchOperationData = new(_currentPatchConfig.PackageName,
                _currentPatchConfig.PlayMode,
                _currentPatchConfig.BuildPipeline.ToString(),
                _currentPatchConfig.HostServerIP,
                _currentPatchConfig.AppID,
                _currentPatchConfig.AppVersion,
                _currentPatchConfig.AppendTimeTicks,
                _currentPatchConfig.Timeout,
                _currentPatchConfig.DownloadingMaxNum,
                _currentPatchConfig.FailedTryAgain,
                _currentPatchConfig.SangoPatchWnd,
                _currentPatchConfig.GameRootObjectName,
                _currentPatchConfig.GameRootParentTransform,
                _currentPatchConfig.HotUpdateDllList,
                _currentPatchConfig.AOTMetaAssemblyNames,
                _currentPatchConfig.OnUpdaterDone);

            void Add<T>() where T : PatchOperationOP_Base
            {
                T patchOperation = Activator.CreateInstance<T>();
                _patchOperationOPs.Add(patchOperation.PatchOperationEventCode, patchOperation);
            }
        }

        private void C_OnPatchUserEvent(object sender, PatchUserEventArgs eventArgs)
        {
            switch (eventArgs.PatchUserEventCode)
            {
                case PatchUserEventCode.UserTryInitialize:
                    SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.InitializePackage));
                    break;
                case PatchUserEventCode.UserBeginDownloadWebFiles:
                    SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.DownloadPackageFiles));
                    break;
                case PatchUserEventCode.UserTryUpdatePackageVersion:
                    SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.UpdatePackageVersion));
                    break;
                case PatchUserEventCode.UserTryUpdatePatchManifest:
                    SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.UpdatePackageManifest));
                    break;
                case PatchUserEventCode.UserTryDownloadWebFiles:
                    SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.CreatePackageDownloader));
                    break;
            }
        }

        private void C_OnPatchOperationEvent(object sender, PatchOperationEventArgs eventArgs)
        {
            if (_patchOperationOPs.TryGetValue(eventArgs.PatchOperationEventCode, out var patchOperationOP))
            {
                patchOperationOP.OnEvent();
            }
            if (eventArgs.PatchOperationEventCode == PatchOperationEventCode.UpdaterDone)
            {
                _onPatchUserEventHandler = null;
                Status = EOperationStatus.Succeed;
            }
        }
    }
}