using System;
using UnityEngine;
using UnityEngine.XR;


namespace VRJester.Utils.VRData {
    public struct Pose(Vector3 position, Quaternion direction) {
        public Vector3 Position { get; set; } = position;
        public Quaternion Direction { get; set; } = direction;
		public override readonly string ToString() => $"Pose[ position: {Position} | rotation: {Direction} ]";
    }

	public class VRDataState {
		// Class for encapsulating VRData of devices

		private readonly Pose hmd, rc, lc;

		public VRDataState() {
			hmd = GetPose(VRDevice.HEAD_MOUNTED_DISPLAY);
			rc = GetPose(VRDevice.RIGHT_CONTROLLER);
			lc = GetPose(VRDevice.LEFT_CONTROLLER);
		}

		public override string ToString() {
			return "data:" +
			"\r\n\t hmd: " + hmd +
			"\r\n\t rc: " + rc +
			"\r\n\t lc: " + lc;
		}

		public Pose GetPose(VRDevice vrDevice) {
			XRNode xrNode;
			switch (vrDevice) {
				case VRDevice.HEAD_MOUNTED_DISPLAY:
					xrNode = XRNode.Head;
					break;
				case VRDevice.RIGHT_CONTROLLER:
					xrNode = XRNode.RightHand;
					break;
				case VRDevice.LEFT_CONTROLLER:
					xrNode = XRNode.LeftHand;
					break;
				default:
					Log.Info("VRDevice not yet supported. Defaulting to HMD instead");
					xrNode = XRNode.Head;
					break;
			}
			InputDevice inputDevice = InputDevices.GetDeviceAtXRNode(xrNode);
			inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos);
			inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion dir);
			return new Pose(pos, dir);
		}

		public Pose Hmd {
			get { return hmd; }
		}

		public Pose Rc {
			get { return rc; }
		}

		public Pose Lc {
			get { return lc; }
		}
	}
}