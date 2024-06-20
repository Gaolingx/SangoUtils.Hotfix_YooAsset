using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchLinkedFSM_UpdatePackageManifest : FSMLinkedStaterItemBase
    {
        private CoroutineHandler _coroutine = null;

        internal override void OnEnter()
        {
            EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "更新资源清单！"));
            _coroutine = UpdateManifest().Start();
        }

        private IEnumerator UpdateManifest()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
            var packageVersion = (string)_fsmLinkedStater.GetBlackboardValue("PackageVersion");
            var package = YooAssets.GetPackage(packageName);
            bool savePackageVersion = true;
            var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchManifestUpdateFailed));
                yield break;
            }
            else
            {
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_CreatePackageDownloader>();
            }
        }
    }
}