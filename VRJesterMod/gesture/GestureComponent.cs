using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRJester.Utils;


namespace VRJester.Core {

	/// <param name="VrDevice">           = "RC"; // The VRDevice </param>
	/// <param name="Movement">           = "idle"; // Movement taken to get to Vox </param>
	/// <param name="ElapsedTime">        = 0; // Time spent within Vox in milliseconds (added on the fly while idle) </param>
	/// <param name="Speed">              = 0.0; // Average Speed within a Vox in m/s (calculated on the fly while idle) </param>
	/// <param name="Direction">          = {0.0 , 0.0, 0.0}; // Average Direction the VRDevice is facing </param>
	/// <param name="DevicesInProximity"> = new HashMap<>(); // Other VRDevices within the Vox </param>

	// This record represents a piece of a gesture & its attributes in an iteration of time per VRDevice
	public class GestureComponent(string VrDevice, string Movement, long ElapsedTime, double Speed, Vector3 Direction, IDictionary<string, int> DevicesInProximity) {
        public readonly string VrDevice = VrDevice;
        public readonly string Movement = Movement;
        public readonly long ElapsedTime = ElapsedTime;
        public readonly double Speed = Speed;
        public readonly Vector3 Direction = Direction;
        public readonly IDictionary<string, int> DevicesInProximity = DevicesInProximity;

        public override string ToString() {
			return string.Format("Path[ {0} | Movement={1} | time={2:D} | Speed={3:F2} | Direction=({4:F2}, {5:F2}, {6:F2})]", VrDevice, Movement, ElapsedTime, Speed, Direction.x, Direction.y, Direction.z);
		}

		// Note to self: DO NOT include VrDevice in hashCode, this is how 'either or' functionality works.
		// This way either VrDevice can recognize the same gesture
		public override int GetHashCode() {
			return Movement.GetHashCode() + ElapsedTime.GetHashCode() + Speed.GetHashCode()
				   + Direction.GetHashCode() + DevicesInProximity.GetHashCode();
		}

		// Check if the traced gesture is equal to a stored gesture
		public override bool Equals(object obj) {
			if (this == obj) {
				return true;
			}
			else if (obj is not GestureComponent other) {
				return false;
			}
			else {
				return VrDevice == other.VrDevice && Movement == other.Movement && object.Equals(ElapsedTime, other.ElapsedTime) && object.Equals(Speed, other.Speed) && object.Equals(Direction, other.Direction) && object.Equals(DevicesInProximity.Keys, other.DevicesInProximity.Keys);
			}
		}

		// Check if the traced gesture is within the parameters of a stored gesture
		public bool Matches(GestureComponent gesturePath) {
			return VrDevice.Equals(gesturePath.VrDevice) && Movement.Equals(gesturePath.Movement) && ElapsedTime <= gesturePath.ElapsedTime && Speed <= gesturePath.Speed && IsWithinDirection(Direction, gesturePath.Direction) && IsWithinProximity(DevicesInProximity, gesturePath.DevicesInProximity);
		}

		// Check if the gesture starts with the subGesture(i.e.: does 'cat' start with 'c')
		public static bool StartsWith(List<GestureComponent> gesture, List<GestureComponent> subGesture) {
			try {
				return gesture.GetRange(0, subGesture.Count).Equals(subGesture);
			}
			catch (System.IndexOutOfRangeException) {
				return false;
			}
		}

		// Check if the gesture has a match with the subGesture
		public static bool MatchesWith(List<GestureComponent> gesture, List<GestureComponent> subGesture) {
			try {
				for (int i = 0; i < subGesture.Count; i++) {
					if (!subGesture[i].Matches(gesture[i])) {
						return false;
					}
				}
				return true;
			}
			catch (System.IndexOutOfRangeException) {
				return false;
			}
		}

		// Check if traced gesture has a Direction within angle of the stored gesture (represented as a cone shape)
		private static bool IsWithinDirection(Vector3 Direction, Vector3 otherDirection) {
			if (Direction.Equals(new Vector3(0,0,0))) {
				return true;
			}
			else {
				return Calcs.GetAngle3D(Direction, otherDirection) <= Constants.DIRECTION_DEGREE_SPAN;
			}
		}

		// Check if traced gesture has the same devices within proximity of the stored gesture
		private static bool IsWithinProximity(IDictionary<string, int> devices, IDictionary<string, int> otherDevices) {
			if (devices.Count == 0) {
				return true;
			}
			else {
				return object.Equals(devices.Keys, otherDevices.Keys);
			}
		}

		// Concatenate the 2 GestureComponent lists and return the new one
		public static List<GestureComponent> Concat(List<GestureComponent> gestureComponent1, List<GestureComponent> gestureComponent2) {
			if (gestureComponent1 == null) {
				gestureComponent1 = [];
			}
			if (gestureComponent2 == null) {
				gestureComponent2 = [];
			}
			return Enumerable.Concat(gestureComponent1, gestureComponent2).ToList();
		} 

		// Copy the given gesture and override fields with new values
		public static List<GestureComponent> Copy(List<GestureComponent> gesture, IDictionary<string, string> newValues) {
			List<GestureComponent> newGesture = [];
			foreach (GestureComponent gestureComponent in gesture) {
				string VrDevice = newValues["VrDevice"] is null ? gestureComponent.VrDevice : newValues["VrDevice"];
				string Movement = gestureComponent.Movement;
				long ElapsedTime = gestureComponent.ElapsedTime;
				double Speed = gestureComponent.Speed;
				Vector3 Direction = gestureComponent.Direction;
				IDictionary<string, int> DevicesInProximity = gestureComponent.DevicesInProximity;

				GestureComponent newComponent = new(VrDevice, Movement, ElapsedTime, Speed, Direction, DevicesInProximity);
				newGesture.Add(newComponent);
			}
			return newGesture;
		}
	}

}

// Dummy class to resolve this error :|
// Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported
namespace System.Runtime.CompilerServices {
	internal static class IsExternalInit {}
}