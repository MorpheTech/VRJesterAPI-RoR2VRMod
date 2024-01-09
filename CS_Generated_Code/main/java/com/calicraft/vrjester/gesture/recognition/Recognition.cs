using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture.recognition
{
	using Constants = com.calicraft.vrjester.config.Constants;
	using Gesture = com.calicraft.vrjester.gesture.Gesture;
	using com.calicraft.vrjester.gesture;
	using Gestures = com.calicraft.vrjester.gesture.Gestures;


	public class Recognition
	{
		// Class that handles identifying a gesture utilizing the RadixTree

		// TODO - Either Check constantly, everytime Path gets appended or check at end of gesture listening.
		//      - Note, I must determine how to know when to start & stop listening to a gesture.
		//      - There will be 2 modes of triggering & 3 modes of terminating the recognition listener
		//      - listenOnKey | listenOnPosition
		//      - recognizeOnContinuous | recognizeOnRecognize | recognizeOnRelease
		//      - Upon terminating the listener, a GestureRecognition Event
		//      will either be fired. As a traced gesture makes its way through
		//      the radix sort tree, each "isGesture node" will be fired to
		//      InterMod Event Bus to notify consumers of the API that a "step"
		//      in a gesture's path has been fulfilled. This allows a way for
		//      users/devs to know if and when their gestures are being recognized

		public Gestures gestures;

		public Recognition(Gestures gestures)
		{
			this.gestures = gestures;
		}

		// Recognize the gesture & return its name
		public virtual Dictionary<string, string> recognize(Gesture gesture)
		{
			Dictionary<string, string> ctx = new Dictionary<string, string>();
			string gestureName, id = "";
			IList<GestureComponent> foundHmdGesture = gestures.hmdGestures.search(gesture.hmdGesture);
			IList<GestureComponent> foundRcGesture = gestures.rcGestures.search(gesture.rcGesture);
			IList<GestureComponent> foundLcGesture = gestures.lcGestures.search(gesture.lcGesture);
			if (foundHmdGesture != null)
			{
				id += foundHmdGesture.GetHashCode();
				ctx[Constants.HMD] = gestures.hmdGestureMapping[foundHmdGesture.GetHashCode()];
			}
			if (foundRcGesture != null)
			{
				id += foundRcGesture.GetHashCode();
				ctx[Constants.RC] = gestures.rcGestureMapping[foundRcGesture.GetHashCode()];
			}
			if (foundLcGesture != null)
			{
				id += foundLcGesture.GetHashCode();
				ctx[Constants.LC] = gestures.lcGestureMapping[foundLcGesture.GetHashCode()];
			}
	//        FOR DEBUGGING:
	//        System.out.println(gesture);
	//        System.out.println("foundHmdGesture: " + foundHmdGesture);
	//        System.out.println("foundRcGesture: " + foundRcGesture);
	//        System.out.println("foundLcGesture: " + foundLcGesture);
	//        System.out.println("RECOGNIZE ID:" + id);
			gestureName = gestures.gestureNameSpace[id];
			ctx["gestureName"] = gestureName;
			return !string.ReferenceEquals(gestureName, null) ? ctx : new Dictionary<string, string>();
		}
	}

}
