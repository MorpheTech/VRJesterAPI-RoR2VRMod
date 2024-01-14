using System;
using UnityEngine;


namespace VRJester.Utils {

    public class Calcs {
        // Class that holds mathematical functions

        // Get magnitude/length of 2D vector v
        public static double GetMagnitude2D(Vector3 v) {
            return Math.Sqrt(Math.Pow(v.x, 2) + Math.Pow(v.z, 2));
        }

        // Get magnitude/length of 3D vector v
        public static double GetMagnitude3D(Vector3 v) {
            return Math.Sqrt(Math.Pow(v.x, 2) + Math.Pow(v.y, 2) + Math.Pow(v.z, 2));
        }

        // Get angle between two 2D vectors and return in ° (degrees)
        public static double GetAngle2D(Vector3 v1, Vector3 v2) {
            Vector3 flat = new(1, 0, 1);
            double dotProduct = Vector3.Dot(Vector3.Scale(v1, flat), Vector3.Scale(v2, flat));
            double radians = dotProduct / (GetMagnitude2D(v1) * GetMagnitude2D(v2));
            return ConvertToDegrees(Math.Acos(radians));
        }

        // Get angle between two 3D vectors and return in ° (degrees)
        public static double GetAngle3D(Vector3 v1, Vector3 v2) {
            double dotProduct = Vector3.Dot(v1, v2);
            double radians = dotProduct / (GetMagnitude3D(v1) * GetMagnitude3D(v2));
            return ConvertToDegrees(Math.Acos(radians));
        }

        public static double ConvertToDegrees(double radians) {
            return 180 / Math.PI * radians;
        }
    }
}
