using SangoUtils.Patchs_YooAsset.Utils;
using System;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchService : MonoBehaviour
    {
        private static PatchService _instance;

        internal static PatchService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(PatchService)) as PatchService;
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject("[" + typeof(PatchService).FullName + "]");
                        _instance = gameObject.AddComponent<PatchService>();
                        gameObject.hideFlags = HideFlags.HideInHierarchy;
                        DontDestroyOnLoad(gameObject);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (null != _instance && _instance != this)
            {
                Destroy(gameObject);
            }
        }

        private SangoPatchConfig _currentPatchConfig;

        public Action<bool> OnPatchCompleted { get; set; }

        internal void Initialize(SangoPatchConfig patchConfig)
        {
            _currentPatchConfig = patchConfig;
            StartOperation().Start();
        }

        private IEnumerator StartOperation()
        {
            YooAssets.Initialize();

            PatchOperation hotFixOperation = new PatchOperation(_currentPatchConfig);
            YooAssets.StartOperation(hotFixOperation);
            yield return hotFixOperation;

            ResourcePackage assetPackage = YooAssets.GetPackage(_currentPatchConfig.PackageName);
            YooAssets.SetDefaultPackage(assetPackage);

            EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.ClosePatchWindow));
            OnPatchCompleted?.Invoke(true);
        }
    }
}