using System.Collections.Generic;

namespace VRJester.Gesture {

	/// <param name="vrDevice">           = "RC"; // The VRDevice </param>
	/// <param name="movement">           = "idle"; // Movement taken to get to Vox </param>
	/// <param name="elapsedTime">        = 0; // Time spent within Vox in milliseconds (added on the fly while idle) </param>
	/// <param name="speed">              = 0.0; // Average speed within a Vox in m/s (calculated on the fly while idle) </param>
	/// <param name="direction">          = {0.0 , 0.0, 0.0}; // Average direction the VRDevice is facing </param>
	/// <param name="devicesInProximity"> = new HashMap<>(); // Other VRDevices within the Vox </param>

	// This record represents a piece of a gesture & its attributes in an iteration in time per VRDevice
	public record class GestureComponent(string vrDevice, string movement, long elapsedTime, double speed, Vec3 direction, IDictionary<string, int> devicesInProximity)
	{

		public override string ToString()
		{
			return string.Format("Path[ {0} | movement={1} | time={2:D} | speed={3:F2} | direction=({4:F2}, {5:F2}, {6:F2})]", vrDevice, movement, elapsedTime, speed, direction.x, direction.y, direction.z);
		}

		// Note to self: DO NOT include vrDevice in hashCode, this is how 'either or' functionality works.
		// This way either vrDevice can recognize the same gesture
		public override int GetHashCode()
		{
			return Objects.hash(movement, elapsedTime, speed, direction, devicesInProximity);
		}

		// Check if the traced gesture is equal to a stored gesture
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			else if (!(obj is GestureComponent other))
			{
				return false;
			}
			else
			{
				return vrDevice == other.vrDevice && movement == other.movement && object.Equals(elapsedTime, other.elapsedTime) && object.Equals(speed, other.speed) && object.Equals(direction, other.direction) && object.Equals(devicesInProximity.keySet(), other.devicesInProximity.keySet());
			}
		}

		// Check if the traced gesture is within the parameters of a stored gesture
		public bool matches(GestureComponent gesturePath)
		{
			return vrDevice.Equals(gesturePath.vrDevice) && movement.Equals(gesturePath.movement) && elapsedTime <= gesturePath.elapsedTime && speed <= gesturePath.speed && isWithinDirection(direction, gesturePath.direction) && isWithinProximity(devicesInProximity, gesturePath.devicesInProximity);
		}

		// Check if the gesture starts with the subGesture(i.e.: does 'cat' start with 'c')
		public static bool startsWith(IList<GestureComponent> gesture, IList<GestureComponent> subGesture)
		{
			try
			{
				return gesture.subList(0, subGesture.Count).Equals(subGesture);
			}
			catch (System.IndexOutOfRangeException)
			{
				return false;
			}
		}

		// Check if the gesture has a match with the subGesture
		public static bool matchesWith(IList<GestureComponent> gesture, IList<GestureComponent> subGesture)
		{
			try
			{
				for (int i = 0; i < subGesture.Count; i++)
				{
					if (!subGesture[i].matches(gesture[i]))
					{
						return false;
					}
				}
				return true;
			}
			catch (System.IndexOutOfRangeException)
			{
				return false;
			}
		}

		// Check if traced gesture has a direction within angle of the stored gesture (represented as a cone shape)
		private static bool isWithinDirection(Vec3 direction, Vec3 otherDirection)
		{
			if (direction.Equals(new Vec3(0,0,0)))
			{
				return true;
			}
			else
			{
				return Calcs.getAngle3D(direction, otherDirection) <= Constants.DIRECTION_DEGREE_SPAN;
			}
		}

		// Check if traced gesture has the same devices within proximity of the stored gesture
		private static bool isWithinProximity(IDictionary<string, int> devices, IDictionary<string, int> otherDevices)
		{
			if (devices.Count == 0)
			{
				return true;
			}
			else
			{
				return object.Equals(devices.Keys, otherDevices.Keys);
			}
		}

		// Concatenate the 2 GestureComponent lists and return the new one
		public static IList<GestureComponent> concat(IList<GestureComponent> gestureComponent1, IList<GestureComponent> gestureComponent2)
		{
			if (gestureComponent1 == null)
			{
				gestureComponent1 = new List<GestureComponent>();
			}
			if (gestureComponent2 == null)
			{
				gestureComponent2 = new List<GestureComponent>();
			}
			return Stream.concat(gestureComponent1.stream(), gestureComponent2.stream()).toList();
		}

		// Copy the given gesture and override fields with new values
		public static IList<GestureComponent> copy(IList<GestureComponent> gesture, IDictionary<string, string> newValues)
		{
			IList<GestureComponent> newGesture = new List<GestureComponent>();
			foreach (GestureComponent gestureComponent in gesture)
			{
				string vrDevice = string.ReferenceEquals(newValues["vrDevice"], null) ? gestureComponent.vrDevice : newValues["vrDevice"];
				string movement = gestureComponent.movement;
				long elapsedTime = gestureComponent.elapsedTime;
				double speed = gestureComponent.speed;
				Vec3 direction = gestureComponent.direction;
				IDictionary<string, int> devicesInProximity = gestureComponent.devicesInProximity;

				GestureComponent newComponent = new GestureComponent(vrDevice, movement, elapsedTime, speed, direction, devicesInProximity);
				newGesture.Add(newComponent);
			}
			return newGesture;
		}
	}

}
