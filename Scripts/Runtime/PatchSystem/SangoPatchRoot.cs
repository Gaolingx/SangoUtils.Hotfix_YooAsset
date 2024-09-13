using SangoUtils.Patchs_YooAsset.Utils;
using System;
using UnityEngine;

namespace SangoUtils.Patchs_YooAsset
{
    public class SangoPatchRoot : MonoBehaviour
    {
        private SangoPatchWnd _sangoPatchWnd;

        private event EventHandler<PatchSystemEventArgs> _onPatchSystemEventHandler;

        private class GetBlackboardValue : FSMStaterBase
        {
            private FSMLinkedStater _fsmLinkedStater;

            public string GetPatchWndPath()
            {
                return (string)_fsmLinkedStater.GetBlackboardValue("GameRootParentTransformName") + "/SangoPatchRoot/SangoPatchWnd";
            }
        }

        public void OnInit()
        {
            GetBlackboardValue blackboardValue = new GetBlackboardValue();
            _sangoPatchWnd = transform.Find(blackboardValue.GetPatchWndPath()).GetComponent<SangoPatchWnd>();

            EventBus_Patchs.SangoPatchRoot = this;
            _onPatchSystemEventHandler += C_OnPatchSystemEvent;

            _sangoPatchWnd.SetRoot(this);
            _sangoPatchWnd.gameObject.SetActive(true);
            _sangoPatchWnd.Initialize();
        }

        internal void OnEvent(object sender, PatchSystemEventArgs eventArgs)
        {
            _onPatchSystemEventHandler?.Invoke(sender, eventArgs);
        }

        private void PatchSystemEvent_InitializeFailed(PatchSystemEventArgs eventArgs)
        {
            Action callback = delegate
            {
                EventBus_Patchs.PatchOperation.OnEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserTryInitialize));
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to initialize package !", callback);
        }

        private void PatchSystemEvent_PatchStatesChange(PatchSystemEventArgs eventArgs)
        {
            string tips = eventArgs.ExtensionData[0].ToString();
            _sangoPatchWnd.UpdateTips(tips);
        }

        private void PatchSystemEvent_FoundUpdateFiles(PatchSystemEventArgs eventArgs)
        {
            int totalCount = int.Parse(eventArgs.ExtensionData[0].ToString());
            long totalSizeBytes = long.Parse(eventArgs.ExtensionData[1].ToString());
            Action callback1 = delegate
            {
                EventBus_Patchs.PatchOperation.OnEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserBeginDownloadWebFiles));
            };
            float sizeMB = totalSizeBytes / 1048576f;
            sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
            string totalSizeMB = sizeMB.ToString("f1");
            _sangoPatchWnd.ShowMessageBox($"Found update patch files, Total count {totalCount} Total szie {totalSizeMB}MB", callback1);
        }

        private void PatchSystemEvent_DownloadProgressUpdate(PatchSystemEventArgs eventArgs)
        {
            int totalDownloadCount = int.Parse(eventArgs.ExtensionData[0].ToString());
            int currentDownloadCount = int.Parse(eventArgs.ExtensionData[1].ToString());
            long totalDownloadSizeBytes = long.Parse(eventArgs.ExtensionData[2].ToString());
            long currentDownloadSizeBytes = long.Parse(eventArgs.ExtensionData[3].ToString());
            float sliderValue = (float)currentDownloadCount / totalDownloadCount;
            _sangoPatchWnd.UpdateSliderValue(sliderValue);
            string currentSizeMB = (currentDownloadSizeBytes / 1048576f).ToString("f1");
            string totalSizeMB1 = (totalDownloadSizeBytes / 1048576f).ToString("f1");
            string tips1 = $"{currentDownloadCount}/{totalDownloadCount} {currentSizeMB}MB/{totalSizeMB1}MB";
            _sangoPatchWnd.UpdateTips(tips1);
        }

        private void PatchSystemEvent_PackageVersionUpdateFailed(PatchSystemEventArgs eventArgs)
        {
            Action callback2 = delegate
            {
                EventBus_Patchs.PatchOperation.OnEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserTryUpdatePackageVersion));
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to update static version, please check the network status.", callback2);
        }

        private void PatchSystemEvent_PatchManifestUpdateFailed(PatchSystemEventArgs eventArgs)
        {
            Action callback3 = delegate
            {
                EventBus_Patchs.PatchOperation.OnEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserTryUpdatePatchManifest));
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to update patch manifest, please check the network status.", callback3);
        }

        private void PatchSystemEvent_WebFileDownloadFailed(PatchSystemEventArgs eventArgs)
        {
            string fileName = eventArgs.ExtensionData[0].ToString();
            string Error = eventArgs.ExtensionData[1].ToString();
            Action callback4 = delegate
            {
                EventBus_Patchs.PatchOperation.OnEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserTryDownloadWebFiles));
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to download file : {fileName}", callback4);
        }

        private void PatchSystemEvent_ClosePatchWindow()
        {
            _sangoPatchWnd.gameObject.SetActive(false);
        }

        private void C_OnPatchSystemEvent(object sender, PatchSystemEventArgs eventArgs)
        {
            switch (eventArgs.PatchSystemEventCode)
            {
                case PatchSystemEventCode.InitializeFailed:
                    PatchSystemEvent_InitializeFailed(eventArgs);

                    break;
                case PatchSystemEventCode.PatchStatesChange:
                    PatchSystemEvent_PatchStatesChange(eventArgs);

                    break;
                case PatchSystemEventCode.FoundUpdateFiles:
                    PatchSystemEvent_FoundUpdateFiles(eventArgs);

                    break;
                case PatchSystemEventCode.DownloadProgressUpdate:
                    PatchSystemEvent_DownloadProgressUpdate(eventArgs);

                    break;
                case PatchSystemEventCode.PackageVersionUpdateFailed:
                    PatchSystemEvent_PackageVersionUpdateFailed(eventArgs);

                    break;
                case PatchSystemEventCode.PatchManifestUpdateFailed:
                    PatchSystemEvent_PatchManifestUpdateFailed(eventArgs);

                    break;
                case PatchSystemEventCode.WebFileDownloadFailed:
                    PatchSystemEvent_WebFileDownloadFailed(eventArgs);

                    break;
                case PatchSystemEventCode.ClosePatchWindow:
                    PatchSystemEvent_ClosePatchWindow();

                    break;
            }
        }
    }
}