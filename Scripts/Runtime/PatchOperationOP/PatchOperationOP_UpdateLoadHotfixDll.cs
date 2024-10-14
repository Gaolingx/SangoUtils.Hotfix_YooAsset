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
            var HotDllList = EventBus_Patchs.PatchOperation.PatchOperationData.HotUpdateDllList;
            var AOTMetaAssemblyNames = EventBus_Patchs.PatchOperation.PatchOperationData.AOTMetaAssemblyNames;

            LoadDll(AOTMetaAssemblyNames, HotDllList);
            LoadMetadataForAOTAssemblies(AOTMetaAssemblyNames);

#if !UNITY_EDITOR
            LoadHotAssembly(HotDllList);
#endif
        }

        #endregion


        //��ȡ��Դ������
        private static Dictionary<string, byte[]> _dllAssetDataDict = new Dictionary<string, byte[]>();
        private static byte[] GetAssetData(string dllName)
        {
            return _dllAssetDataDict[dllName];
        }

        #region LoadAssemblies
        internal void LoadDll(List<string> AOTMetaAssemblyNames, List<string> HotDllList)
        {
            var packageName = EventBus_Patchs.PatchOperation.PatchOperationData.PackageName;

            var dllNameList = new List<string>().Concat(AOTMetaAssemblyNames).Concat(HotDllList);
            foreach (var dllName in dllNameList)
            {
                byte[] fileData = SangoAssetService.Instance.LoadTextAsset(packageName, dllName, true).bytes;
                _dllAssetDataDict.Add(dllName, fileData);
                Debug.Log($"dll:{dllName}  size:{fileData.Length}");
            }
        }

        /// <summary>
        /// Ϊaot assembly����ԭʼmetadata�� ��������aot�����ȸ��¶��С�
        /// һ�����غ����AOT���ͺ�����Ӧnativeʵ�ֲ����ڣ����Զ��滻Ϊ����ģʽִ��
        /// </summary>
        internal static void LoadMetadataForAOTAssemblies(List<string> AOTMetaAssemblyNames)
        {
            // ���Լ�������aot assembly�Ķ�Ӧ��dll����Ҫ��dll������unity build���������ɵĲü����dllһ�£�������ֱ��ʹ��ԭʼdll��
            // ������BuildProcessors������˴�����룬��Щ�ü����dll�ڴ��ʱ�Զ������Ƶ� {��ĿĿ¼}/HybridCLRData/AssembliesPostIl2CppStrip/{Target} Ŀ¼��

            /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
            /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
            /// 
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AOTMetaAssemblyNames)
            {
                byte[] dllBytes = GetAssetData(aotDllName);
                // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
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