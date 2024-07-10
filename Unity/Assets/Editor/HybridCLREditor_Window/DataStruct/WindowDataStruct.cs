using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LKZ.HybridCLREditor.DataStruct
{
    [Serializable]
    internal sealed class WindowDataStruct
    {
        /// <summary>
        /// 复制热更的dll到那个路径下
        /// </summary>
        public string CoptHotUpdateDllToPath { get; set; } = Application.streamingAssetsPath+ "/HotUpdateDll";

        /// <summary>
        /// 修改程序集dll后缀名
        /// </summary>
        public string AssembliesDllSuffix { get; set; } = ".bin";


        public bool IsChangeDllSuffix { get; set; } = true;

        /// <summary>
        /// 复制AOT元数据补充的dll到其他路径下
        /// </summary>
        public string CoptAssembliesPostIl2CppStripToPath { get; set; } =
        Application.streamingAssetsPath + "/AssembliesPostIl2CppStripDll";

         
        public bool HybridCLRInitializedAdvancedOptions { get; set; } = false;
        public bool HybridCLRGenerateAdvancedOptions { get; set; } = false;
        public bool HybridCLRCompileAdvancedOptions { get; set; } = false;
        public bool HybridCLRDllMoveAdvancedOptions { get; set; } = false;
    }
}
