using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace VRJesterMod {
    public class TriggerEventHandler : MonoBehaviour {

        Vector3 rc;
        Vector3 lc;
        InputDevice rightController;
        InputDevice leftController;

        // The Update() method is run on every frame of the game.
        private void Update() {
            if (Input.GetKeyDown(KeyCode.G)) {
                if (this.GetComponent<TriggerEventHandler>() == null) {
                    gameObject.AddComponent<TriggerEventHandler>();
                }
                rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                rightController.TryGetFeatureValue(CommonUsages.devicePosition, out rc);
                Log.Info($"rightController position: {rc}");
            }
        }

        // private void OnNewPoses(TrackedDevicePose_t[] poses) {
        //     //Loop through each current pose
        //     for (uint i = 0; i < poses.Length; i++) {
        //         //Query SteamVR for controller position & rotation (aka pose)
        //         var pose = poses[i];
        //         var worldPose = new SteamVR_Utils.RigidTransform(pose.mDeviceToAbsoluteTracking);
        //         //Valid tracked device at this Index?
        //         if (pose.bDeviceIsConnected && pose.bPoseIsValid) {
        //             var deviceClass = OpenVR.System.GetTrackedDeviceClass((uint)i);
        //             if (deviceClass == ETrackedDeviceClass.Controller) {
        //                 //Get Rotation and Position Data
        //                 var pos = worldPose.pos;
        //                 var rot = worldPose.rot.eulerAngles;
        //                 Log.Info($"Device {i} Position: {pos}");
        //             }
        //         }
        //     }
        // }

    }
}