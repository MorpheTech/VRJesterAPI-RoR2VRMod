using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using VRJester.Utils.VRData;


namespace VRJester {
    public class GestureHandler : MonoBehaviour {
        public static Config config = Config.ReadConfig();
        private static VRDataState vrDataState;
        //  private static Gesture gesture;

        Vector3 rc;
        Vector3 lc;
        InputDevice rightController;
        InputDevice leftController;


        // The Update() method is run on every frame of the game.
        private void Update() {
            if (Input.GetKeyDown(KeyCode.G)) {
                rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                rightController.TryGetFeatureValue(CommonUsages.devicePosition, out rc);
                leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                leftController.TryGetFeatureValue(CommonUsages.devicePosition, out lc);
                Log.Info($"rightController: {rightController}");
                Log.Info($"rightController position: {rc}");
                // Log.Info($"leftController position: {lc}");
                HandleNonVrGesture();
            }
            if (VRJesterMod.VR_LOADED) {
                HandleVrGesture();
            }
        }

        private static void HandleVrGesture() {
            vrDataState = new VRDataState();
            // if (gesture == null) { // For initial tick from trigger
            //     gesture = new Gesture(vrDataRoomPre);
            // } else {
            //     gesture.track(vrDataRoomPre);
            // }
        }

        private static void HandleNonVrGesture() {
            config = Config.ReadConfig();
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
                        Log.Info($"Device {i} Position: {pos}");
                    }
                }
            }
        }

    }
}