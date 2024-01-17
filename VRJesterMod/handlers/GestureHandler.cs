using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using VRJester.Core;
using VRJester.Core.Recog;
using VRJester.Utils.VRData;


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
        private static bool toggled = false;

        InputDevice rightController;

        // The Update() method is run on every frame of the game.
        private void Update() {
            if (Input.GetKeyDown(KeyCode.G)) {
                rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rc);
                Log.Info($"rightController: {rightController}");
                Log.Info($"rightController position: {rc}");
                HandleNonVrGesture();
            }
            if (VRJesterMod.VR_LOADED) {
                if (Input.GetKeyDown(KeyCode.G)) {
                    Log.Info("Reloading configs...");
                    gestures.Load();
                    config = Config.ReadConfig();
                    if (toggled)
                        toggled = false;
                    else
                        toggled = true;
                }
                if (toggled)
                    HandleVrGesture();
            }
        }

        private static void HandleVrGesture() {
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
                    // Dictionary<string, string> gestureAction = config.GESTURE_KEY_MAPPINGS[recognizedGesture["gestureName"]];
                    if (!previousGesture.Equals(recognizedGesture["gestureName"])) { // Initial gesture recognition
                        previousGesture = recognizedGesture["gestureName"];
                        // TRIGGER EVENT
                        if (config.DEBUG_MODE)
                            Log.Debug("RECOGNIZED: " + recognizedGesture["gestureName"]);
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

        private static void HandleNonVrGesture() {
            config = Config.ReadConfig();
            gestures.Load();
        }

        // Clear and reset gesture listener
        private static void ResetJesterListener() {
            gesture = null;
            previousGesture = "";
            limiter = config.MAX_LISTENING_TIME;
        }

        private void OnNewPoses(TrackedDevicePose_t[] poses) {
            //Loop through each current pose
            for (uint i = 0; i < poses.Length; i++) {
                //Query SteamVR for controller position & rotation (aka pose)
                var pose = poses[i];
                var worldPose = new SteamVR_Utils.RigidTransform(pose.mDeviceToAbsoluteTracking);
                //Valid tracked device at this Index?
                if (pose.bDeviceIsConnected && pose.bPoseIsValid) {
                    var deviceClass = OpenVR.System.GetTrackedDeviceClass((uint)i);
                    if (deviceClass == ETrackedDeviceClass.Controller) {
                        //Get Rotation and Position Data
                        var pos = worldPose.pos;
                        var rot = worldPose.rot.eulerAngles;
                        Log.Info($"<< OnNewPoses >> Device {i} Position: {pos}");
                    }
                }
            }
        }
    }
}