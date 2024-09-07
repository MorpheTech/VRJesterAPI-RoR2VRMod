

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

        public GestureComponent CreateGestureComponent(string vrDevice="", string movement="", long elapsedTime=0, double speed=0.0, string direction="") {
            // Override object fields if value provided
            if (vrDevice != "")
                this.vrDevice = vrDevice;
            if (movement != "")
                this.movement = movement;
            if (elapsedTime != 0)
                this.elapsedTime = elapsedTime;
            if (speed != 0)
                this.speed = speed;
            if (direction != "")
                this.direction = direction;
            return new GestureComponent(this.vrDevice, movement, elapsedTime, speed, direction, devicesInProximity);
        }

        public Gesture CreateGesture(List<GestureComponent> hmdGesture, List<GestureComponent> rcGesture, List<GestureComponent> lcGesture, IList<string> validDevices) {
            Gesture gesture = new(hmdGesture, rcGesture, lcGesture);
            return gesture;
        }
    }
}