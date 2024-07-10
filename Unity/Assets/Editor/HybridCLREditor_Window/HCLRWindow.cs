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
        /// ������ʾ����
        /// </summary>
        [MenuItem("LKZ/HybridCLR���ڹ���", priority = 2050)]
        public static void ShowWindow()
        {
            LoadABEditorData();

            var window = GetWindow<HCLRWindow>();
            window.titleContent = new GUIContent("HybridCLR ���ڹ���");
            window.minSize = new Vector2(500, 300);
            window.Show();

        }


        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            //�߼�
            using (new EditorGUI.DisabledScope(false))//�Ƿ���Խ���
            {
                dataStruct.HybridCLRInitializedAdvancedOptions = EditorGUILayout.Foldout(dataStruct.HybridCLRInitializedAdvancedOptions, "HybridCLR��ʼ������");

                if (dataStruct.HybridCLRInitializedAdvancedOptions)
                {
                    GUILayout.BeginHorizontal();

                    var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
                    centeredStyle.alignment = TextAnchor.UpperLeft;
                    centeredStyle.contentOffset = new Vector2(8, 0);

                    GUILayout.Label(new GUIContent("HybridCLR ��ʼ��,�״α���ִ��"), centeredStyle);
                    if (GUILayout.Button("��ʼ��(Installer)", GUILayout.MaxWidth(130f)))
                    {
                        InstallerWindow window = EditorWindow.GetWindow<InstallerWindow>("HybridCLR Installer", true);
                        window.minSize = new Vector2(800f, 500f);
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(20);
                using (new EditorGUI.DisabledScope(false))//�Ƿ���Խ���
                {
                    //�߼� 
                    dataStruct.HybridCLRGenerateAdvancedOptions = EditorGUILayout.Foldout(dataStruct.HybridCLRGenerateAdvancedOptions, "HybridCLR����");

                    if (dataStruct.HybridCLRGenerateAdvancedOptions)
                    {
                        GUILayout.BeginHorizontal();

                        var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
                        centeredStyle.alignment = TextAnchor.UpperLeft;
                        centeredStyle.contentOffset = new Vector2(8, 0);

                        GUILayout.Label(new GUIContent("����IL2CPP����ü�LinkXml�ļ�"), centeredStyle);
                        if (GUILayout.Button("����Link", GUILayout.MaxWidth(150)))
                        {
                            LinkGeneratorCommand.GenerateLinkXml();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("����MethodBridge"), centeredStyle);
                        if (GUILayout.Button("����MethodBridge", GUILayout.MaxWidth(150)))
                        {
                            //MethodBridgeGeneratorCommand.CompileAndGenerateMethodBridge();
                            MethodBridgeGeneratorCommand.GenerateMethodBridgeAndReversePInvokeWrapper();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();


                        GUILayout.Label(new GUIContent("����AOTGenericReference"), centeredStyle);
                        if (GUILayout.Button("����Ref", GUILayout.MaxWidth(150)))
                        {
                            AOTReferenceGeneratorCommand.CompileAndGenerateAOTGenericReference();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("����ReversePInvokeWrapper"), centeredStyle);
                        if (GUILayout.Button("����ReversePInvokeWrapper", GUILayout.MaxWidth(150)))
                        {
                            //ReversePInvokeWrapperGeneratorCommand.CompileAndGenerateReversePInvokeWrapper();
                            MethodBridgeGeneratorCommand.GenerateMethodBridgeAndReversePInvokeWrapper();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("����Il2CppDef"), centeredStyle);
                        if (GUILayout.Button("����CppDef", GUILayout.MaxWidth(150)))
                        {
                            Il2CppDefGeneratorCommand.GenerateIl2CppDef();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("����AOTDlls"), centeredStyle);
                        if (GUILayout.Button("����AOTDlls", GUILayout.MaxWidth(150)))
                        {
                            StripAOTDllCommand.GenerateStripedAOTDlls();
                        }
                        GUILayout.EndHorizontal();
                        EditorGUILayout.Space(10);


                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("����All"), centeredStyle);


                        if (GUILayout.Button("����All", GUILayout.MaxWidth(150)))
                        {
                            PrebuildCommand.GenerateAll();
                        }
                        GUILayout.EndHorizontal();

                        var centeredStyle_t = new GUIStyle(GUI.skin.GetStyle("Label"));
                        centeredStyle_t.fontSize = 10;
                        centeredStyle_t.contentOffset = new Vector2(10, 0);

                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("�������й����л�ִ��һ�ε������̣������ɲü����AOT dll����һ�����ڴ�����Ŀ��˵���ܷǳ���ʱ�����������ʱ��������һ����"), centeredStyle_t);
                        GUILayout.EndHorizontal();

                    }
                }

                EditorGUILayout.Space(20);
                using (new EditorGUI.DisabledScope(false))//�Ƿ���Խ���
                {
                    //�߼� 
                    dataStruct.HybridCLRCompileAdvancedOptions = EditorGUILayout.Foldout(dataStruct.HybridCLRCompileAdvancedOptions, "�����ȸ���dll");

                    if (dataStruct.HybridCLRCompileAdvancedOptions)
                    {
                        GUILayout.BeginHorizontal();

                        var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
                        centeredStyle.alignment = TextAnchor.UpperLeft;
                        centeredStyle.contentOffset = new Vector2(8, 0);

                        GUILayout.Label(new GUIContent("���뵱ǰƽ̨"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllActiveBuildTarget();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();


                        GUILayout.Label(new GUIContent("���뵱ǰƽ̨������"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllActiveBuildTargetDevelopment();
                        }
                        GUILayout.EndHorizontal();

                        EditorGUILayout.Space(20);

                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("����Win32ƽ̨"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllWin32();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("����Win64ƽ̨"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllWin64();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("����MacOSƽ̨"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllMacOS();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("����Linuxƽ̨"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllLinux();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("����Androidƽ̨"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllAndroid();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("����IOSƽ̨"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllIOS();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(new GUIContent("����WebGLƽ̨"), centeredStyle);
                        if (GUILayout.Button("����", GUILayout.MaxWidth(100)))
                        {
                            CompileDllCommand.CompileDllWebGL();
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.Space(20);
                using (new EditorGUI.DisabledScope(false))//�Ƿ���Խ���
                {
                    //�߼� 
                    dataStruct.HybridCLRDllMoveAdvancedOptions = EditorGUILayout.Foldout(dataStruct.HybridCLRDllMoveAdvancedOptions, "�����ȸ���dll");

                    if (dataStruct.HybridCLRDllMoveAdvancedOptions)
                    {
                        GUILayout.BeginHorizontal();
                        dataStruct.CoptHotUpdateDllToPath = EditorGUILayout.TextField(new GUIContent("HybridCLR����������ȸ�Dll������·����"), dataStruct.CoptHotUpdateDllToPath);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("ѡ���ƶ��ȸ�dll·��", GUILayout.MaxWidth(130f)))
                            dataStruct.CoptHotUpdateDllToPath = OpenSelectFilePath("ѡ��AB��Դ�ļ�·��", defaultPath: dataStruct.CoptHotUpdateDllToPath ?? Application.dataPath);

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        dataStruct.CoptAssembliesPostIl2CppStripToPath = EditorGUILayout.TextField(new GUIContent("HybridCLR��Ҫ����Ԫ���ݵ�dll���Ƶ�·����"), dataStruct.CoptAssembliesPostIl2CppStripToPath);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("ѡ���ƶ�����Ԫ����dll·��", GUILayout.MaxWidth(130f)))
                            dataStruct.CoptAssembliesPostIl2CppStripToPath = OpenSelectFilePath("ѡ��AB��Դ�ļ�·��", defaultPath: dataStruct.CoptAssembliesPostIl2CppStripToPath ?? Application.dataPath);



                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        dataStruct.IsChangeDllSuffix = GUILayout.Toggle(
                    dataStruct.IsChangeDllSuffix,
                    new GUIContent("Dll�޸ĺ�׺"));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        if (dataStruct.IsChangeDllSuffix)
                        {
                            dataStruct.AssembliesDllSuffix = EditorGUILayout.TextField(new GUIContent("��׺"), dataStruct.AssembliesDllSuffix);
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("�ƶ�"))
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
        /// ����AB���༭��������
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
        /// ����AB���༭��������
        /// </summary>
        private static void SaveABEditorData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.OpenOrCreate);
            bf.Serialize(file, dataStruct);
            file.Close();
        }


        /// <summary>
        /// ���ļ�ѡ���ļ�·��
        /// </summary>
        /// <param name="title">����</param>
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
        /// ���ļ��������
        /// </summary>
        /// <param name="title">����</param>
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