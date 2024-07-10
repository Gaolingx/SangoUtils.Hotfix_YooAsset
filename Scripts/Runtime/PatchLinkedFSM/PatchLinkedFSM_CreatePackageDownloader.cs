using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchLinkedFSM_CreatePackageDownloader : FSMLinkedStaterItemBase
    {
        private CoroutineHandler _coroutine = null;

        internal override void OnEnter()
        {
            EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "创建补丁下载器！"));
            _coroutine = CreateDownloader().Start();
        }

        private IEnumerator CreateDownloader()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            _fsmLinkedStater.SetBlackboardValue("Downloader", downloader);

            if (downloader.TotalDownloadCount == 0)
            {
                Debug.Log("Not found any download files !");
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_ClearPackageCache>();
            }
            else
            {
                // 发现新更新文件后，挂起流程系统
                // 注意：开发者需要在下载前检测磁盘空间不足
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.FoundUpdateFiles, totalDownloadCount, totalDownloadBytes));
            }
        }
    }
}