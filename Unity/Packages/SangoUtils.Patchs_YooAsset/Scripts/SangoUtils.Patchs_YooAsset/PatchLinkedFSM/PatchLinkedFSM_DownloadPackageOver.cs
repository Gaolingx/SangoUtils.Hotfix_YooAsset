using SangoUtils.Patchs_YooAsset.Utils;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchLinkedFSM_DownloadPackageOver : FSMLinkedStaterItemBase
    {
        internal override void OnEnter()
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_ClearPackageCache>();
        }
    }
}