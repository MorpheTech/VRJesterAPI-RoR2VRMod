using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture.radix
{
	using Calcs = com.calicraft.vrjester.utils.tools.Calcs;
	using Vec3 = com.calicraft.vrjester.utils.tools.Vec3;


	internal class MetaData
	{
		// Class that handles representing and checking metadata of a GestureComponent

		internal long elapsedTime;
		internal double speed;
		internal Vec3 direction;
		internal IDictionary<string, int> devicesInProximity;

		protected internal MetaData(long elapsedTime, double speed, Vec3 direction, IDictionary<string, int> devicesInProximity)
		{
			this.elapsedTime = elapsedTime;
			this.speed = speed;
			this.direction = direction;
			this.devicesInProximity = devicesInProximity;
		}

		protected internal virtual bool isClosestFit(long maxTime, double maxSpeed, double minDegree, Vec3 gestureDirection)
		{
			return closestTime(maxTime) && closestSpeed(maxSpeed) && closestDirection(minDegree, gestureDirection);
		}

		protected internal virtual bool closestTime(long maxTime)
		{
			return elapsedTime >= maxTime;
		}

		protected internal virtual bool closestSpeed(double maxSpeed)
		{
			return speed >= maxSpeed;
		}

		protected internal virtual bool closestDirection(double minDegree, Vec3 gestureDirection)
		{
			bool ret;
			double degree = Calcs.getAngle3D(direction, gestureDirection);
			if (double.IsNaN(degree))
			{
				ret = minDegree == 180;
			}
			else
			{
				ret = degree < minDegree;
			}
			return ret;
		}
	}

}
