using System.Collections.Generic;
using Rewired.Utils;


namespace VRJester.Core {

    public class GestureStore {
        // Class for formatting the Gestures to be stored using Gson into a JSON file

        public Dictionary<string, Dictionary<string, List<GestureComponent>>> GESTURES = [];

        public GestureStore() {}

        // Add gesture to GestureStore based on VRDevice
        public virtual void AddGesture(string vrDevice, string gestureName, List<GestureComponent> gesture, IList<string> validDevices) {
            GESTURES.TryGetValue(gestureName, out Dictionary<string, List<GestureComponent>> deviceGesture);
            if (deviceGesture.IsNullOrDestroyed()) // Default the value myself...
                deviceGesture = [];
            if (validDevices != null) {
                vrDevice = string.Join("|", validDevices);
                List<GestureComponent> newGesture = GestureComponent.Copy(gesture, new Dictionary<string, string> { { "VrDevice", vrDevice } });
                deviceGesture[vrDevice] = newGesture;
                GESTURES[gestureName] = deviceGesture;
            }
            else {
                deviceGesture[vrDevice] = gesture;
                GESTURES[gestureName] = deviceGesture;
            }
        }
    }
}
