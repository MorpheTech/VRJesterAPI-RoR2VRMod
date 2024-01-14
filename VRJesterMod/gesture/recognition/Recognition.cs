using System.Collections.Generic;


namespace VRJester.Core.Recog {

	public class Recognition(Gestures gestures) {
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

		public Gestures gestures = gestures;

        // Recognize the gesture & return its name
        public virtual Dictionary<string, string> Recognize(Gesture gesture) {
			Dictionary<string, string> ctx = [];
			string gestureName, id = "";
			List<GestureComponent> foundHmdGesture = gestures.hmdGestures.Search(gesture.hmdGesture);
			List<GestureComponent> foundRcGesture = gestures.rcGestures.Search(gesture.rcGesture);
			List<GestureComponent> foundLcGesture = gestures.lcGestures.Search(gesture.lcGesture);
			if (foundHmdGesture != null) {
				id += foundHmdGesture.HashCode();
				gestures.hmdGestureMapping.TryGetValue(foundHmdGesture.HashCode(), out string name);
				ctx[Constants.HMD] = name;
			}
			if (foundRcGesture != null) {
				id += foundRcGesture.HashCode();
				gestures.hmdGestureMapping.TryGetValue(foundRcGesture.HashCode(), out string name);
				ctx[Constants.RC] = name;
			}
			if (foundLcGesture != null) {
				id += foundLcGesture.HashCode();
				gestures.hmdGestureMapping.TryGetValue(foundLcGesture.HashCode(), out string name);
				ctx[Constants.LC] = name;
			}
	//        FOR DEBUGGING:
			foreach
			Log.Info(gesture);
			Log.Info(foundHmdGesture.HashCode() + " -> foundHmdGesture: " + foundHmdGesture);
			Log.Info(foundRcGesture.HashCode() + " -> foundRcGesture: " + foundRcGesture);
			Log.Info(foundLcGesture.HashCode() + " -> foundLcGesture: " + foundLcGesture);
			Log.Info("RECOGNIZE ID:" + id);
			gestureName = gestures.gestureNameSpace[id];
			ctx["gestureName"] = gestureName;
			return gestureName is not null ? ctx : [];
		}
	}

}
