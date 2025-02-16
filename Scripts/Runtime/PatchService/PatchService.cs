using SangoUtils.Patchs_YooAsset.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
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

        public UnityWebRequest CustomWebRequester(string url)
        {
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            if (_currentPatchConfig.SkipCertificate)
            {
                request.certificateHandler = new WebRequestSkipCertificate();
            }
            return request;
        }

        /// <summary>
        /// 跳过Web请求证书
        /// </summary>
        public class WebRequestSkipCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        private IEnumerator StartOperation()
        {
            YooAssets.Initialize();

            YooAssets.SetDownloadSystemUnityWebRequest(CustomWebRequester);

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