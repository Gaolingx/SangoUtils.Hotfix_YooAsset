using SangoUtils.Patchs_YooAsset.Utils;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_DownloadPackageOver : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.DownloadPackageOver;

        internal override void OnEvent()
        {
            EventBus_Patchs.PatchOperation.SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.ClearPackageCache));
        }
    }
}