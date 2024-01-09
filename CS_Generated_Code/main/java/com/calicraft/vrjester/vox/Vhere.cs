using System;
using System.Collections.Generic;

namespace com.calicraft.vrjester.vox
{
	using Config = com.calicraft.vrjester.config.Config;
	using Constants = com.calicraft.vrjester.config.Constants;
	using GestureTrace = com.calicraft.vrjester.gesture.GestureTrace;
	using VRDataState = com.calicraft.vrjester.utils.vrdata.VRDataState;
	using VRDevice = com.calicraft.vrjester.utils.vrdata.VRDevice;
	using Vec3 = net.minecraft.world.phys.Vec3;


	public class Vhere
	{
		// Class that represents a virtual sphere

		private readonly VRDevice vrDevice;
		private readonly IDictionary<string, Vec3> vertices = new Dictionary<string, Vec3>();
		public readonly Config config;
		private int id, previousId;
		private string movementDirection = "idle";
		private GestureTrace gestureTrace;
		public Vec3 centroid, faceDirection;
		public float sphereRadius = Constants.VIRTUAL_SPHERE_RADIUS;

		public Vhere(VRDevice vrDevice, Vec3[] centroidPose, string configPath)
		{
			config = Config.readConfig(configPath);
			if (config.VIRTUAL_SPHERE_RADIUS != sphereRadius) // Override defaults
			{
				sphereRadius = config.VIRTUAL_SPHERE_RADIUS;
			}

			this.Id = 0; // Initialize Vhere Id
			this.previousId = id; // Initialize soon to be previous ID
			this.vrDevice = vrDevice; // Initialize VRDevice name
			this.faceDirection = centroidPose[1]; // Initialize facing angle of user
			this.gestureTrace = new GestureTrace(Convert.ToString(id), vrDevice, centroidPose, faceDirection);
			// Initialize Center of Vhere
			this.updateVherePosition(centroidPose[0]);
		}

		// Check if point is within Vhere
		public virtual bool hasPoint(Vec3 point)
		{
			// Let the sphere's centre coordinates be (cx, cy, cz) and its radius be r,
			// then point (x, y, z) is in the sphere if sqrt( (x−cx)^2 +  (y−cy)^2 + (z−cz)^2 ) <= r.
			double xcx = Math.Pow(point.x - centroid.x, 2);
			double ycy = Math.Pow(point.y - centroid.y, 2);
			double zcz = Math.Pow(point.z - centroid.z, 2);
			double radial_dist = Math.Sqrt(xcx + ycy + zcz);
			return radial_dist <= sphereRadius;
		}

		// Checks if VRDevice is in this Vhere
		public virtual void updateProximity(VRDataState vrDataRoomPre, VRDevice vrDevice)
		{
			Vec3 pos = VRDataState.getVRDevicePose(vrDataRoomPre, vrDevice, 0);
			if (hasPoint(pos))
			{
				IDictionary<string, int> devicesInProximity = gestureTrace.DevicesInProximity;
				gestureTrace.updateDeviceInProximity(vrDevice.ToString(), devicesInProximity.GetOrDefault(vrDevice.ToString(), 0));
			}
		}

		// When VRDevice is outside current Vhere, new Vhere is generated at neighboring position and returns the Trace data
		public virtual Vec3[] generateVhere(VRDataState vrDataRoomPre)
		{
			Vec3[] pose = new Vec3[2];
			for (int i = 0; i < (VRDevice[])Enum.GetValues(typeof(VRDevice)).Length - 1; i++)
			{ // Check which VRDevice this Vhere identifies with
				if (this.VrDevice.Equals((VRDevice[])Enum.GetValues(typeof(VRDevice))[i])) // Assign the current position of the identified VRDevice
				{
					pose = VRDataState.getVRDevicePose(vrDataRoomPre, this.VrDevice);
				}
				else // Update the devicesInProximity for the other VRDevices (if they're within proximity)
				{
					updateProximity(vrDataRoomPre, (VRDevice[])Enum.GetValues(typeof(VRDevice))[i]);
				}
			}
			if (!this.hasPoint(pose[0]))
			{ // Check if point is outside of current Vhere
				updateVherePosition(pose[0]);
				Id = this.Id + 1;
				gestureTrace.Movement = movementDirection;
				movementDirection = "idle";
			}
			else
			{
				gestureTrace.addPose(pose); // Constantly update the current Trace
			}
			return pose;
		}

		// Update Vhere center
		public virtual void updateVherePosition(Vec3 dif)
		{
			centroid = dif;
		}

		public virtual GestureTrace Trace
		{
			get
			{
				return gestureTrace;
			}
		}

		// Begin with a new Trace object
		public virtual GestureTrace beginTrace(Vec3[] pose)
		{
			gestureTrace = new GestureTrace(Convert.ToString(id), vrDevice, pose, faceDirection);
			return gestureTrace;
		}

		public virtual int Id
		{
			get
			{
				return id;
			}
			set
			{
				 this.id = value;
			}
		}


		public virtual int PreviousId
		{
			get
			{
				return previousId;
			}
			set
			{
				this.previousId = value;
			}
		}


		public virtual VRDevice VrDevice
		{
			get
			{
				return vrDevice;
			}
		}

	}

}
