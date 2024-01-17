using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using VRJester.Utils.VRData;
using static VRJester.Utils.Calcs;


namespace VRJester.Core {

    public class GestureTrace {
        // POJO for tracing Vox state per VRDevice in an iteration of time

        public string voxId; // The Vox ID
        public string vrDevice; // The VRDevice
        public string movement = "idle"; // Movement taken within this gesture trace
        public string direction = "*"; // The last Direction of the gesture trace
        public long elapsedTime = 0; // Time spent within Vox in ms (added on the fly while idle)
        public double speed; // Average speed within Vox (calculated on the fly while idle)
        public readonly IDictionary<string, int> devicesInProximity = new Dictionary<string, int>(); // Time other VRDevices spent within this Vox
        private Vector3 front, back, right, left;
        private readonly IList<VRPose> poses = []; // Poses captured within Vox

        public Quaternion faceDirection;

        public GestureTrace(string voxId, VRDevice vrDevice, VRPose pose, Quaternion faceDirection) {
            this.voxId = voxId;
            this.vrDevice = vrDevice.ToString();
            this.faceDirection = faceDirection;
            SetMovementBuckets(faceDirection);
            ElapsedTime = NanoTime();
            poses.Add(pose);
        }

        public override string ToString() {
            return string.Format("VRDEVICE: {0} | MOVED: {1} | Time Elapsed: {2:D}ms", vrDevice, movement, elapsedTime);
        }

        // Convert Trace object to GestureComponent
        public GestureComponent ToGestureComponent() {
            return new GestureComponent(VrDevice, Movement, ElapsedTime, Speed, Direction, DevicesInProximity);
        }

        public string VoxId {
            get {
                return voxId;
            }
        }

        public string VrDevice {
            get {
                return vrDevice;
            }
        }

        public string Movement {
            get {
                return movement;
            }
            set {
                movement = value;
            }
        }

        // Set the gesture movement or direction the VRDevice took to arrive at this current Trace
        public void SetTrajectory(Vector3 gestureVector, ref string gestureTrajectory) {
            if (gestureVector.y > Constants.VERTICAL_DEGREE_SPAN) {
                gestureTrajectory = "up";
            }
            else if (gestureVector.y < -Constants.VERTICAL_DEGREE_SPAN) {
                gestureTrajectory = "down";
            }
            else if (GetAngle2D(front, gestureVector) <= Constants.CARDINAL_DEGREE_SPAN) {
                gestureTrajectory = "forward";
            }
            else if (GetAngle2D(back, gestureVector) <= Constants.CARDINAL_DEGREE_SPAN) {
                gestureTrajectory = "back";
            }
            else if (GetAngle2D(right, gestureVector) <= Constants.CARDINAL_DEGREE_SPAN) {
                gestureTrajectory = "right";
            }
            else if (GetAngle2D(left, gestureVector) <= Constants.CARDINAL_DEGREE_SPAN) {
                gestureTrajectory = "left";
            }
            else {
                Log.Warning("NO MOVEMENT RECOGNIZED!");
                Log.Warning("VECTORS: " + front + " | " + gestureVector);
                Log.Warning("ANGLE BETWEEN FACING DIRECTION AND VECTOR: " + GetAngle2D(front, gestureVector));
            }
        }

        // Set elapsed time in ms
        public long ElapsedTime {
            set {
                if (elapsedTime == 0) {
                    elapsedTime = value;
                }
                else {
                    elapsedTime = (value - elapsedTime) / 1000000;
                }
            }
            get {
                return elapsedTime;
            }
        }

        // Set speed in ms
        public void SetSpeed(Vector3 end) {
            speed = GetMagnitude3D(end - poses[0].Position) / elapsedTime * 1000000000000;
        }

        public double Speed {
            get { return speed; }
        }

        public string Direction {
            get {
                return direction;
            }
            set {
                direction = value;
            }
        }

        public void UpdateDeviceInProximity(string vrDevice, int? times) {
            devicesInProximity[vrDevice] = times.Value+1;
        }

        public IDictionary<string, int> DevicesInProximity {
            get {
                return devicesInProximity;
            }
        }

        public void AddPose(VRPose pose) {
            poses.Add(pose);
        }

        // Set all final values resulting from a VRDevice moving into a new Vox
        public void CompleteTrace(VRPose end) {
            // Note: After this executes, it is ready to be converted into a GestureComponent
            Vector3 start = poses[0].Position;
            Vector3 gestureMovement = Vector3.Normalize(end.Position - start);
            // Vector3 gestureDirection = Vector3.Normalize(end.Direction.eulerAngles);
            Vector3 gestureDirection = Vector3.Normalize(end.Direction * Vector3.forward);
            SetTrajectory(gestureMovement, ref movement);
            SetTrajectory(gestureDirection, ref direction);
            SetSpeed(end.Position);
            ElapsedTime = NanoTime();
        }

        // Set all movement directional buckets used to determine movement
        private void SetMovementBuckets(Quaternion faceDirection) {
            // front = value;
            // back = new Vector3(-value.x, value.y, -value.z);
            // right = new Vector3(-value.z, value.y, value.x);
            // left = new Vector3(value.z, value.y, -value.x);
            front = faceDirection * Vector3.forward;
            back = faceDirection * Vector3.back;
            right = faceDirection * Vector3.right;
            left = faceDirection * Vector3.left;
        }

        private static long NanoTime() {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }
    }

}
