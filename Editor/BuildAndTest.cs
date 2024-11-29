#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using VRC.SDK3.Editor;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Random = System.Random;

class BuildAndTest : EditorWindow
{
    [MenuItem("Tool/Fix World Issue on Linux/Build & Test")]
    private static async void BuildAndOpenVRChat()
    {
        if (!VRCSdkControlPanel.TryGetBuilder<IVRCSdkWorldBuilderApi>(out var builder))
        {
            Debug.LogError("Please open Builder tab in VRChat SDK Control Panel");
            return;
        }
        RunVRChat(await builder.Build(), 0, true, true);
    }

    // 以下をVRChat SDKの非公開APIを使用しないように改変
    // https://github.com/BefuddledLabs/LinuxVRChatSDKPatch/blob/main/Packages/befuddledlabs.linuxvrchatsdkpatch.worlds/Editor/World.cs#L23
    // TODO: Make the args configurable
    public static bool RunVRChat(string vrcwFilePath, int clientNumber, bool isNoVR, bool isAutoReload)
    {
        // TODO: Make this configurable
        var vrcInstallPath = Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/common/VRChat/VRChat.exe";
        if (string.IsNullOrEmpty(vrcInstallPath) || !File.Exists(vrcInstallPath))
        {
            Debug.LogError("Could not find VRChat.exe");
            return true;
        }

        // TODO: Make this configurable
        var compatDataPath = Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/compatdata/";
        if (compatDataPath == null) // Check if we could find the compatdata directory
        {
            Debug.LogError("Could not find compatdata Path");
            return false;
        }

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

        // TODO: Make them configurable
        args.Append(" --enable-debug-gui");
        args.Append(" --enable-sdk-log-levels");
        args.Append(" --enable-udon-debug-logging");
        if (isNoVR) { args.Append(" --no-vr"); }
        if (isAutoReload) { args.Append(" --watch-worlds"); }

        var argsPathFixed = Regex.Replace(args.ToString(), @"file:[/\\]*", "file:///Z:/"); // The file we have is relative to / and not the "c drive" Z:/ is /
        var processStartInfo =
            new ProcessStartInfo(
                // TODO: Make this configurable
                Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/common/Proton - Experimental/proton",
                argsPathFixed
            )
            {
                EnvironmentVariables =
                {
                        { "STEAM_COMPAT_DATA_PATH", compatDataPath },
                        { "STEAM_COMPAT_CLIENT_INSTALL_PATH", Environment.GetEnvironmentVariable("HOME") + "/.steam/" }
                },
                WorkingDirectory = Path.GetDirectoryName(vrcInstallPath) ?? "",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
        for (var index = 0; index < clientNumber; ++index)
        {
            Process.Start(processStartInfo);
            Thread.Sleep(3000);
        }

        return false;
    }
}
#endif
