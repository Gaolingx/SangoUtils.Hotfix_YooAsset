using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_DownloadPackageFiles : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.DownloadPackageFiles;

        internal override void OnEvent()
        {
            EventBus_Patchs.CallPatchSystemEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "开始下载补丁文件！"));
            _BeginDownloadASync().Start();
        }

        private IEnumerator _BeginDownloadASync()
        {
            var downloader = EventBus_Patchs.PatchConfig.ResourceDownloaderOperation;
            downloader.OnDownloadErrorCallback = delegate (string fileName, string error)
            {
                EventBus_Patchs.CallPatchSystemEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.WebFileDownloadFailed, fileName, error));
            };
            downloader.OnDownloadProgressCallback = delegate (int totalDownloadCount, int currentDownloadCount, long totalDownloadSizeBytes, long currentDownloadSizeBytes)
            {
                EventBus_Patchs.CallPatchSystem_DownloadProgressUpdateEvent(this, new PatchSystem_DownloadProgressUpdateEventArgs
                    (totalDownloadCount, currentDownloadCount, totalDownloadSizeBytes, currentDownloadSizeBytes));
            };
            downloader.BeginDownload();
            yield return downloader;

            if (downloader.Status != EOperationStatus.Succeed)
                yield break;

            EventBus_Patchs.CallPatchOperationEvent(this, new PatchOperationEventArgs(PatchOperationEventCode.DownloadPackageOver));
        }
    }
}