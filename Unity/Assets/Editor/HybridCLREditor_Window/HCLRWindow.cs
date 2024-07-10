using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using HybridCLR.Editor.Installer;
using LKZ.HybridCLREditor.DataStruct;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace LKZ.HybridCLREditor
{
    public sealed class HCLRWindow : EditorWindow
    {
        private static WindowDataStruct dataStruct;
        private static string dataPath;

        /// <summary>
        /// 按下显示窗口
        /// </summary>
        [MenuItem("LKZ/HybridCLR窗口工具", priority = 2050)]
        public static void ShowWindow()
        {
            LoadABEditorData();

            var window = GetWindow<HCLRWindow>();
            window.titleContent = new GUIContent("HybridCLR 窗口工具");
            window.minSize = new Vector2(500, 300);
            window.Show();

        }


        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            //高级
            using (new EditorGUI.DisabledScope(false))//是否可以禁用
            {
                dataStruct.HybridCLRInitializedAdvancedOptions = EditorGUILayout.Foldout(dataStruct.HybridCLRInitializedAdvancedOptions, "HybridCLR初始化配置");

                if (dataStruct.HybridCLRInitializedAdvancedOptions)
                {
                    GUILayout.BeginHorizontal();

                    var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
                    centeredStyle.alignment = TextAnchor.UpperLeft;
                    centeredStyle.contentOffset = new Vector2(8, 0);

                    GUILayout.Label(new GUIContent("HybridCLR 初始化,首次必须执行"), centeredStyle);
                    if (GUILayout.Button("初始化(Installer)", GUILayout.MaxWidth(130f)))
                    {
                        InstallerWindow window = EditorWindow.GetWindow<InstallerWindow>("HybridCLR Installer", true);
                        window.minSize = new Vector2(800f, 500f);
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(20);
                using (new EditorGUI.DisabledScope(false))//是否可以禁用
                {
                    //高级 
                    dataStruct.HybridCLRGenerateAdvancedOptions = EditorGUILayout.Foldout(dataStruct.HybridCLRGenerateAdvancedOptions, "HybridCLR生成");

                    if (dataStruct.HybridCLRGenerateAdvancedOptions)
                    {
                        GUILayout.BeginHorizontal();

                        var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
                        centeredStyle.alignment = TextAnchor.UpperLeft;
                        centeredStyle.contentOffset = new Vector2(8, 0);

                        GUILayout.Label(new GUIContent("生成IL2CPP避免裁剪LinkXml文件"), centeredStyle);
                        if (GUILayout.Button("生成Link", GUILayout.MaxWidth(150)))
                        {
                            LinkGeneratorCommand.GenerateLinkXml();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("生成MethodBridge"), centeredStyle);
                        if (GUILayout.Button("生成MethodBridge", GUILayout.MaxWidth(150)))
                        {
                            //MethodBridgeGeneratorCommand.CompileAndGenerateMethodBridge();
                            MethodBridgeGeneratorCommand.GenerateMethodBridgeAndReversePInvokeWrapper();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();


                        GUILayout.Label(new GUIContent("生成AOTGenericReference"), centeredStyle);
                        if (GUILayout.Button("生成Ref", GUILayout.MaxWidth(150)))
                        {
                            AOTReferenceGeneratorCommand.CompileAndGenerateAOTGenericReference();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("生成ReversePInvokeWrapper"), centeredStyle);
                        if (GUILayout.Button("生成ReversePInvokeWrapper", GUILayout.MaxWidth(150)))
                        {
                            //ReversePInvokeWrapperGeneratorCommand.CompileAndGenerateReversePInvokeWrapper();
                            MethodBridgeGeneratorCommand.GenerateMethodBridgeAndReversePInvokeWrapper();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("生成Il2CppDef"), centeredStyle);
                        if (GUILayout.Button("生成CppDef", GUILayout.MaxWidth(150)))
                        {
                            Il2CppDefGeneratorCommand.GenerateIl2CppDef();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("生成AOTDlls"), centeredStyle);
                        if (GUILayout.Button("生成AOTDlls", GUILayout.MaxWidth(150)))
                        {
                            StripAOTDllCommand.GenerateStripedAOTDlls();
                        }
                        GUILayout.EndHorizontal();
                        EditorGUILayout.Space(10);


                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("生成All"), centeredStyle);


                        if (GUILayout.Button("生成All", GUILayout.MaxWidth(150)))
                        {
                            PrebuildCommand.GenerateAll();
                        }
                        GUILayout.EndHorizontal();

                        var centeredStyle_t = new GUIStyle(GUI.skin.GetStyle("Label"));
                        centeredStyle_t.fontSize = 10;
                        centeredStyle_t.contentOffset = new Vector2(10, 0);

                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("命令运行过程中会执行一次导出工程，以生成裁剪后的AOT dll。这一步对于大型项目来说可能非常耗时，几乎将打包时间增加了一倍。"), centeredStyle_t);
                        GUILayout.EndHorizontal();

                    }
                }

                EditorGUILayout.Space(20);
                using (new EditorGUI.DisabledScope(false))//是否可以禁用
                {
                    //高级 
                    dataStruct.HybridCLRCompileAdvancedOptions = EditorGUILayout.Foldout(dataStruct.HybridCLRCompileAdvancedOptions, "编译热更的dll");

                    if (dataStruct.HybridCLRCompileAdvancedOptions)
                    {
                        GUILayout.BeginHorizontal();

                        var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
                        centeredStyle.alignment = TextAnchor.UpperLeft;
                        centeredStyle.contentOffset = new Vector2(8, 0);

                        GUILayout.Label(new GUIContent("编译当前平台"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllActiveBuildTarget();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();


                        GUILayout.Label(new GUIContent("编译当前平台开发包"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllActiveBuildTargetDevelopment();
                        }
                        GUILayout.EndHorizontal();

                        EditorGUILayout.Space(20);

                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("编译Win32平台"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllWin32();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("编译Win64平台"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllWin64();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("编译MacOS平台"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllMacOS();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("编译Linux平台"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllLinux();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("编译Android平台"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllAndroid();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("编译IOS平台"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllIOS();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("编译WebGL平台"), centeredStyle);
                        if (GUILayout.Button("编译", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllWebGL();
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.Space(20);
                using (new EditorGUI.DisabledScope(false))//是否可以禁用
                {
                    //高级 
                    dataStruct.HybridCLRDllMoveAdvancedOptions = EditorGUILayout.Foldout(dataStruct.HybridCLRDllMoveAdvancedOptions, "编译热更的dll");

                    if (dataStruct.HybridCLRDllMoveAdvancedOptions)
                    {
                        GUILayout.BeginHorizontal();
                        dataStruct.CoptHotUpdateDllToPath = EditorGUILayout.TextField(new GUIContent("HybridCLR编译出来的热更Dll复制在路径下"), dataStruct.CoptHotUpdateDllToPath);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("选择移动热更dll路径", GUILayout.MaxWidth(130f)))
                            dataStruct.CoptHotUpdateDllToPath = OpenSelectFilePath("选择AB资源文件路径", defaultPath: dataStruct.CoptHotUpdateDllToPath ?? Application.dataPath);

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        dataStruct.CoptAssembliesPostIl2CppStripToPath = EditorGUILayout.TextField(new GUIContent("HybridCLR需要补充元数据的dll复制到路径下"), dataStruct.CoptAssembliesPostIl2CppStripToPath);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("选择移动补充元数据dll路径", GUILayout.MaxWidth(130f)))
                            dataStruct.CoptAssembliesPostIl2CppStripToPath = OpenSelectFilePath("选择AB资源文件路径", defaultPath: dataStruct.CoptAssembliesPostIl2CppStripToPath ?? Application.dataPath);



                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        dataStruct.IsChangeDllSuffix = GUILayout.Toggle(
                    dataStruct.IsChangeDllSuffix,
                    new GUIContent("Dll修改后缀"));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        if (dataStruct.IsChangeDllSuffix)
                        {
                            dataStruct.AssembliesDllSuffix = EditorGUILayout.TextField(new GUIContent("后缀"), dataStruct.AssembliesDllSuffix);
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("移动"))
                        {
                            if (!Directory.Exists(dataStruct.CoptHotUpdateDllToPath))
                                Directory.CreateDirectory(dataStruct.CoptHotUpdateDllToPath);


                            foreach (var item in Directory.GetFiles(SettingsUtil.HotUpdateDllsRootOutputDir, "*.dll", SearchOption.AllDirectories))
                            {
                                var path_temp = $"{dataStruct.CoptHotUpdateDllToPath}/{Path.GetFileName(item)}";
                                path_temp += dataStruct.IsChangeDllSuffix ? dataStruct.AssembliesDllSuffix : "";

                                try
                                {
                                    File.Copy(item, path_temp); 
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex);
                                }
                            }

                            if (!Directory.Exists(dataStruct.CoptAssembliesPostIl2CppStripToPath))
                                Directory.CreateDirectory(dataStruct.CoptAssembliesPostIl2CppStripToPath);
                            foreach (var item in Directory.GetFiles(SettingsUtil.AssembliesPostIl2CppStripDir, "*.dll", SearchOption.AllDirectories))
                            {
                                var path_temp = $"{dataStruct.CoptAssembliesPostIl2CppStripToPath}/{Path.GetFileName(item)}";
                                path_temp += dataStruct.IsChangeDllSuffix ? dataStruct.AssembliesDllSuffix : "";

                                try
                                {
                                    File.Copy(item, path_temp);

                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex);
                                }
                            } 
                            AssetDatabase.Refresh(); 
                        }

                    }
                }
            }
        }

        private void OnDestroy()
        {
            SaveABEditorData();
        }

        /// <summary>
        /// 加载AB包编辑器的数据
        /// </summary>
        private static void LoadABEditorData()
        {
            dataPath = System.IO.Path.GetFullPath(".");
            dataPath = dataPath.Replace("\\", "/");
            dataPath = dataPath + "/Library/HybridCLREditorWindowData.dat";

            try
            {
                if (File.Exists(dataPath))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    using (FileStream file = File.Open(dataPath, FileMode.Open))
                    {
                        dataStruct = bf.Deserialize(file) as WindowDataStruct;
                    }
                }
                else
                {
                    dataStruct = new WindowDataStruct();
                }
            }
            catch { dataStruct = new WindowDataStruct(); }
        }

        /// <summary>
        /// 保存AB包编辑器的数据
        /// </summary>
        private static void SaveABEditorData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.OpenOrCreate);
            bf.Serialize(file, dataStruct);
            file.Close();
        }


        /// <summary>
        /// 打开文件选择文件路径
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public static string OpenSelectFilePath(string title, string defaultPath = @"C:\")
        {
            var newPath = EditorUtility.OpenFolderPanel(title, defaultPath, string.Empty);
            if (!string.IsNullOrEmpty(newPath))
            {
                var gamePath = System.IO.Path.GetFullPath(".");
                gamePath = gamePath.Replace("\\", "/");
                if (newPath.StartsWith(gamePath) && newPath.Length > gamePath.Length)
                    newPath = newPath.Remove(0, gamePath.Length + 1);
                return newPath;
            }
            return defaultPath;
        }

        /// <summary>
        /// 打开文件保存面板
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public static string SaveFilePanel(string title, string defaultPath = @"C:\", string defaultName = "Name", string defaultextension = "xml")
        {
            var newPath = EditorUtility.SaveFilePanel(title, defaultPath, defaultName, defaultextension);
            if (!string.IsNullOrEmpty(newPath))
            {
                return newPath;
            }
            return defaultPath;
        }
    }
}