using UnityEditor;

namespace GuraRil.LinuxVrcSdkFix
{
    internal static class SessionStates
    {
        private const string SESSION_STATE_PREFIX = "GuraRil.LinuxVrcSdkFix";
        private const string VRCHAT_PATH = SESSION_STATE_PREFIX + ".VRChatPath";
        private const string COMPATDATA_PATH = SESSION_STATE_PREFIX + ".CompatdataPath";
        private const string PROTON_PATH = SESSION_STATE_PREFIX + ".ProtonPath";

#if ON_VRCWORLD
        private const string NUMBER_OF_CLIENT = SESSION_STATE_PREFIX + ".NumberOfClient";
        private const string FORCE_NON_VR = SESSION_STATE_PREFIX + ".ForceNonVR";
        private const string WORLD_RELOAD = SESSION_STATE_PREFIX + ".WorldReload";
        private const string ENABLE_DEBUG_GUI = SESSION_STATE_PREFIX + ".EnableDebugGUI";
        private const string ENABLE_SDK_LOG_LEVELS = SESSION_STATE_PREFIX + ".EnableSDKLogLevels";
        private const string UDON_DEBUG_LOGGING = SESSION_STATE_PREFIX + ".UdonDebugLogging";
#endif

        public static string VRChatPath
        {
            get
            {
                return EditorPrefs.GetString(VRCHAT_PATH, DefaultPaths.LocateVRChat());
            }
            set
            {
                EditorPrefs.SetString(VRCHAT_PATH, value);
            }
        }
        public static string ProtonPath
        {
            get
            {
                return EditorPrefs.GetString(PROTON_PATH, DefaultPaths.LocateProton());
            }
            set
            {
                EditorPrefs.SetString(PROTON_PATH, value);
            }
        }
        public static string CompatdataPath
        {
            get
            {
                return EditorPrefs.GetString(COMPATDATA_PATH, DefaultPaths.LocateCompatdata());
            }
            set
            {
                EditorPrefs.SetString(COMPATDATA_PATH, value);
            }
        }

#if ON_VRCWORLD
        public static int NumberOfClient
        {
            get
            {
                return SessionState.GetInt(NUMBER_OF_CLIENT, 1);
            }
            set
            {
                SessionState.SetInt(NUMBER_OF_CLIENT, value);
            }
        }
        public static bool ForceNonVR
        {
            get
            {
                return SessionState.GetBool(FORCE_NON_VR, true);
            }
            set
            {
                SessionState.SetBool(FORCE_NON_VR, value);
            }
        }
        public static bool WorldReload
        {
            get
            {
                return SessionState.GetBool(WORLD_RELOAD, false);
            }
            set
            {
                SessionState.SetBool(WORLD_RELOAD, value);
            }
        }
        public static bool EnableDebugGUI
        {
            get
            {
                return SessionState.GetBool(ENABLE_DEBUG_GUI, true);
            }
            set
            {
                SessionState.SetBool(ENABLE_DEBUG_GUI, value);
            }
        }
        public static bool EnableSDKLogLevels
        {
            get
            {
                return SessionState.GetBool(ENABLE_SDK_LOG_LEVELS, true);
            }
            set
            {
                SessionState.SetBool(ENABLE_SDK_LOG_LEVELS, value);
            }
        }
        public static bool UdonDebugLogging
        {
            get
            {
                return SessionState.GetBool(UDON_DEBUG_LOGGING, true);
            }
            set
            {
                SessionState.SetBool(UDON_DEBUG_LOGGING, value);
            }
        }
#endif
    }
}
