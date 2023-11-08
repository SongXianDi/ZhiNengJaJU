using UnityEngine;
using System.IO;
using UnityEditor;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

namespace XRFramework.Editor
{
    public class EXEExporter
    {
        public static ExportExeConfig config;
        public static string exportName;
        public static void GenerateExportAIP()
        {
            config = Resources.Load<ExportExeConfig>("XRFrameworkConfig/ExportExeConfig");
            var template = config.template;
            
            //var defaultBuildPath = Path.GetDirectoryName(EditorUserBuildSettings.GetBuildLocation(BuildTarget.StandaloneWindows64));
            var buildPath = string.IsNullOrEmpty(config.BuildPath) ? EditorUtility.OpenFolderPanel("Select Build Path", 
            "", "") : config.BuildPath;

            string aipSavePath = string.IsNullOrEmpty(config.ExportFullName) ? EditorUtility.SaveFilePanel("Save Export AIP", "", "exportName", "aip") : config.ExportFullName;
            if (string.IsNullOrEmpty(aipSavePath)||string.IsNullOrEmpty(buildPath))
            {
                EditorUtility.DisplayDialog("Export AIP", "Export AIP Failed, Please Check Your Config", "OK");
                return;
            }

            var finalExportPath = string.IsNullOrEmpty(config.FinalExportPath) ? EditorUtility.OpenFolderPanel("Select Final Export Path", "", "") : config.FinalExportPath;
            if (string.IsNullOrEmpty(finalExportPath))
            {
                EditorUtility.DisplayDialog("Export AIP", "Export AIP Failed, Please Check Your Config", "OK");
                return;
            }
            
            FileInfo fileInfo = new FileInfo(aipSavePath);
            exportName = string.IsNullOrEmpty(config.ExportName) ? fileInfo.Name.Replace(fileInfo.Extension, "") : config.ExportName;

            string content = template.text;
            content = content.Replace("%ProductNamePlaceHolder%", PlayerSettings.productName);
            content = content.Replace("%CompanyName%", PlayerSettings.companyName);
            content = content.Replace("%Version%", PlayerSettings.bundleVersion);
            content = content.Replace("%ExportName%", exportName);
            content = content.Replace("%BuildPath%", buildPath);
            content = content.Replace("%FinalExportPath%", finalExportPath);
            File.WriteAllText(aipSavePath, content);

            Debug.Log("Export AIP Success");
            if(EditorUtility.DisplayDialog("Export AIP", "Export AIP Success", "开始生成EXE安装文件", "稍后再说"))
            {
                ExportExe();
            }
        }
        public static void ExportExe()
        {
            if(string.IsNullOrEmpty(config.AdvancedInstallerPath))
            {
                EditorUtility.DisplayDialog("Export EXE", "Export EXE Failed, AdvancedInstallerPath is Empty", "OK");
                return;
            }
            Process.Start("cmd.exe", "/k " + "c:" + " & " + "cd " + config.AdvancedInstallerPath + " & " + "AdvancedInstaller.com /build " 
            + config.ExportFullName + " -buildslist \"DefaultBuild\"");
        }
    }
}
