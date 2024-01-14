using BepInEx;
using R2API;
using RoR2;
using Valve.VR;
using System.IO;
using System.Collections.Generic;
using System.Collections;


namespace VRJester
{

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
        public static Dictionary<string, string> KEY_MAPPINGS = [];

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
            yield return null; yield return null; yield return null;
            // yield return new WaitForSeconds(3.0F);

            if (OpenVR.IsHmdPresent())
                VR_LOADED = true;
            if (VR_LOADED) {
                EVRInitError eError = EVRInitError.None;
                CVRSystem VR_SYSTEM = OpenVR.Init(ref eError, EVRApplicationType.VRApplication_Background);
                Log.Info("OpenVR Background Process Initialized...");
                VR_SYSTEM.IsDisplayOnDesktop();
            } else {
                Log.Info("Running in Non-VR Mode...");
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

		private static void SetupClient() {
			// Create setup for assigning gestures to keys
		}
    }
}