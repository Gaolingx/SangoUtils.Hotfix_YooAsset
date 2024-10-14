using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_UpdatePackageVersion : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.UpdatePackageVersion;

        internal override void OnEvent()
        {
            EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "获取最新的资源版本!"));
            UpdatePackageVersion().Start();
        }

        private IEnumerator UpdatePackageVersion()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var appendTimeTicks = EventBus_Patchs.PatchOperation.PatchOperationData.AppendTimeTicks;
            var timeout = EventBus_Patchs.PatchOperation.PatchOperationData.Timeout;
            var packageName = EventBus_Patchs.PatchOperation.PatchOperationData.PackageName;
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageVersionAsync(appendTimeTicks, timeout);
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.PackageVersionUpdateFailed));
            }
            else
            {
                EventBus_Patchs.PatchOperation.PatchOperationData.PackageVersion = operation.PackageVersion;
                EventBus_Patchs.PatchOperation.SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.UpdatePackageManifest));
            }
        }
    }
}