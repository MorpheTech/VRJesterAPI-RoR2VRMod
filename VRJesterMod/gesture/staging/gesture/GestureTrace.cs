using System;
using System.Collections.Generic;
using UnityEngine;
using VRJester.Utils.VRData;


namespace VRJester.Gesture {

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
			this.vrDevice = vrDevice.name();
			MovementBuckets = faceDirection;
			ElapsedTime = System.nanoTime();
			poses.Add(pose);
		}

		public override string ToString()
		{
			return string.Format("VRDEVICE: {0} | MOVED: {1} | Time Elapsed: {2:D}l", vrDevice, movement, elapsedTime);
		}

		// Convert Trace object to GestureComponent
		public virtual GestureComponent toGestureComponent()
		{
			return new GestureComponent(VrDevice, getMovement(), ElapsedTime, getSpeed(), new com.calicraft.vrjester.utils.tools.Vector3(Direction), DevicesInProximity);
		}

		public virtual string VoxId
		{
			get
			{
				return voxId;
			}
		}

		public virtual string VrDevice
		{
			get
			{
				return vrDevice;
			}
		}

		public virtual string Movement
		{
			get
			{
				return movement;
			}
		}

		public virtual void setMovement(string movement)
		{
			this.movement = movement;
		}

		// Set the movement the VRDevice took to arrive at this current Trace
		public virtual void setMovement(Vector3 gestureDirection)
		{
			if (gestureDirection.y > 0.85D)
			{
				movement = "up";
			}
			else if (gestureDirection.y < -0.85D)
			{
				movement = "down";
			}
			else if (getAngle2D(front, gestureDirection) <= Constants.MOVEMENT_DEGREE_SPAN)
			{
				movement = "forward";
			}
			else if (getAngle2D(back, gestureDirection) <= Constants.MOVEMENT_DEGREE_SPAN)
			{
				movement = "back";
			}
			else if (getAngle2D(right, gestureDirection) <= Constants.MOVEMENT_DEGREE_SPAN)
			{
				movement = "right";
			}
			else if (getAngle2D(left, gestureDirection) <= Constants.MOVEMENT_DEGREE_SPAN)
			{
				movement = "left";
			}
			else
			{
				Console.WriteLine("NO MOVEMENT RECOGNIZED!");
				Console.WriteLine("ANGLE BETWEEN FACING DIRECTION AND GESTURE: " + getAngle2D(front, gestureDirection));
			}
		}

		// Set elapsed time in ms
		public virtual long ElapsedTime
		{
			set
			{
				if (elapsedTime == 0)
				{
					elapsedTime = value;
				}
				else
				{
					elapsedTime = (value - elapsedTime) / 1000000;
				}
			}
			get
			{
				return elapsedTime;
			}
		}


		// Set speed in ms
		public virtual Vector3 Speed
		{
			set
			{
				this.speed = (getMagnitude3D(value.subtract(poses[0][0])) / elapsedTime) * 1000000;
			}
			get
			{
				return speed;
			}
		}


		public virtual Vector3 Direction
		{
			get
			{
				return direction;
			}
			set
			{
				this.direction = value;
			}
		}


		public virtual void updateDeviceInProximity(string vrDevice, int? times)
		{
			devicesInProximity[vrDevice] = times.Value+1;
		}

		public virtual IDictionary<string, int> DevicesInProximity
		{
			get
			{
				return devicesInProximity;
			}
		}

		public virtual void addPose(VRPose pose)
		{
			poses.Add(pose);
		}

		// Set all final values resulting from a VRDevice moving into a new Vox
		public virtual void completeTrace(Vector3[] end)
		{
			// Note: After this executes, it is ready to be converted into a GestureComponent
			Vector3 start = poses[0][0];
			Vector3 gestureDirection = end[0].subtract(start).normalize();
			setMovement(gestureDirection);
			ElapsedTime = System.nanoTime();
			setSpeed(end[0]);
			Direction = end[1];
		}

		// Set all movement directional buckets used to determine movement
		private Vector3 MovementBuckets
		{
			set
			{
				front = value;
				back = new Vector3(-value.x, value.y, -value.z);
				right = new Vector3(-value.z, value.y, value.x);
				left = new Vector3(value.z, value.y, -value.x);
				// yRot method causes following error: Unable to retrieve inform for type dumb-color
			}
		}
	}

}
