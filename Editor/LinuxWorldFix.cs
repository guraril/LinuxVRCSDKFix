using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDK3.Editor;

namespace com.guraril.linux_world_fix
{

    public class LinuxWorldBugFix
    {
        [InitializeOnLoadMethod]
        public static void RegisterSDKCallback()
        {
            VRCSdkControlPanel.OnSdkPanelEnable += AddBuildHook;
        }
        private static void AddBuildHook(object sender, EventArgs e)
        {
            if (VRCSdkControlPanel.TryGetBuilder<IVRCSdkWorldBuilderApi>(out var builder))
            {
                builder.OnSdkBuildStart += Run;
            }
        }
        static void Run(object sender, object target)
        {
            FixVrcwFileName("/tmp/" + Application.companyName + "/" + Application.productName);
            FixVrcwFileName(
                Environment.GetEnvironmentVariable("HOME") + ".var/app/com.unity.UnityHub/cache/tmp/" + Application.companyName + "/" + Application.productName);
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
