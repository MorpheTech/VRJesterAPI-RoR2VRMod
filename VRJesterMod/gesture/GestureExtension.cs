using System.Collections.Generic;


namespace VRJester.Core {
    // Class for extending List<GestureComponent> functionaility
    // using the following methods instead since overridden methods
    // aren't inherited by List interface

    public static class Extensions {
        public static bool IsEqual(this List<GestureComponent> gesture, List<GestureComponent> otherGesture) {
            bool ret = true;
            if (gesture.Count == otherGesture.Count) {
                for (int i = 0; i < gesture.Count; i++) {
                    if (gesture[i].Equals(otherGesture[i])) {
                        ret = true;
                    } else {
                        ret = false; break;
                    }
                }
            } else {
                ret = false;
            }
            return ret;
        }

        public static int HashCode(this List<GestureComponent> gesture) {
            int hashCode = 1;
            foreach (GestureComponent component in gesture)
                hashCode = 31*hashCode + (component==null ? 0 : component.GetHashCode());
            return hashCode;
        }
    }
}