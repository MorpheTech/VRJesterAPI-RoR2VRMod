using System;
using System.Collections.Generic;

namespace com.calicraft.vrjester.utils.tools
{
	using Vec3 = net.minecraft.world.phys.Vec3;

	public class Calcs
	{
		// Class that holds mathematical functions

		// Get magnitude/length of 2D vector v
		public static double getMagnitude2D(Vec3 v)
		{
			return Math.Sqrt(Math.Pow(v.x, 2) + Math.Pow(v.z, 2));
		}

		// Get magnitude/length of 3D vector v
		public static double getMagnitude3D(Vec3 v)
		{
			return Math.Sqrt(Math.Pow(v.x, 2) + Math.Pow(v.y, 2) + Math.Pow(v.z, 2));
		}

		// Get angle between two 2D vectors and return in ° (degrees)
		public static double getAngle2D(Vec3 v1, Vec3 v2)
		{
			double dotProduct = v1.multiply((1), (0), (1)).dot(v2.multiply((1), (0), (1)));
			double radians = dotProduct / (getMagnitude2D(v1) * getMagnitude2D(v2));
			return Math.toDegrees(Math.Acos(radians));
		}

		// Get angle between two 3D vectors and return in ° (degrees)
		public static double getAngle3D(Vec3 v1, Vec3 v2)
		{
			double dotProduct = v1.dot(v2);
			double radians = dotProduct / (getMagnitude3D(v1) * getMagnitude3D(v2));
			return Math.toDegrees(Math.Acos(radians));
		}

		// Check if list of point vectors are collinear with 0.15% error
		public static bool isCollinear(List<Vec3> vectors)
		{
			bool ret = false;
			Vec3 p1 = vectors[0];
			Vec3 p2 = vectors[vectors.Count - 1];
			Vec3 p3 = vectors[(vectors.Count - 1) / 2];
			double s1 = getSlope(p1, p2);
			double s2 = getSlope(p1, p3);
			double dif = getDiff(s1, s2);
			Console.WriteLine("VALID VECTORS LENGTH: " + vectors.Count);
			Console.WriteLine("COLLINEAR DIF: " + dif);
			if (dif < .15)
			{
				ret = true;
			}

			return ret;
		}

		// Get percent difference between two slopes
		public static double getDiff(double slope1, double slope2)
		{
			double ret = Math.Abs(slope1 - slope2);
			return ret / ((slope1 + slope2) / 2);
		}

		// Get slope of two 3D points
		public static double getSlope(Vec3 p1, Vec3 p2)
		{
			double xyd = Math.Pow(Math.Abs(p1.x - p2.x), 2) + Math.Pow(Math.Abs(p1.y - p2.y), 2);
			double run = Math.Sqrt(xyd);
			double rise = Math.Abs(p1.z - p2.z);
			double ret = 0;
			if (run != 0)
			{
				ret = rise / run;
			}
			return ret;
		}
	}

}
