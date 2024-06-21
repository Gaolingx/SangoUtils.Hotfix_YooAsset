using SangoUtils.Patchs_YooAsset.Utils;
using System;
using YooAsset;


namespace SangoUtils.Patchs_YooAsset
{
    public class PatchOperation : GameAsyncOperation
    {
        private PatchConfig _currentPatchConfig;

        private FSMLinkedStater _fsmLinkedStater;

        private ESteps _steps = ESteps.None;

        private event EventHandler<PatchUserEventArgs> _onPatchUserEventHandler;

        public PatchOperation(PatchConfig patchConfig)
        {
            _currentPatchConfig = patchConfig;
            EventBus_Patchs.PatchOperation = this;
            _onPatchUserEventHandler += C_OnPatchUserEvent;
            AddFSMStater();
        }

        internal void OnEvent(object sender, PatchUserEventArgs eventArgs)
        {
            _onPatchUserEventHandler?.Invoke(sender, eventArgs);
        }

        protected override void OnAbort() { }

        protected override void OnStart()
        {
            _steps = ESteps.Update;
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_InitializePackage>(true);
        }

        protected override void OnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            if (_steps == ESteps.Update)
            {
                _fsmLinkedStater.UpdateCurrentStaterItem();
                if (_fsmLinkedStater.CurrentStaterName == typeof(PatchLinkedFSM_UpdaterDone).FullName)
                {
                    _onPatchUserEventHandler = null;
                    Status = EOperationStatus.Succeed;
                    _steps = ESteps.Done;
                }
            }
        }

        private void AddFSMStater()
        {
            _fsmLinkedStater = new FSMLinkedStater(this);
            _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_InitializePackage>();
            _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_UpdatePackageVersion>();
            _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_UpdatePackageManifest>();
            _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_CreatePackageDownloader>();
            _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_DownloadPackageFiles>();
            _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_DownloadPackageOver>();
            _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_ClearPackageCache>();
            _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_UpdaterDone>();

            _fsmLinkedStater.SetBlackboardValue("PackageName", _currentPatchConfig.PackageName);
            _fsmLinkedStater.SetBlackboardValue("PlayMode", _currentPatchConfig.PlayMode);
            _fsmLinkedStater.SetBlackboardValue("BuildPipeline", _currentPatchConfig.BuildPipeline.ToString());
            _fsmLinkedStater.SetBlackboardValue("HostServerIP", _currentPatchConfig.HostServerIP);
            _fsmLinkedStater.SetBlackboardValue("AppId", _currentPatchConfig.AppId);
            _fsmLinkedStater.SetBlackboardValue("AppVersion", _currentPatchConfig.AppVersion);
            _fsmLinkedStater.SetBlackboardValue("appendTimeTicks", _currentPatchConfig.appendTimeTicks);
            _fsmLinkedStater.SetBlackboardValue("GameRootObjectName", _currentPatchConfig.GameRootObjectName);
            _fsmLinkedStater.SetBlackboardValue("HotUpdateDllName", _currentPatchConfig.HotUpdateDllName);
            _fsmLinkedStater.SetBlackboardValue("AOTMetaAssemblyNames", _currentPatchConfig.AOTMetaAssemblyNames);
        }

        private void C_OnPatchUserEvent(object sender, PatchUserEventArgs eventArgs)
        {
            switch (eventArgs.PatchUserEventCode)
            {
                case PatchUserEventCode.UserTryInitialize:
                    _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_InitializePackage>();
                    break;
                case PatchUserEventCode.UserBeginDownloadWebFiles:
                    _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_DownloadPackageFiles>();
                    break;
                case PatchUserEventCode.UserTryUpdatePackageVersion:
                    _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdatePackageVersion>();
                    break;
                case PatchUserEventCode.UserTryUpdatePatchManifest:
                    _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdatePackageManifest>();
                    break;
                case PatchUserEventCode.UserTryDownloadWebFiles:
                    _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_CreatePackageDownloader>();
                    break;
            }
        }

        private enum ESteps
        {
            None,
            Update,
            Done,
        }
    }
}