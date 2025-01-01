using System;
using System.IO;
using UnityEditor;

namespace GuraRil.LinuxVrcSdkFix
{
    static class DefaultPaths
    {
        private static readonly string[] vrchatPathList =
        {
            Environment.GetEnvironmentVariable("HOME")+"/.local/share/Steam/steamapps/common/VRChat/VRChat.exe"
        };
        private static readonly string[] protonPathList =
        {
            "/usr/bin/proton",
            Environment.GetEnvironmentVariable("HOME")+"/.local/share/Steam/steamapps/common/Proton - Experimental/proton",
            Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/common/Proton 9.0 (Beta)/proton",
            Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/common/Proton 8.0/proton",
            Environment.GetEnvironmentVariable("HOME") + "/.local/share/Steam/steamapps/common/Proton 7.0/proton"
        };
        private static readonly string[] compatdataPathList =
        {
            Environment.GetEnvironmentVariable("HOME")+"/.local/share/Steam/steamapps/compatdata"
        };

        public static string LocateVRChat()
        {
            foreach (var path in vrchatPathList)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            EditorUtility.DisplayDialog("VRChatが見つかりませんでした",
                "VRChatの実行ファイルが見つからないか、VRChatがインストールされていません。\n" +
                "すでにVRChatがインストールされている場合は、パスを手動で設定してください。",
                "OK");
            return "";
        }
        public static string LocateProton()
        {
            foreach (var path in protonPathList)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            EditorUtility.DisplayDialog("Protonが見つかりませんでした",
                "protonの実行ファイルが見つからないか、Protonがインストールされていません。\n" +
                "すでにProtonがインストールされている場合は、パスを手動で設定してください。",
                "OK");
            return "";
        }
        public static string LocateCompatdata()
        {
            foreach (var path in compatdataPathList)
            {
                if (Directory.Exists(path))
                {
                    return path;
                }
            }

            EditorUtility.DisplayDialog("compatdataが見つかりませんでした",
                "compatdataフォルダが見つからないか、Steamがインストールされていません。\n" +
                "すでにSteamがインストールされている場合は、パスを手動で設定してください。",
                "OK");
            return "";
        }
    }
}
