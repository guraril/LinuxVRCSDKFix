using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEditor;
using VRC.SDK3.Editor;

namespace GuraRil.LinuxVrcSdkFix
{
    static class Builder
    {
#if ON_VRCWORLD
        // 参考実装: https://github.com/BefuddledLabs/LinuxVRChatSDKPatch/blob/main/Packages/befuddledlabs.linuxvrchatsdkpatch.worlds/Editor/World.cs
        public static async void BuildAndTest()
        {
            if (VRCSdkControlPanel.window == null)
            {
                EditorUtility.DisplayDialog("コントロールパネルを開く", "ビルドする前にVRChat SDKコントロールパネルのBuilderタブを開いておいてください。", "OK");
                EditorWindow.GetWindow<VRCSdkControlPanel>();
            }
            if (!VRCSdkControlPanel.TryGetBuilder<IVRCSdkWorldBuilderApi>(out var builder))
            {
                EditorUtility.DisplayDialog("builderの取得に失敗", "builderの取得に失敗しました。", "OK");
                return;
            }
            RunVRChat(await builder.Build());
        }
        private static void RunVRChat(string vrcwFilePath)
        {
            string vrcInstallPath = SessionStates.VRChatPath;
            string protonPath = SessionStates.ProtonPath;
            string compatdataPath = SessionStates.CompatdataPath;

            var compatClientInstallPath = Environment.GetEnvironmentVariable("HOME") + "/.steam/";

            // Making sure that the paths are using forward slashes
            var bundleFilePath = vrcwFilePath.Replace('\\', '/');

            var args = new StringBuilder();
            args.Append("run ");
            args.Append(vrcInstallPath);
            args.Append(' ');

            args.Append('\'');
            args.Append("--url=create?roomId=");
            args.Append(new Random().Next(65536));
            args.Append("&hidden=true");
            args.Append("&name=BuildAndRun");
            args.Append("&url=file:///");
            args.Append(bundleFilePath);
            args.Append('\'');

            if (SessionStates.EnableDebugGUI) { args.Append(" --enable-debug-gui"); }
            if (SessionStates.EnableSDKLogLevels) { args.Append(" --enable-sdk-log-levels"); }
            if (SessionStates.UdonDebugLogging) { args.Append(" --enable-udon-debug-logging"); }
            if (SessionStates.ForceNonVR) { args.Append(" --no-vr"); }
            if (SessionStates.WorldReload) { args.Append(" --watch-worlds"); }

            var argsPathFixed = Regex.Replace(args.ToString(), @"file:[/\\]*", "file:///Z:/"); // The file we have is relative to / and not the "c drive" Z:/ is /
            var processStartInfo =
                new ProcessStartInfo(protonPath, argsPathFixed)
                {
                    EnvironmentVariables =
                    {
                        { "STEAM_COMPAT_DATA_PATH", compatdataPath },
                        { "STEAM_COMPAT_CLIENT_INSTALL_PATH", compatClientInstallPath }
                    },
                    WorkingDirectory = Path.GetDirectoryName(vrcInstallPath) ?? "",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
            for (var index = 0; index < SessionStates.NumberOfClient; ++index)
            {
                Process.Start(processStartInfo);
                Thread.Sleep(3000);
            }
        }
#endif
    }
}
