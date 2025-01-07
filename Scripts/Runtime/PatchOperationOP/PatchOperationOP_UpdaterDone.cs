using SangoUtils.Patchs_YooAsset.Utils;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_UpdaterDone : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.UpdaterDone;

        internal override void OnEvent()
        {
            LoadGameRootObject();
            EventBus_Patchs.PatchOperation.PatchOperationData.PatchConfig.OnUpdaterDone?.Invoke();
        }

        private void LoadGameRootObject()
        {
            var packageName = EventBus_Patchs.PatchOperation.PatchOperationData.PatchConfig.PackageName;
            var package = YooAssets.GetPackage(packageName);
            var GameRootObject = EventBus_Patchs.PatchOperation.PatchOperationData.PatchConfig.GameRootObjectName;
            var asset1 = package.LoadAssetSync<GameObject>(GameRootObject);
            var asset1Transform = GameObject.Find(EventBus_Patchs.PatchOperation.PatchOperationData.PatchConfig.GameRootParentTransform).transform;
            asset1.InstantiateSync(new Vector3(0, 0, 0), Quaternion.identity, asset1Transform);
        }
    }
}