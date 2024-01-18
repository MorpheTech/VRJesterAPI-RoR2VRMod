using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRJester.Core;
using VRJester.Utils.VRData;


namespace VRJester.Vox {

    public class Vhere {
        // Class that represents a sphere

        private readonly VRDevice vrDevice;
        public readonly Config config;
        private int id, previousId;
        private string movementDirection = "idle";
        private GestureTrace gestureTrace;
        public Vector3 centroid;
        public Quaternion faceDirection;
        public float sphereRadius = Constants.VHERE_RADIUS;

        public Vhere(VRDevice vrDevice, VRPose centroidPose, Quaternion faceDirection, string configPath) {
            config = Config.ReadConfig(configPath); // Override defaults
            if (config.VHERE_RADIUS != sphereRadius) {
                sphereRadius = config.VHERE_RADIUS;
            }

            id = 0; // Initialize Vhere Id
            previousId = id; // Initialize soon to be previous ID
            this.vrDevice = vrDevice; // Initialize VRDevice name
            this.faceDirection = faceDirection; // Initialize facing trajectory of user
            gestureTrace = new GestureTrace(Convert.ToString(id), vrDevice, centroidPose, faceDirection);
            // Initialize Center of Vhere
            UpdateVherePosition(centroidPose.Position);
        }

        // Check if point is within Vhere
        public bool HasPoint(Vector3 point) {
            // Let the sphere's centre coordinates be (cx, cy, cz) and its radius be r,
            // then point (x, y, z) is in the sphere if sqrt( (x−cx)^2 +  (y−cy)^2 + (z−cz)^2 ) <= r.
            double xcx = Math.Pow(point.x - centroid.x, 2);
            double ycy = Math.Pow(point.y - centroid.y, 2);
            double zcz = Math.Pow(point.z - centroid.z, 2);
            double radial_dist = Math.Sqrt(xcx + ycy + zcz);
            return radial_dist <= sphereRadius;
        }

        // Checks if VRDevice is in this Vhere
        public void UpdateProximity(VRDataState vrDataState, VRDevice vrDevice) {
            Vector3 pos = VRDataState.GetVRPose(vrDataState, vrDevice).Position;
            if (HasPoint(pos)) {
                IDictionary<string, int> devicesInProximity = gestureTrace.DevicesInProximity;
                devicesInProximity.TryGetValue(vrDevice.ToString(), out int times);
                gestureTrace.UpdateDeviceInProximity(vrDevice.ToString(), times);
            }
        }

        // When VRDevice is outside current Vhere, new Vhere is generated at neighboring position and returns the Trace data
        public VRPose GenerateVhere(VRDataState vrDataState) {
            VRPose pose = new();
            for (int i = 0; i < Enum.GetValues(typeof(VRDevice)).Length; i++) { // Check which VRDevice this Vhere identifies with
                VRDevice validVRDevice = Enum.GetValues(typeof(VRDevice)).Cast<VRDevice>().ToList()[i];
                if (vrDevice == validVRDevice) { // Assign the current position of the identified VRDevice {
                    pose = VRDataState.GetVRPose(vrDataState, vrDevice);
                } else { // Update the devicesInProximity for the other VRDevices (if they're within proximity)
                    UpdateProximity(vrDataState, validVRDevice);
                }
            }
            if (!HasPoint(pose.Position)) { // Check if point is outside of current Vhere
                UpdateVherePosition(pose.Position);
                Id++;
                gestureTrace.movement = movementDirection;
                movementDirection = "idle";
            }
            else {
                gestureTrace.AddPose(pose); // Constantly update the current Trace
            }
            return pose;
        }

        // Update Vhere center
        public void UpdateVherePosition(Vector3 dif) {
            centroid = dif;
        }

        // Begin with a new Trace object
        public GestureTrace BeginTrace(VRPose pose) {
            gestureTrace = new GestureTrace(Convert.ToString(id), vrDevice, pose, faceDirection);
            return gestureTrace;
        }

        public GestureTrace Trace {
            get { return gestureTrace; }
        }

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public int PreviousId {
            get { return previousId; }
            set { previousId = value; }
        }

        public VRDevice VrDevice {
            get { return vrDevice; }
        }
    }
}
