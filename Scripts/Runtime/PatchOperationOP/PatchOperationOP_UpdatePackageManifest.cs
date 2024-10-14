using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_UpdatePackageManifest : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.UpdatePackageManifest;

        internal override void OnEvent()
        {
            EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "������Դ�嵥��"));
            UpdateManifest().Start();
        }

        private IEnumerator UpdateManifest()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = EventBus_Patchs.PatchOperation.PatchOperationData.PackageName;
            var packageVersion = EventBus_Patchs.PatchOperation.PatchOperationData.PackageVersion;
            var timeout = EventBus_Patchs.PatchOperation.PatchOperationData.Timeout;
            var package = YooAssets.GetPackage(packageName);
            bool savePackageVersion = true;
            var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion, timeout);
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchManifestUpdateFailed));
                yield break;
            }
            else
            {
                EventBus_Patchs.PatchOperation.SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.CreatePackageDownloader));
            }
        }
    }
}