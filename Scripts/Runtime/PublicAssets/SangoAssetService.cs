using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    public class SangoAssetService : MonoBehaviour
    {
        private static SangoAssetService _instance;

        public static SangoAssetService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(SangoAssetService)) as SangoAssetService;
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject("[" + typeof(SangoAssetService).FullName + "]");
                        _instance = gameObject.AddComponent<SangoAssetService>();
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

        private List<AssetHandle> _cacheAssetHandles = new();

        private Dictionary<string, AssetHandle> _audioClipHandleDict = new();
        private Dictionary<string, AssetHandle> _textAssetHandleDict = new();
        private Dictionary<string, AssetHandle> _videoClipHandleDict = new();
        private Dictionary<string, AssetHandle> _spriteHandleDict = new();
        private Dictionary<string, AssetHandle> _prefabHandleDict = new();

        public void Initialize()
        {
            _audioClipHandleDict.Clear();
            _textAssetHandleDict.Clear();
            _videoClipHandleDict.Clear();
            _spriteHandleDict.Clear();
            _prefabHandleDict.Clear();
        }

        public AudioClip LoadAudioClip(string packageName, string audioClipPath, bool isCache)
        {
            AudioClip audioClip = null;
            _audioClipHandleDict.TryGetValue(audioClipPath, out AssetHandle handle);
            if (handle == null)
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadAssetSync<AudioClip>(audioClipPath);
                audioClip = handle.AssetObject as AudioClip;
                if (isCache)
                {
                    if (!_audioClipHandleDict.ContainsKey(audioClipPath))
                    {
                        _audioClipHandleDict.Add(audioClipPath, handle);
                    }
                }
                else
                {
                    GCAssetHandleTODO(handle);
                }
            }
            else
            {
                audioClip = handle.AssetObject as AudioClip;
            }
            return audioClip;
        }

        public TextAsset LoadTextAsset(string packageName, string textAssetPath, bool isCache)
        {
            TextAsset textAsset = null;
            _textAssetHandleDict.TryGetValue(textAssetPath, out AssetHandle handle);
            if (handle == null)
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadAssetSync<TextAsset>(textAssetPath);
                textAsset = handle.AssetObject as TextAsset;
                if (isCache)
                {
                    if (!_textAssetHandleDict.ContainsKey(textAssetPath))
                    {
                        _textAssetHandleDict.Add(textAssetPath, handle);
                    }
                }
                else
                {
                    GCAssetHandleTODO(handle);
                }
            }
            else
            {
                textAsset = handle.AssetObject as TextAsset;
            }
            return textAsset;
        }

        public void LoadAudioClipASync(string packageName, string audioClipPath, Action<AudioClip> assetLoadedCallBack, bool isCache)
        {
            AudioClip audioClip = null;
            _audioClipHandleDict.TryGetValue(audioClipPath, out AssetHandle handle);
            if (handle == null)
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadAssetAsync<AudioClip>(audioClipPath);
                handle.Completed += (handle) =>
                {
                    audioClip = handle.AssetObject as AudioClip;
                    assetLoadedCallBack?.Invoke(audioClip);
                    if (isCache)
                    {
                        if (!_audioClipHandleDict.ContainsKey(audioClipPath))
                        {
                            _audioClipHandleDict.Add(audioClipPath, handle);
                        }
                    }
                    else
                    {
                        GCAssetHandleTODO(handle);
                    }
                };
            }
            else
            {
                audioClip = handle.AssetObject as AudioClip;
                assetLoadedCallBack?.Invoke(audioClip);
            }
        }

        public VideoClip LoadVideoClip(string packageName, string videoClipPath, bool isCache)
        {
            VideoClip videoClip = null;
            _videoClipHandleDict.TryGetValue(videoClipPath, out AssetHandle handle);
            if (handle == null)
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadAssetSync<VideoClip>(videoClipPath);
                videoClip = handle.AssetObject as VideoClip;
                if (isCache)
                {
                    if (!_videoClipHandleDict.ContainsKey(videoClipPath))
                    {
                        _videoClipHandleDict.Add(videoClipPath, handle);
                    }
                }
                else
                {
                    GCAssetHandleTODO(handle);
                }
            }
            else
            {
                videoClip = handle.AssetObject as VideoClip;
            }
            return videoClip;
        }

        public void LoadVideoClipASync(string packageName, string videoClipPath, Action<VideoClip> assetLoadedCallBack, bool isCache)
        {
            VideoClip videoClip = null;
            _videoClipHandleDict.TryGetValue(videoClipPath, out AssetHandle handle);
            if (handle == null)
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadAssetAsync<VideoClip>(videoClipPath);
                handle.Completed += (handle) =>
                {
                    videoClip = handle.AssetObject as VideoClip;
                    assetLoadedCallBack?.Invoke(videoClip);
                    if (isCache)
                    {
                        if (!_videoClipHandleDict.ContainsKey(videoClipPath))
                        {
                            _videoClipHandleDict.Add(videoClipPath, handle);
                        }
                    }
                    else
                    {
                        GCAssetHandleTODO(handle);
                    }
                };
            }
            else
            {
                videoClip = handle.AssetObject as VideoClip;
                assetLoadedCallBack?.Invoke(videoClip);
            }
        }

        public GameObject LoadPrefab(string packageName, string prefabPath, bool isCache)
        {
            GameObject prefab = null;
            _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
            if (handle == null)
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadAssetSync<GameObject>(prefabPath);
                prefab = handle.AssetObject as GameObject;
                if (isCache)
                {
                    if (!_prefabHandleDict.ContainsKey(prefabPath))
                    {
                        _prefabHandleDict.Add(prefabPath, handle);
                    }
                }
                else
                {
                    GCAssetHandleTODO(handle);
                }
            }
            else
            {
                prefab = handle.AssetObject as GameObject;
            }
            return prefab;
        }

        public void LoadPrefabASync(string packageName, string prefabPath, Action<GameObject> assetLoadedCallBack, bool isCache)
        {
            GameObject prefab = null;
            _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
            if (handle == null)
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadAssetAsync<GameObject>(prefabPath);
                handle.Completed += (handle) =>
                {
                    prefab = handle.AssetObject as GameObject;
                    assetLoadedCallBack?.Invoke(prefab);
                    if (isCache)
                    {
                        if (!_prefabHandleDict.ContainsKey(prefabPath))
                        {
                            _prefabHandleDict.Add(prefabPath, handle);
                        }
                    }
                    else
                    {
                        GCAssetHandleTODO(handle);
                    }
                };
            }
            else
            {
                prefab = handle.AssetObject as GameObject;
                assetLoadedCallBack?.Invoke(prefab);
            }
        }

        public GameObject InstantiatePrefab(string packageName, Transform parentTrans, string prefabPath, bool isCache)
        {
            GameObject prefab = LoadPrefab(packageName, prefabPath, isCache);
            GameObject instantiatedPrefab = Instantiate(prefab, parentTrans);
            return instantiatedPrefab;
        }

        public void InstantiatePrefabASync(string packageName, Transform parentTrans, string prefabPath, Action<GameObject> assetLoadedCallBack, bool isCache)
        {
            GameObject prefab = null;
            _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
            if (handle == null)
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadAssetAsync<GameObject>(prefabPath);
                handle.Completed += (handle) =>
                {
                    prefab = handle.InstantiateSync(parentTrans);
                    assetLoadedCallBack?.Invoke(prefab);
                    if (isCache)
                    {
                        if (!_prefabHandleDict.ContainsKey(prefabPath))
                        {
                            _prefabHandleDict.Add(prefabPath, handle);
                        }
                    }
                    else
                    {
                        GCAssetHandleTODO(handle);
                    }
                };
            }
            else
            {
                prefab = handle.AssetObject as GameObject;
                assetLoadedCallBack?.Invoke(prefab);
            }
        }

        private void GCAssetHandleTODO(AssetHandle assetHandle)
        {
            _cacheAssetHandles.Add(assetHandle);
        }

        public void ReleaseAssetHandles()
        {
            for (int i = 0; i < _cacheAssetHandles.Count; i++)
            {
                _cacheAssetHandles[i].Release();
            }
            _cacheAssetHandles.Clear();
        }

        #region UnloadAsset
        // 卸载所有引用计数为零的资源包。
        // 可以在切换场景之后调用资源释放方法或者写定时器间隔时间去释放。
        public void UnloadUnusedAssets(string packageName)
        {
            var package = YooAssets.GetPackage(packageName);
            package.UnloadUnusedAssets();
        }

        // 尝试卸载指定的资源对象
        // 注意：如果该资源还在被使用，该方法会无效。
        public void TryUnloadUnusedAsset(string packageName, string path)
        {
            var package = YooAssets.GetPackage(packageName);
            package.TryUnloadUnusedAsset(path);
        }

        // 强制卸载所有资源包，该方法请在合适的时机调用。
        // 注意：Package在销毁的时候也会自动调用该方法。
        public void ForceUnloadAllAssets(string packageName)
        {
            var package = YooAssets.GetPackage(packageName);
            package.ForceUnloadAllAssets();
        }

        #endregion
    }
}