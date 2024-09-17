using SangoUtils.Patchs_YooAsset.Utils;
using System;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    [RequireComponent(typeof(SangoPatchConfig))]
    internal class SangoPatchRoot : MonoBehaviour
    {
        private SangoPatchWnd _sangoPatchWnd;

        private void Awake()
        {
            new EventBus_Patchs();

            EventBus_Patchs.PatchConfig = GetComponent<SangoPatchConfig>();

            GameObject patchWnd = Instantiate(EventBus_Patchs.PatchConfig.SangoPatchWnd, new Vector3(0, 0, 0), Quaternion.identity, transform.Find($"{EventBus_Patchs.PatchConfig.GameRootParentTransformName}"));
            patchWnd.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            patchWnd.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            patchWnd.AddComponent<SangoPatchWnd>();

            _sangoPatchWnd = patchWnd.GetComponent<SangoPatchWnd>();

            EventBus_Patchs.AddPatchSystemEvent(_OnPatchSystemEvent);
            EventBus_Patchs.AddPatchSystem_DownloadProgressUpdateEvent(_OnPatchSystemDownloadProgressUpdateEvent);

            _sangoPatchWnd.SetRoot(this);
        }

        private void Start()
        {
            _StartOperationASync().Start();

            _sangoPatchWnd.gameObject.SetActive(true);
            _sangoPatchWnd.Initialize();
        }

        private IEnumerator _StartOperationASync()
        {
            YooAssets.Initialize();

            PatchOperation hotFixOperation = new();
            YooAssets.StartOperation(hotFixOperation);
            yield return hotFixOperation;

            ResourcePackage assetPackage = YooAssets.GetPackage(EventBus_Patchs.PatchConfig.PackageName);
            YooAssets.SetDefaultPackage(assetPackage);

            EventBus_Patchs.CallPatchSystemEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.ClosePatchWindow));
        }

        private void _OnPatchSystemEvent(object sender, PatchSystemEventArgs eventArgs)
        {
            switch (eventArgs.PatchSystemEventCode)
            {
                case PatchSystemEventCode.InitializeFailed:
                    Action callback = delegate
                    {
                        EventBus_Patchs.CallPatchUserEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserTryInitialize));
                    };
                    _sangoPatchWnd.ShowMessageBox($"Failed to initialize package !", callback);
                    break;
                case PatchSystemEventCode.PatchStatesChange:
                    string tips = eventArgs.ExtensionData[0].ToString();
                    _sangoPatchWnd.UpdateTips(tips);
                    break;
                case PatchSystemEventCode.FoundUpdateFiles:
                    int totalCount = int.Parse(eventArgs.ExtensionData[0].ToString());
                    long totalSizeBytes = long.Parse(eventArgs.ExtensionData[1].ToString());
                    Action callback1 = delegate
                    {
                        EventBus_Patchs.CallPatchUserEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserBeginDownloadWebFiles));
                    };
                    float sizeMB = totalSizeBytes / 1048576f;
                    sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
                    string totalSizeMB = sizeMB.ToString("f1");
                    _sangoPatchWnd.ShowMessageBox($"Found update patch files, Total count {totalCount} Total szie {totalSizeMB}MB", callback1);
                    break;
                case PatchSystemEventCode.PackageVersionUpdateFailed:
                    Action callback2 = delegate
                    {
                        EventBus_Patchs.CallPatchUserEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserTryUpdatePackageVersion));
                    };
                    _sangoPatchWnd.ShowMessageBox($"Failed to update static version, please check the network status.", callback2);
                    break;
                case PatchSystemEventCode.PatchManifestUpdateFailed:
                    Action callback3 = delegate
                    {
                        EventBus_Patchs.CallPatchUserEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserTryUpdatePatchManifest));
                    };
                    _sangoPatchWnd.ShowMessageBox($"Failed to update patch manifest, please check the network status.", callback3);
                    break;
                case PatchSystemEventCode.WebFileDownloadFailed:
                    string fileName = eventArgs.ExtensionData[0].ToString();
                    string Error = eventArgs.ExtensionData[1].ToString();
                    Action callback4 = delegate
                    {
                        EventBus_Patchs.CallPatchUserEvent(this, new PatchUserEventArgs(PatchUserEventCode.UserTryDownloadWebFiles));
                    };
                    _sangoPatchWnd.ShowMessageBox($"Failed to download file : {fileName}", callback4);
                    break;
                case PatchSystemEventCode.ClosePatchWindow:
                    _sangoPatchWnd.gameObject.SetActive(false);
                    break;
            }
        }

        private void _OnPatchSystemDownloadProgressUpdateEvent(object sender, PatchSystem_DownloadProgressUpdateEventArgs eventArgs)
        {
            int totalDownloadCount = eventArgs.TotalDownloadCount;
            int currentDownloadCount = eventArgs.CurrentDownloadCount;
            long totalDownloadSizeBytes = eventArgs.TotalDownloadSizeBytes;
            long currentDownloadSizeBytes = eventArgs.CurrentDownloadSizeBytes;
            float sliderValue = (float)currentDownloadCount / totalDownloadCount;
            _sangoPatchWnd.UpdateSliderValue(sliderValue);
            string currentSizeMB = (currentDownloadSizeBytes / 1048576f).ToString("f1");
            string totalSizeMB = (totalDownloadSizeBytes / 1048576f).ToString("f1");
            string tips = $"{currentDownloadCount}/{totalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
            _sangoPatchWnd.UpdateTips(tips);
        }
    }
}