using UnityEditor;

namespace XRFramework.Editor
{
    public class EXEExporterMenuItem
    {
        [MenuItem("XRFramework/Export EXE/Generate Export AIP")]
        public static void GenerateExportAIP()
        {
            EXEExporter.GenerateExportAIP();
        }
        [MenuItem("XRFramework/Export EXE/Export EXE Installer")]
        public static void ExportExe()
        {
            EXEExporter.ExportExe();
        }
    }
}
