using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRJester.Core;
using VRJester.Core.Recog;
using VRJester.Utils.VRData;
using WindowsInput.Native;


namespace VRJester {

    public class GestureHandler : MonoBehaviour {
        public static Config config = Config.ReadConfig();
        private static VRDataState vrDataState;
        private static string previousGesture = "";
        private static Gesture gesture;
        public static Gestures gestures = new(config, Constants.GESTURE_STORE_PATH);
        public static readonly Recognition recognition = new(gestures);
        private static readonly int DELAY = config.INTERVAL_DELAY; // 0.75 second (15 ticks)
        private static int sleep = DELAY;
        private static int limiter = config.MAX_LISTENING_TIME; // 10 seconds (400 ticks)
        // private static bool toggled = false;

        public static Config ReloadConfigs() {
            config = Config.ReadConfig();
            gestures.Load();
            VRJesterMod.SetupKeyBinds();
            return config;
        }

        // The Update() method is run on every frame of the game.
        private void Update() {
            if (Input.GetKeyDown(KeyCode.G)) {
                ReloadConfigs();
            }
            if (VRJesterMod.VR_LOADED) {
                if (Input.GetKeyDown(KeyCode.G)) {
                    Log.Info("Reloading configs...");
                    ReloadConfigs();
                    ResetJesterListener();
                    // if (toggled)
                    //     toggled = false;
                    // else
                    //     toggled = true;
                }
                // if (toggled)
                HandleVrGesture();
            }
        }

        private void HandleVrGesture() {
            vrDataState = new VRDataState();
            if (gesture == null) { // For initial tick from trigger
                gesture = new Gesture(vrDataState);
            } else {
                gesture.Track(vrDataState);
            }

            // TODO - Implement way to only try to recognize the 1st GestureComponent,
            // then attempt to recognize the rest of the gesture. If a gesture is recognized,
            // wait a bit longer for the next interval to see if another gesture is recognized.
            // First GestureComponent's should be unique and intentional so that when the user 
            // wants to perform a gesture they won't have to wait for the interval to reset the gesture.
            Dictionary<string, string> recognizedGesture = recognition.Recognize(gesture);
            if (sleep != 0) { // Execute every tick
                if (recognizedGesture.Count != 0) {
                    // Dictionary<string, string> gestureAction = config.GESTURE_ACTIONS[recognizedGesture["gestureName"]];
                    if (!previousGesture.Equals(recognizedGesture["gestureName"])) { // Initial gesture recognition
                        previousGesture = recognizedGesture["gestureName"];
                        StartCoroutine(TriggerAction(previousGesture));
                        sleep = DELAY; // Reset ticker to extend listening time
                        limiter = config.MAX_LISTENING_TIME; // Reset limiter
                    }
                    // else { // Handle gesture recognition after initial if gesture hasn't changed (hold down key)
                    //     if (gestureAction != null && gestureAction["KEY_ACTION"].Equals("hold")) {
                    //         // TRIGGER EVENT
                    //         Log.Info("RECOGNIZED GESTURE EZ GG -> " + recognizedGesture["gestureName"]);
                    //         sleep = DELAY; // Reset ticker to extend listening time
                    //         limiter = config.MAX_LISTENING_TIME; // Reset limiter
                    //     }
                    // }
                }
            }
            if (sleep == 0) { // Reset trigger at the end of the delay interval
                ResetJesterListener();
                sleep = DELAY;
            }
            sleep--;
        }

        // Clear and reset gesture listener
        private static void ResetJesterListener() {
            gesture = null;
            previousGesture = "";
            limiter = config.MAX_LISTENING_TIME;
        }

        private static IEnumerator TriggerAction(string gestureName) {
            config.GESTURE_ACTIONS.TryGetValue(gestureName, out Dictionary<string, string> gestureAction);
            gestureAction.TryGetValue("KEY_BIND", out string keyBind);
            gestureAction.TryGetValue("KEY_ACTION", out string keyAction);
            VRJesterMod.KEY_MAPPINGS.TryGetValue(keyBind, out VirtualKeyCode keyCode);
            if ((int)keyCode < 5)
                yield return MousePress(keyCode, keyAction);
            else
                yield return KeyboardPress(keyCode, keyAction);
        }

        private static IEnumerator MousePress(VirtualKeyCode keyCode, string keyAction) {
            if (keyAction.ToLower() == "click" && keyCode == VirtualKeyCode.LBUTTON) {
                VRJesterMod.SIM.Mouse.LeftButtonDown();
                yield return null; yield return null;
                VRJesterMod.SIM.Mouse.LeftButtonUp();
            } else if (keyAction.ToLower() == "click" && keyCode == VirtualKeyCode.RBUTTON) {
                VRJesterMod.SIM.Mouse.RightButtonDown();
                yield return null; yield return null;
                VRJesterMod.SIM.Mouse.RightButtonUp();
            } else if (keyAction.ToLower() == "hold" && keyCode == VirtualKeyCode.LBUTTON) {
                VRJesterMod.SIM.Mouse.LeftButtonDown();
            } else if (keyAction.ToLower() == "release" && keyCode == VirtualKeyCode.LBUTTON){
                VRJesterMod.SIM.Mouse.LeftButtonUp();
            } else if (keyAction.ToLower() == "hold" && keyCode == VirtualKeyCode.RBUTTON) {
                VRJesterMod.SIM.Mouse.RightButtonDown();
            } else if (keyAction.ToLower() == "release" && keyCode == VirtualKeyCode.RBUTTON){
                VRJesterMod.SIM.Mouse.RightButtonUp();
            }
        }

        private static IEnumerator KeyboardPress(VirtualKeyCode keyCode, string keyAction) {
            if (keyAction.ToLower() == "click") {
                VRJesterMod.SIM.Keyboard.KeyDown(keyCode);
                yield return null; yield return null;
                VRJesterMod.SIM.Keyboard.KeyUp(keyCode);
            } else if (keyAction.ToLower() == "hold") {
                VRJesterMod.SIM.Keyboard.KeyDown(keyCode);
            } else if (keyAction.ToLower() == "release"){
                VRJesterMod.SIM.Keyboard.KeyUp(keyCode);
            }
        }
    }
}