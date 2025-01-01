#if ON_VRCWORLD
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDK3.Editor;

namespace GuraRil.LinuxVrcSdkFix
{

    public class LinuxWorldBugFix
    {
        [InitializeOnLoadMethod]
        public static void RegisterSDKCallback()
        {
            VRCSdkControlPanel.OnSdkPanelEnable += (sender, e) =>
            {
                if (VRCSdkControlPanel.TryGetBuilder<IVRCSdkWorldBuilderApi>(out var builder))
                {
                    builder.OnSdkBuildProgress += Run;
                }
            };
        }

        static void Run(object sender, object target)
        {
            string flatpakDir =
                Environment.GetEnvironmentVariable("HOME") + "/.var/app/com.unity.UnityHub/cache/tmp/" + Application.companyName + "/" + Application.productName;

            FixVrcwFileName("/tmp/" + Application.companyName + "/" + Application.productName);
            FixVrcwFileName(flatpakDir);
        }
        static void FixVrcwFileName(string path)
        {
            if (Directory.Exists(path))
            {
                string vrcwFileName = "/scene-" + EditorUserBuildSettings.activeBuildTarget + "-" + SceneManager.GetActiveScene().name + ".vrcw";
                System.Diagnostics.ProcessStartInfo process_start_info_ = new()
                {
                    FileName = "ln",
                    Arguments = "-s \"" + path + vrcwFileName.ToLower() + "\" \"" + path + vrcwFileName + "\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

                System.Diagnostics.Process process_ = System.Diagnostics.Process.Start(process_start_info_);

                process_.WaitForExit();
                process_.Close();
            }
        }
    }

}
#endif
