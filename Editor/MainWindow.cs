using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainWindow : EditorWindow
{
#pragma warning disable IDE0044
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
#pragma warning restore IDE0044

    [MenuItem("Tools/Fix VRCWorld Issues/Open Control Panel")]
    public static void ShowWindow()
    {
        MainWindow wnd = GetWindow<MainWindow>();
        wnd.titleContent = new GUIContent("Build & Test");
        wnd.minSize = new Vector2(300, 275);
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        var vrchatPath = root.Q<TextField>("VRChatPath");
        var protonPath = root.Q<TextField>("ProtonPath");
        var compatdataPath = root.Q<TextField>("compatdataPath");
        var numberOfClient = root.Q<IntegerField>("NumberOfClient");
        var isNonVR = root.Q<Toggle>("ForceNonVR");
        var enableWorldReload = root.Q<Toggle>("EnableWorldReload");
        var enableDebugGUI = root.Q<Toggle>("EnableDebugGUI");
        var enableSDKLogLevels = root.Q<Toggle>("EnableSDKLogLevels");
        var enableUdonDebugLogging = root.Q<Toggle>("UdonDebugLogging");
        var buildAndTest = root.Q<Button>("BuildAndTest");
        var configurePath = root.Q<Button>("ConfigurePath");

        vrchatPath.value = GamePaths.GetVRChatPath();
        protonPath.value = GamePaths.GetProtonPath();
        compatdataPath.value = GamePaths.GetCompatdataPath();

        buildAndTest.RegisterCallback<ClickEvent>((_) =>
        {
            BuildAndTest.BuildAndOpenVRChat(
                numberOfClient.value, isNonVR.value, enableWorldReload.value, enableDebugGUI.value, enableSDKLogLevels.value, enableUdonDebugLogging.value
            );
        });
        configurePath.RegisterCallback<ClickEvent>((_) =>
        {
            GamePaths.SetVRChatPath(vrchatPath.value);
            GamePaths.SetProtonPath(protonPath.value);
            GamePaths.SetCompatdataPath(compatdataPath.value);
        });
    }
}
