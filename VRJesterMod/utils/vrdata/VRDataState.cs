using System;
using UnityEngine;
using UnityEngine.XR;


namespace VRJester.Utils.VRData {

    public struct VRPose(Vector3 position, Quaternion direction) {
        public Vector3 Position { get; set; } = position;
        public Quaternion Direction { get; set; } = direction;
		public override readonly string ToString() => $"Pose[ position: {Position} | rotation: {Direction} ]";
    }

	public class VRDataState {
		// Class for encapsulating VRData of devices

		private readonly VRPose hmd, rc, lc;

		public VRDataState() {
			hmd = SetVRPose(VRDevice.HEAD_MOUNTED_DISPLAY);
			rc = SetVRPose(VRDevice.RIGHT_CONTROLLER);
			lc = SetVRPose(VRDevice.LEFT_CONTROLLER);
		}

		public override string ToString() {
			return "data:" +
			"\r\n\t hmd: " + hmd +
			"\r\n\t rc: " + rc +
			"\r\n\t lc: " + lc;
		}

		public VRPose SetVRPose(VRDevice vrDevice) {
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
			return new VRPose(pos, dir);
		}

		public VRPose Hmd {
			get { return hmd; }
		}

		public VRPose Rc {
			get { return rc; }
		}

		public VRPose Lc {
			get { return lc; }
		}

		public static VRPose GetVRPose(VRDataState vrDataState, VRDevice vrDevice) {
			VRPose ret;
			switch (vrDevice) {
				case VRDevice.HEAD_MOUNTED_DISPLAY:
					ret = vrDataState.hmd;
					break;
				case VRDevice.RIGHT_CONTROLLER:
					ret = vrDataState.rc;
					break;
				case VRDevice.LEFT_CONTROLLER:
					ret = vrDataState.lc;
					break;
				default:
					Log.Error("VRDevice not yet supported!");
					ret = new VRPose();
					break;
			}
			return ret;
		}
	}
}