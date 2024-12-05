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

class BuildAndTest
{
    public static async void BuildAndOpenVRChat(
        int numberOfClient, bool isNonVR, bool enableWorldReload, bool enableDebugGUI, bool enableSDKLogLevel, bool enableUdonDebugLogging
    )
    {
        if (!VRCSdkControlPanel.TryGetBuilder<IVRCSdkWorldBuilderApi>(out var builder))
        {
            Debug.LogError("Please open Builder tab in VRChat SDK Control Panel");
            return;
        }
        RunVRChat(await builder.Build(), numberOfClient, isNonVR, enableWorldReload, enableDebugGUI, enableSDKLogLevel, enableUdonDebugLogging);
    }

    // 以下をVRChat SDKの非公開APIを使用しないように改変
    // https://github.com/BefuddledLabs/LinuxVRChatSDKPatch/blob/main/Packages/befuddledlabs.linuxvrchatsdkpatch.worlds/Editor/World.cs#L23
    private static void RunVRChat(
        string vrcwFilePath, int clientNumber, bool isNoVR, bool isAutoReload, bool enableDebugGUI, bool enableSDKLogLevel, bool enableUdonDebugLogging
    )
    {
        var defaultVRChatPath = Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/common/VRChat/VRChat.exe";
        var defaultProtonPath = Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/common/Proton - Experimental/proton";
        var defaultCompatdataPath = Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/compatdata/";

        string vrcInstallPath;
        if (EditorPrefs.HasKey("FixLinuxVRCWorldIssue:VRChatPath"))
        {
            vrcInstallPath = EditorPrefs.GetString("FixLinuxVRCWorldIssue:VRChatPath");
        }
        else if (File.Exists(defaultVRChatPath))
        {
            EditorPrefs.SetString("FixLinuxVRCWorldIssue:VRChatPath", defaultVRChatPath);
            vrcInstallPath = defaultVRChatPath;
        }
        else
        {
            Debug.LogError("incorrect VRChat path");
            return;
        }

        string protonPath;
        if (EditorPrefs.HasKey("FixLinuxVRCWorldIssue:ProtonPath"))
        {
            protonPath = EditorPrefs.GetString("FixLinuxVRCWorldIssue:ProtonPath");
        }
        else if (File.Exists(defaultProtonPath))
        {
            EditorPrefs.SetString("FixLinuxVRCWorldIssue:ProtonPath", defaultProtonPath);
            protonPath = defaultProtonPath;
        }
        else
        {
            Debug.LogError("incorrect Proton path");
            return;
        }

        string compatdataPath;
        if (EditorPrefs.HasKey("FixLinuxVRCWorldIssue:compatdataPath"))
        {
            compatdataPath = EditorPrefs.GetString("FixLinuxVRCWorldIssue:compatdataPath");
        }
        else if (Directory.Exists(defaultCompatdataPath))
        {
            EditorPrefs.SetString("FixLinuxVRCWorldIssue:compatdataPath", defaultCompatdataPath);
            compatdataPath = defaultCompatdataPath;
        }
        else
        {
            Debug.LogError("incorrect compatdata path");
            return;
        }

        // TODO: Make this configurable
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

        if (enableDebugGUI) { args.Append(" --enable-debug-gui"); }
        if (enableSDKLogLevel) { args.Append(" --enable-sdk-log-levels"); }
        if (enableUdonDebugLogging) { args.Append(" --enable-udon-debug-logging"); }
        if (isNoVR) { args.Append(" --no-vr"); }
        if (isAutoReload) { args.Append(" --watch-worlds"); }

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
        for (var index = 0; index < clientNumber; ++index)
        {
            Process.Start(processStartInfo);
            Thread.Sleep(3000);
        }
    }
}
