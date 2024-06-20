using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchLinkedFSM_UpdatePackageVersion : FSMLinkedStaterItemBase
    {
        private CoroutineHandler _coroutine = null;

        internal override void OnEnter()
        {
            EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "��ȡ���µ���Դ�汾!"));
            _coroutine = UpdatePackageVersion().Start();
        }

        private IEnumerator UpdatePackageVersion()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageVersionAsync();
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PackageVersionUpdateFailed));
            }
            else
            {
                _fsmLinkedStater.SetBlackboardValue("PackageVersion", operation.PackageVersion);
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdatePackageManifest>();
            }
        }
    }
}