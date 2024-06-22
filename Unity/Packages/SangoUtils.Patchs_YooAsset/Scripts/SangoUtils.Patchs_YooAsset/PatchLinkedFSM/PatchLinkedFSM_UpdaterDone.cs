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


        //��ȡ��Դ������
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
        #endregion
    }
}