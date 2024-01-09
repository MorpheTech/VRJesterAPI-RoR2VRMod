using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture
{


	public class GestureStore
	{
		// Class for formatting the Gestures to be stored using Gson into a JSON file

		public Dictionary<string, Dictionary<string, IList<GestureComponent>>> GESTURES = new Dictionary<string, Dictionary<string, IList<GestureComponent>>>();

		public GestureStore()
		{
		}

		// Add gesture to GestureStore based on VRDevice
		public virtual void addGesture(string vrDevice, string gestureName, IList<GestureComponent> gesture, IList<string> validDevices)
		{
			Dictionary<string, IList<GestureComponent>> deviceGesture = GESTURES.GetOrDefault(gestureName, new Dictionary<string, IList<GestureComponent>>());
			if (validDevices != null)
			{
				vrDevice = String.join("|", validDevices);
				IList<GestureComponent> newGesture = GestureComponent.copy(gesture, Map.of("vrDevice", vrDevice));
				deviceGesture[vrDevice] = newGesture;
				GESTURES[gestureName] = deviceGesture;
			}
			else
			{
				deviceGesture[vrDevice] = gesture;
				GESTURES[gestureName] = deviceGesture;
			}
		}
	}
}
