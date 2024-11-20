using HybridCLR;
using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_UpdateLoadHotfixDll : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.LoadHotfixDlls;

        internal override void OnEvent()
        {
            EnterSangoGameRoot();
            EventBus_Patchs.PatchOperation.SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.UpdaterDone));
        }

        #region EnterGameRoot
        internal void EnterSangoGameRoot()
        {
            var HotDllList = EventBus_Patchs.PatchOperation.PatchOperationData.PatchConfig.HotUpdateDllList;
            var AOTMetaAssemblyNames = EventBus_Patchs.PatchOperation.PatchOperationData.PatchConfig.AOTMetaAssemblyNames;

            LoadDll(AOTMetaAssemblyNames, HotDllList);
            LoadMetadataForAOTAssemblies(AOTMetaAssemblyNames);

#if !UNITY_EDITOR
            LoadHotAssembly(HotDllList);
#endif
        }

        #endregion


        //获取资源二进制
        private static Dictionary<string, byte[]> _dllAssetDataDict = new Dictionary<string, byte[]>();
        private static byte[] GetAssetData(string dllName)
        {
            return _dllAssetDataDict[dllName];
        }

        #region LoadAssemblies
        internal void LoadDll(List<string> AOTMetaAssemblyNames, List<string> HotDllList)
        {
            var packageName = EventBus_Patchs.PatchOperation.PatchOperationData.PatchConfig.PackageName;

            var dllNameList = new List<string>().Concat(AOTMetaAssemblyNames).Concat(HotDllList);
            foreach (var dllName in dllNameList)
            {
                byte[] fileData = SangoAssetService.Instance.LoadTextAsset(packageName, dllName, true).bytes;
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

        internal static void LoadHotAssembly(List<string> HotDllList)
        {
            for (int i = 0; i < HotDllList.Count; i++)
            {
                System.Reflection.Assembly.Load(GetAssetData(HotDllList[i]));
            }
        }

        #endregion
    }
}