using BepInEx;
using R2API;
using RoR2;
using Valve.VR;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Rewired.Utils;
using WindowsInput;
using WindowsInput.Native;


namespace VRJester {

    // This attribute specifies that we have a dependency on a given BepInEx Plugin,
    // We need the R2API ItemAPI dependency because we are using for adding our item to the game.
    // You don't need this if you're not using R2API in your plugin,
    // it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(ItemAPI.PluginGUID)]

    // This one is because we use a .language file for language tokens
    // More info in https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Assets/Localization/
    [BepInDependency(LanguageAPI.PluginGUID)]

    // This attribute is required, and lists metadata for your plugin.
    [BepInPlugin("com.cali.vrjester", "VRJesterMod", "1.0.0")]
    public class VRJesterMod : BaseUnityPlugin {
        public const string PluginAuthor = "Caliburs";
        public static CVRSystem VR_SYSTEM = null;
        public static bool VR_LOADED = false;
        public static Dictionary<string, VirtualKeyCode> KEY_MAPPINGS = [];
        public static InputSimulator SIM = new();

        // The Awake() method is run at the very start when the game is initialized.
        public void Awake() {
            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);

            // Init setup for configs and gesture mapping triggers
            gameObject.AddComponent(typeof(GestureHandler));
            SetupConfig();
            SetupClient();
            
            RoR2Application.onLoad += () => {
                StartCoroutine(InitVRJester());
            };
        }

        // Check if user is in VR
        private IEnumerator InitVRJester() {
            yield return null; // yield return new WaitForSeconds(3.0F);

            EVRInitError eError = EVRInitError.None;
            CVRSystem VR_SYSTEM = OpenVR.Init(ref eError, EVRApplicationType.VRApplication_Background);
            if (!VR_SYSTEM.IsNullOrDestroyed()) {
                Log.Info("OpenVR Background Process Initialized...");
                Log.Info("Is tracked device connected ?  " + VR_SYSTEM.IsTrackedDeviceConnected(0));
                VR_LOADED = true;
            } else {
                Log.Info("Running in Non-VR Mode...");
                VR_LOADED = false;
            }
        }

        private static void SetupConfig() {
            Log.Info("Setting up config files...");
            if (!File.Exists(Constants.CONFIG_PATH)) {
                VRJester.Config.WriteConfig();
            }
            if (!File.Exists(Constants.GESTURE_STORE_PATH)) {
                VRJester.Config.WriteGestureStore();
            }
        }

        // Create setup for loading & assigning gestures to keys
        private static void SetupClient() {
            Log.Info("Setting up client...");
            GestureHandler.gestures.Load();
            Log.Info("Setting up key bindings...");
            foreach (Dictionary<string, string> gestureAction in
                     GestureHandler.config.GESTURE_ACTIONS.Values) {
                // foreach (KeyValuePair<string, string> kvp in gestureAction) Log.Debug(kvp.Key + " -> " + kvp.Value);
                gestureAction.TryGetValue("KEY_BIND", out string keyBind);
                if (keyBind.Length == 1) {
                    VirtualKeyCode keyCode = (VirtualKeyCode) Enum.Parse(typeof(VirtualKeyCode), "VK_"+keyBind);
                    KEY_MAPPINGS[keyBind] = keyCode;
                    Log.Info("Storing key bind:  " + keyBind + " -> " + keyCode);
                } else if (keyBind.Length == 0) {
                    Log.Info("Key bind not found!");
                } else {
                    try {
                        VirtualKeyCode keyCode = (VirtualKeyCode) Enum.Parse(typeof(VirtualKeyCode), keyBind);
                        KEY_MAPPINGS[keyBind] = keyCode;
                        Log.Info("Storing key bind:  " + keyBind + " -> " + keyCode);
                    } catch (ArgumentException) {
                        Log.Warning("Key bind not found! Failed to store key bind:  " + keyBind);
                    }
                }
            }
        }
    }
}