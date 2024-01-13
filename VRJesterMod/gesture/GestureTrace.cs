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
		public string movement = "idle"; // Movement taken to get to Vox
		public long elapsedTime = 0; // Time spent within Vox in ms (added on the fly while idle)
		public double speed; // Average speed within Vox (calculated on the fly while idle)
		public readonly IDictionary<string, int> devicesInProximity = new Dictionary<string, int>(); // Time other VRDevices spent within this Vox
		private Vector3 direction, front, back, right, left;
		private readonly IList<VRPose> poses = []; // Poses captured within Vox

		public GestureTrace(string voxId, VRDevice vrDevice, VRPose pose, Vector3 faceDirection) {
			this.voxId = voxId;
			this.vrDevice = vrDevice.ToString();
			MovementBuckets = faceDirection;
			ElapsedTime = NanoTime();
			poses.Add(pose);
		}

		public override string ToString() {
			return string.Format("VRDEVICE: {0} | MOVED: {1} | Time Elapsed: {2:D}l", vrDevice, movement, elapsedTime);
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
		}

		public void SetMovement(string movement) {
			this.movement = movement;
		}

		// Set the movement the VRDevice took to arrive at this current Trace
		public void SetMovement(Vector3 gestureDirection) {
			if (gestureDirection.y > 0.85D) {
				movement = "up";
			}
			else if (gestureDirection.y < -0.85D) {
				movement = "down";
			}
			else if (GetAngle2D(front, gestureDirection) <= Constants.MOVEMENT_DEGREE_SPAN) {
				movement = "forward";
			}
			else if (GetAngle2D(back, gestureDirection) <= Constants.MOVEMENT_DEGREE_SPAN) {
				movement = "back";
			}
			else if (GetAngle2D(right, gestureDirection) <= Constants.MOVEMENT_DEGREE_SPAN) {
				movement = "right";
			}
			else if (GetAngle2D(left, gestureDirection) <= Constants.MOVEMENT_DEGREE_SPAN) {
				movement = "left";
			}
			else {
				Console.WriteLine("NO MOVEMENT RECOGNIZED!");
				Console.WriteLine("ANGLE BETWEEN FACING DIRECTION AND GESTURE: " + GetAngle2D(front, gestureDirection));
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
			speed = GetMagnitude3D(end - poses[0].Position) / elapsedTime * 1000000;
        }

		public double Speed {
			get { return speed; }
		}

        public Vector3 Direction {
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
			Vector3 gestureDirection = Vector3.Normalize(end.Position - start);
			SetMovement(gestureDirection);
			SetSpeed(end.Position);
			ElapsedTime = NanoTime();
			Direction = Vector3.Normalize(end.Direction.eulerAngles);
		}

		// Set all movement directional buckets used to determine movement
		private Vector3 MovementBuckets {
			set {
				front = value;
				back = new Vector3(-value.x, value.y, -value.z);
				right = new Vector3(-value.z, value.y, value.x);
				left = new Vector3(value.z, value.y, -value.x);
			}
		}

		private static long NanoTime() {
			long nano = 10000L * Stopwatch.GetTimestamp();
			nano /= TimeSpan.TicksPerMillisecond;
			nano *= 100L;
			return nano;
		}
	}

}
