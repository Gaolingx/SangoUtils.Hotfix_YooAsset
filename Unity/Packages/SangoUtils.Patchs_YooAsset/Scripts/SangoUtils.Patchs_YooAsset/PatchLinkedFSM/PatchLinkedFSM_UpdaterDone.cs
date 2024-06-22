using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections.Generic;
using UnityEngine;
using HybridCLR;
using YooAsset;
using System.Linq;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchLinkedFSM_UpdaterDone : FSMLinkedStaterItemBase
    {
        internal override void OnEnter()
        {
            EnterSangoGameRoot();
        }

        #region EnterGameRoot
        internal void EnterSangoGameRoot()
        {
            var HotDllName = (string)_fsmLinkedStater.GetBlackboardValue("HotUpdateDllName");
            var AOTMetaAssemblyNames = (List<string>)_fsmLinkedStater.GetBlackboardValue("AOTMetaAssemblyNames");

            LoadDll(AOTMetaAssemblyNames, HotDllName);
            LoadMetadataForAOTAssemblies(AOTMetaAssemblyNames);

#if !UNITY_EDITOR
        System.Reflection.Assembly.Load(GetAssetData(HotDllName));
#endif
            LoadGameRootObject();
        }

        internal void LoadGameRootObject()
        {
            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var GameRootObject = (string)_fsmLinkedStater.GetBlackboardValue("GameRootObjectName");
            var asset1 = package.LoadAssetSync<GameObject>(GameRootObject);
            var CanvasTransform = GameObject.Find((string)_fsmLinkedStater.GetBlackboardValue("GameRootParentTransformName")).transform;
            GameObject hotFixRoot = asset1.InstantiateSync();
            hotFixRoot.transform.SetParent(CanvasTransform, false);
            RectTransform rect = hotFixRoot.GetComponent<RectTransform>();
            rect.offsetMax = new Vector2(0, 0);
        }
        #endregion


        //获取资源二进制
        private static Dictionary<string, byte[]> _dllAssetDataDict = new Dictionary<string, byte[]>();
        private static byte[] GetAssetData(string dllName)
        {
            return _dllAssetDataDict[dllName];
        }

        #region LoadAssemblies
        internal void LoadDll(List<string> AOTMetaAssemblyNames, string HotDllName)
        {
            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");

            var dllNameList = new List<string>() { HotDllName, }.Concat(AOTMetaAssemblyNames);
            foreach (var dllName in dllNameList)
            {
                byte[] fileData = AssetService.Instance.LoadTextAsset(packageName, dllName, true).bytes;
                _dllAssetDataDict.Add(dllName, fileData);
                Debug.Log($"dll:{dllName}  size:{fileData.Length}");
            }
        }

        /// <summary>
        /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
        /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
        /// </summary>
        internal static void LoadMetadataForAOTAssemblies(List<string> AOTMetaAssemblyNames)
        {
            // 可以加载任意aot assembly的对应的dll。但要求dll必须与unity build过程中生成的裁剪后的dll一致，而不能直接使用原始dll。
            // 我们在BuildProcessors里添加了处理代码，这些裁剪后的dll在打包时自动被复制到 {项目目录}/HybridCLRData/AssembliesPostIl2CppStrip/{Target} 目录。

            /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
            /// 
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AOTMetaAssemblyNames)
            {
                byte[] dllBytes = GetAssetData(aotDllName);
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} return:{err}");
            }
        }
        #endregion
    }
}