using System.Collections.Generic;
using UnityEngine;
using VRJester.Utils;


namespace VRJester.Core.Radix {

	internal class MetaData {
		// Class that handles representing and checking metadata of a GestureComponent

		internal long elapsedTime;
		internal double speed;
		internal Vector3 direction;
		internal IDictionary<string, int> devicesInProximity;

		protected internal MetaData(long elapsedTime, double speed, Vector3 direction, IDictionary<string, int> devicesInProximity) {
			this.elapsedTime = elapsedTime;
			this.speed = speed;
			this.direction = direction;
			this.devicesInProximity = devicesInProximity;
		}

		protected internal virtual bool IsClosestFit(long maxTime, double maxSpeed, double minDegree, Vector3 gestureDirection) {
			return ClosestTime(maxTime) && ClosestSpeed(maxSpeed) && ClosestDirection(minDegree, gestureDirection);
		}

		protected internal virtual bool ClosestTime(long maxTime) {
			return elapsedTime >= maxTime;
		}

		protected internal virtual bool ClosestSpeed(double maxSpeed) {
			return speed >= maxSpeed;
		}

		protected internal virtual bool ClosestDirection(double minDegree, Vector3 gestureDirection) {
			bool ret;
			double degree = Calcs.GetAngle3D(direction, gestureDirection);
			if (double.IsNaN(degree)) {
				ret = minDegree == 180;
			}
			else {
				ret = degree < minDegree;
			}
			return ret;
		}
	}

}
