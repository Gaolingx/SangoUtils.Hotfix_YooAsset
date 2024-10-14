using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_CreatePackageDownloader : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.CreatePackageDownloader;

        internal override void OnEvent()
        {
            EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "创建补丁下载器！"));
            CreateDownloader().Start();
        }

        private IEnumerator CreateDownloader()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = EventBus_Patchs.PatchOperation.PatchOperationData.PackageName;
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = EventBus_Patchs.PatchOperation.PatchOperationData.DownloadingMaxNum;
            int failedTryAgain = EventBus_Patchs.PatchOperation.PatchOperationData.FailedTryAgain;
            int timeout = EventBus_Patchs.PatchOperation.PatchOperationData.Timeout;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain, timeout);

            EventBus_Patchs.PatchOperation.PatchOperationData.ResourceDownloaderOperation = downloader;

            if (downloader.TotalDownloadCount == 0)
            {
                Debug.Log("Not found any download files !");
                EventBus_Patchs.PatchOperation.SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.ClearPackageCache));
            }
            else
            {
                // 发现新更新文件后，挂起流程系统
                // 注意：开发者需要在下载前检测磁盘空间不足
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.FoundUpdateFiles, totalDownloadCount, totalDownloadBytes));
            }
        }
    }
}