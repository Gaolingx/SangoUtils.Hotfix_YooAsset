using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchLinkedFSM_DownloadPackageFiles : FSMLinkedStaterItemBase
    {
        private CoroutineHandler _coroutine = null;

        internal override void OnEnter()
        {
            EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "开始下载补丁文件！"));
            _coroutine = BeginDownload().Start();
        }

        private IEnumerator BeginDownload()
        {
            var downloader = (ResourceDownloaderOperation)_fsmLinkedStater.GetBlackboardValue("Downloader");
            downloader.OnDownloadErrorCallback = delegate (string fileName, string error)
            {
                EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.WebFileDownloadFailed, fileName, error));
            };
            downloader.OnDownloadProgressCallback = delegate (int totalDownloadCount, int currentDownloadCount, long totalDownloadSizeBytes, long currentDownloadSizeBytes)
            {
                EventBus_Patchs.SangoPatchRoot.OnEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.DownloadProgressUpdate, 
                    totalDownloadCount, currentDownloadCount, totalDownloadSizeBytes, currentDownloadSizeBytes));
            };
            downloader.BeginDownload();
            yield return downloader;

            if (downloader.Status != EOperationStatus.Succeed)
                yield break;

            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_DownloadPackageOver>();
        }
    }
}