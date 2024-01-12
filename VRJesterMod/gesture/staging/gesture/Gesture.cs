using System.Collections.Generic;
using VRJester.Vox;

namespace VRJester.Gesture {

	public class Gesture {
		// Class that handles compiling the GestureComponent list for each VRDevice
		// Note: A list of GestureComponent's represents a gesture of an individual VRDevice

		private readonly IList<Vhere> vhereList = new List<Vhere>();
		public IList<GestureComponent> hmdGesture = new List<GestureComponent>();
		public IList<GestureComponent> rcGesture = new List<GestureComponent>();
		public IList<GestureComponent> lcGesture = new List<GestureComponent>();
		public IList<string> validDevices = new List<string>();

		// Initialize the first gesture trace and continue tracking until completion of gesture
		public Gesture(VRDataState vrDataState)
		{
			// Note: Facing direction is set here, meaning all movements after tracing this Gesture object are relative to that
			Vec3[] hmdOrigin = vrDataState.getHmd(), rcOrigin = vrDataState.getRc(), lcOrigin = vrDataState.getLc();
			Vhere hmdVhere = new Vhere(VRDevice.HEAD_MOUNTED_DISPLAY, hmdOrigin, Constants.CONFIG_PATH);
			Vhere rcVhere = new Vhere(VRDevice.RIGHT_CONTROLLER, rcOrigin, Constants.CONFIG_PATH);
			Vhere lcVhere = new Vhere(VRDevice.LEFT_CONTROLLER, lcOrigin, Constants.CONFIG_PATH);
			vhereList.Add(hmdVhere);
			vhereList.Add(rcVhere);
			vhereList.Add(lcVhere);
		}

		// Initialize Gesture with already set gestures for each VRDevice
		public Gesture(IList<GestureComponent> hmdGesture, IList<GestureComponent> rcGesture, IList<GestureComponent> lcGesture)
		{
			if (hmdGesture != null)
			{
				this.hmdGesture = hmdGesture;
			}
			if (rcGesture != null)
			{
				this.rcGesture = rcGesture;
			}
			if (lcGesture != null)
			{
				this.lcGesture = lcGesture;
			}

		}

		// Initialize Gesture with already set gestures for each VRDevice
		public Gesture(Dictionary<string, IList<GestureComponent>> gesture)
		{
			foreach (string vrDevice in gesture.Keys)
			{
				IList<string> devices = new List<string> {vrDevice.Split("\\|", true)};
				if (devices.Contains(Constants.HMD))
				{
					IDictionary<string, string> newValues = new Dictionary<string, string>();
					newValues["vrDevice"] = Constants.HMD;
					if (devices.Count > 1)
					{
						validDevices.Add(Constants.HMD);
					}
					hmdGesture = GestureComponent.copy(gesture[vrDevice], newValues);
				}
				if (devices.Contains(Constants.RC))
				{
					IDictionary<string, string> newValues = new Dictionary<string, string>();
					newValues["vrDevice"] = Constants.RC;
					if (devices.Count > 1)
					{
						validDevices.Add(Constants.RC);
					}
					rcGesture = GestureComponent.copy(gesture[vrDevice], newValues);
				}
				if (devices.Contains(Constants.LC))
				{
					IDictionary<string, string> newValues = new Dictionary<string, string>();
					newValues["vrDevice"] = Constants.LC;
					if (devices.Count > 1)
					{
						validDevices.Add(Constants.LC);
					}
					lcGesture = GestureComponent.copy(gesture[vrDevice], newValues);
				}
			}
		}

		public override string ToString()
		{
			return "Gesture:" + "\r\n  hmdGesture: " + hmdGesture + "\r\n  rcGesture: " + rcGesture + "\r\n  lcGesture: " + lcGesture;
		}

		// Record the Vox trace of each VRDevice and store the resulting data
		public virtual void track(VRDataState vrDataRoomPre)
		{
			foreach (Vhere vhere in vhereList)
			{ // Loop through each VRDevice's Vox
				Vec3[] currentPoint = vhere.generateVhere(vrDataRoomPre);
				int currentId = vhere.getId();
				if (vhere.getPreviousId() != currentId)
				{ // Check if a VRDevice exited Vox
					vhere.setPreviousId(currentId);
					GestureTrace gestureTrace = vhere.getTrace();
					gestureTrace.completeTrace(currentPoint);
	//                System.out.println("COMPLETE TRACK: " + vhere.getId() + ": " + gestureTrace);
					vhere.beginTrace(currentPoint);
					switch (vhere.getVrDevice())
					{ // Append a Vhere trace's new GestureComponent object per VRDevice
						case HEAD_MOUNTED_DISPLAY:
							hmdGesture.Add(gestureTrace.toGestureComponent());
							break;
						case RIGHT_CONTROLLER:
							rcGesture.Add(gestureTrace.toGestureComponent());
							break;
						case LEFT_CONTROLLER:
							lcGesture.Add(gestureTrace.toGestureComponent());
							break;
					}
				}
			}
		}

		// Store the current data of each Vox for each VRDevice
		public virtual void trackComplete(VRDataState vrDataRoomPre)
		{
			// TODO - Implement way to store idle Gesture trace if VRDevice never exited Vox.
			//  And only add it once & complete the trace once it's done.
			//  Also make way to specify starter GestureTrace.
			foreach (Vhere vhere in vhereList)
			{ // Loop through each VRDevice's Vox
				GestureTrace gestureTrace = vhere.getTrace();
	//            System.out.println("GESTURE TRACE: " + gestureTrace);
				if (gestureTrace.getMovement().Equals("idle"))
				{
					Vec3[] currentPoint = vhere.generateVhere(vrDataRoomPre);
					gestureTrace.completeIdleTrace(currentPoint);
					vhere.beginTrace(currentPoint);
					switch (vhere.getVrDevice())
					{ // Append a Vox trace's new GestureComponent object per VRDevice
						case HEAD_MOUNTED_DISPLAY:
							hmdGesture.Add(gestureTrace.toGestureComponent());
							break;
						case RIGHT_CONTROLLER:
							rcGesture.Add(gestureTrace.toGestureComponent());
							break;
						case LEFT_CONTROLLER:
							lcGesture.Add(gestureTrace.toGestureComponent());
							break;
					}
				}
			}
		}

		public virtual IList<GestureComponent> getGesture(string vrDevice)
		{
			IList<GestureComponent> gesture = new List<GestureComponent>();
			switch (vrDevice)
			{
				case Constants.HMD:
					gesture = hmdGesture;
					break;
				case Constants.RC:
					gesture = rcGesture;

//====================================================================================================
//End of the allowed output for the Free Edition of Java to C# Converter.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================
