using SangoUtils.Patchs_YooAsset.Utils;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchLinkedFSM_ClearPackageCache : FSMLinkedStaterItemBase
    {
        internal override void OnEnter()
        {
            EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "清理未使用的缓存文件！"));
            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearUnusedCacheFilesAsync();
            operation.Completed += Operation_Completed;
        }

        private void Operation_Completed(AsyncOperationBase obj)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdaterDone>();
        }
    }
}