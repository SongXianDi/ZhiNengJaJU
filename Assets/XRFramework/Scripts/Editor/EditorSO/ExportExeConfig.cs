using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace XRFramework.Editor
{
    [CreateAssetMenu(fileName = "ExportExeConfig", menuName = "XRFramework/ExportExeConfig")]
    public class ExportExeConfig : ScriptableObject
    {
        public string AdvancedInstallerPath;
        public string BuildPath;
        public string ExportPath;
        public string ExportName;
        public string FinalExportPath;
        private bool overrideTemplate;
        public TextAsset template;

        public string ExportFullName => string.IsNullOrEmpty(ExportName) || string.IsNullOrEmpty(ExportPath) ? null : ExportPath + "/" + ExportName + ".aip";
    }
}
