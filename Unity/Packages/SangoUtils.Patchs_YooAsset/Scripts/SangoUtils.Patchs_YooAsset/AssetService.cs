using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    public class AssetService : MonoBehaviour
    {
        private static AssetService _instance;

        public static AssetService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(AssetService)) as AssetService;
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject("[" + typeof(AssetService).FullName + "]");
                        _instance = gameObject.AddComponent<AssetService>();
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
        private Dictionary<string, AssetHandle> _videoClipHandleDict = new();
        private Dictionary<string, AssetHandle> _spriteHandleDict = new();
        private Dictionary<string, AssetHandle> _prefabHandleDict = new();

        public void Initialize()
        {

        }

        public AudioClip LoadAudioClip(string audioClipPath, bool isCache)
        {
            AudioClip audioClip = null;
            _audioClipHandleDict.TryGetValue(audioClipPath, out AssetHandle handle);
            if (handle == null)
            {
                handle = YooAssets.LoadAssetSync<AudioClip>(audioClipPath);
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

        public void LoadAudioClipASync(string audioClipPath, Action<AudioClip> assetLoadedCallBack, bool isCache)
        {
            AudioClip audioClip = null;
            _audioClipHandleDict.TryGetValue(audioClipPath, out AssetHandle handle);
            if (handle == null)
            {
                handle = YooAssets.LoadAssetAsync<AudioClip>(audioClipPath);
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

        public VideoClip LoadVideoClip(string videoClipPath, bool isCache)
        {
            VideoClip videoClip = null;
            _videoClipHandleDict.TryGetValue(videoClipPath, out AssetHandle handle);
            if (handle == null)
            {
                handle = YooAssets.LoadAssetSync<VideoClip>(videoClipPath);
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

        public void LoadVideoClipASync(string videoClipPath, Action<VideoClip> assetLoadedCallBack, bool isCache)
        {
            VideoClip videoClip = null;
            _videoClipHandleDict.TryGetValue(videoClipPath, out AssetHandle handle);
            if (handle == null)
            {
                handle = YooAssets.LoadAssetAsync<VideoClip>(videoClipPath);
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

        public GameObject LoadPrefab(string prefabPath, bool isCache)
        {
            GameObject prefab = null;
            _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
            if (handle == null)
            {
                handle = YooAssets.LoadAssetSync<GameObject>(prefabPath);
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

        public void LoadPrefabASync(string prefabPath, Action<GameObject> assetLoadedCallBack, bool isCache)
        {
            GameObject prefab = null;
            _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
            if (handle == null)
            {
                handle = YooAssets.LoadAssetAsync<GameObject>(prefabPath);
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

        public GameObject InstantiatePrefab(Transform parentTrans, string prefabPath, bool isCache)
        {
            GameObject prefab = LoadPrefab(prefabPath, isCache);
            GameObject instantiatedPrefab = Instantiate(prefab, parentTrans);
            return instantiatedPrefab;
        }

        public void InstantiatePrefabASync(Transform parentTrans, string prefabPath, Action<GameObject> assetLoadedCallBack, bool isCache)
        {
            GameObject prefab = null;
            _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
            if (handle == null)
            {
                handle = YooAssets.LoadAssetAsync<GameObject>(prefabPath);
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
    }
}