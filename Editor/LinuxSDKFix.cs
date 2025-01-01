using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GuraRil.LinuxVrcSdkFix
{
    class FakeSDKControlPanel : EditorWindow
    {
        [MenuItem("Tools/Linux VRCSDK Fix")]
        public static void OpenWindow()
        {
            var wnd = GetWindow<FakeSDKControlPanel>();
            wnd.titleContent = new GUIContent("Linux VRCSDK ControlPanel");
        }

#if ON_VRCWORLD
        public void CreateGUI()
        {
            var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath("04d64090b51673bb7b896b6b4c6bcd1f")).Instantiate();
            var vrchatPath = ui.Q<TextField>("VRChatPath");
            var protonPath = ui.Q<TextField>("ProtonPath");
            var compatdataPath = ui.Q<TextField>("compatdataPath");
            var numberOfClient = ui.Q<IntegerField>("NumberOfClient");
            var forceNonVR = ui.Q<Toggle>("ForceNonVR");
            var enableWorldReload = ui.Q<Toggle>("EnableWorldReload");
            var enableDebugGUI = ui.Q<Toggle>("EnableDebugGUI");
            var enableSdkLogLevels = ui.Q<Toggle>("EnableSDKLogLevels");
            var udonDebugLogging = ui.Q<Toggle>("UdonDebugLogging");

            vrchatPath.value = SessionStates.VRChatPath;
            protonPath.value = SessionStates.ProtonPath;
            compatdataPath.value = SessionStates.CompatdataPath;
            numberOfClient.value = SessionStates.NumberOfClient;
            forceNonVR.value = SessionStates.ForceNonVR;
            enableWorldReload.value = SessionStates.WorldReload;
            enableDebugGUI.value = SessionStates.EnableDebugGUI;
            enableSdkLogLevels.value = SessionStates.EnableSDKLogLevels;
            udonDebugLogging.value = SessionStates.UdonDebugLogging;

            numberOfClient.RegisterValueChangedCallback((e) => { SessionStates.NumberOfClient = e.newValue; });
            forceNonVR.RegisterCallback<ClickEvent>((e) => { SessionStates.ForceNonVR = (e.currentTarget as Toggle).value; });
            enableWorldReload.RegisterCallback<ClickEvent>((e) => { SessionStates.WorldReload = (e.currentTarget as Toggle).value; });
            enableDebugGUI.RegisterCallback<ClickEvent>((e) => { SessionStates.ForceNonVR = (e.currentTarget as Toggle).value; });
            enableSdkLogLevels.RegisterCallback<ClickEvent>((e) => { SessionStates.EnableSDKLogLevels = (e.currentTarget as Toggle).value; });
            udonDebugLogging.RegisterCallback<ClickEvent>((e) => { SessionStates.UdonDebugLogging = (e.currentTarget as Toggle).value; });
            ui.Q<Button>("BuildAndTest").RegisterCallback<ClickEvent>((_) => { Builder.BuildAndTest(); });
            ui.Q<Button>("ConfigurePath").RegisterCallback<ClickEvent>((_) =>
            {
                SessionStates.VRChatPath = vrchatPath.value;
                SessionStates.ProtonPath = protonPath.value;
                SessionStates.CompatdataPath = compatdataPath.value;
            });

            rootVisualElement.Add(ui);

        }
#endif

#if ON_VRCAVATAR
#endif

    }
}
