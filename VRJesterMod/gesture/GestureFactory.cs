

using System.Collections.Generic;
using VRJester.Core;


namespace VRJester.Utils.VRData {
    // This class is for creating Gesture objects with consistency
    // TODO - Implement defaults for creating gestures for storing
    //      - Implement defaults for creating gestures for testing

    public class GestureFactory(string vrDevice, string movement, long elapsedTime, double speed, string direction, IDictionary<string, int> devicesInProximity) {
        public string vrDevice = vrDevice;
        public string movement = movement;
        public long elapsedTime = elapsedTime;
        public double speed = speed;
        public string direction = direction;
        public IDictionary<string, int> devicesInProximity = devicesInProximity;

        public GestureComponent CreateGestureComponent() {
            return new GestureComponent(vrDevice, movement, elapsedTime, speed, direction, devicesInProximity);
        }
    }

}