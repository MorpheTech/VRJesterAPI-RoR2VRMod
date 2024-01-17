using System.Collections.Generic;


namespace VRJester.Core.Radix {

    internal class MetaData {
        // Class that handles representing and checking metadata of a GestureComponent

        internal long elapsedTime;
        internal double speed;
        // internal Vector3 direction;
        internal IDictionary<string, int> devicesInProximity;

        protected internal MetaData(long elapsedTime, double speed, IDictionary<string, int> devicesInProximity) {
            this.elapsedTime = elapsedTime;
            this.speed = speed;
            this.devicesInProximity = devicesInProximity;
        }

        protected internal virtual bool IsClosestFit(long maxTime, double maxSpeed) {
            return ClosestTime(maxTime) && ClosestSpeed(maxSpeed);
        }

        protected internal virtual bool ClosestTime(long maxTime) {
            return elapsedTime >= maxTime;
        }

        protected internal virtual bool ClosestSpeed(double maxSpeed) {
            return speed >= maxSpeed;
        }

        // protected internal virtual bool ClosestDirection(double minDegree, Vector3 gestureDirection) {
        //     bool ret;
        //     double degree = Calcs.GetAngle3D(direction, gestureDirection);
        //     if (double.IsNaN(degree)) {
        //         ret = minDegree == 180;
        //     }
        //     else {
        //         ret = degree < minDegree;
        //     }
        //     return ret;
        // }
    }
}
