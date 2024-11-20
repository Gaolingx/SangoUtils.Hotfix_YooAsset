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
            GameObject hotFixRoot = asset1.InstantiateSync(EventBus_Patchs.PatchOperation.PatchOperationData.PatchConfig.GameRootParentTransform);

            if (hotFixRoot.TryGetComponent<RectTransform>(out var rect))
            {
                rect.offsetMax = new Vector2(0, 0);
                rect.offsetMin = new Vector2(0, 0);
            }
        }
    }
}