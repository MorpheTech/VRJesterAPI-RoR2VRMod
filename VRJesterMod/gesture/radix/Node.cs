using System.Collections.Generic;


namespace VRJester.Core.Radix {

	public class Node(bool isGesture) {
		// Class that represents a Node in a RadixTree with paths leading to next Nodes

		public bool isGesture = isGesture;
		public Dictionary<GestureComponent, Branch> paths = [];

        public virtual Branch GetTransition(GestureComponent transitionGestureComponent) {
			return paths.TryGetValue(transitionGestureComponent, out Branch value) ? value : null;
		}

		public virtual void AddGestureComponent(List<GestureComponent> gestureComponent, Node next) {
			paths[gestureComponent[0]] = new Branch(gestureComponent, next);
		}

		public virtual int TotalGestureComponent() {
			return paths.Count;
		}

		public virtual Branch GetMatchedPath(GestureComponent transitionPath) {
			Branch newTransition = null;
			long maxTime = 0;
			double maxSpeed = 0.0;
			// double minDegree = 180.0D;
			// Vector3 anyDirection = new(0,0,0);

			foreach (GestureComponent gestureComponent in paths.Keys) {
	//            Log.Info("MATCHES: " + gestureComponent.Matches(transitionPath));
	//            Log.Info("gestureComponent: " + gestureComponent);
	//            Log.Info("transitionPath: " + transitionPath);
				if (gestureComponent.Matches(transitionPath)) {
					MetaData gestureMetaData = new(gestureComponent.ElapsedTime, gestureComponent.Speed, gestureComponent.DevicesInProximity);
	//                Log.Info("HERE: " + gestureMetaData.IsClosestFit(maxTime, maxSpeed, minDegree, transitionPath.Direction));
	//                Log.Info("minDegree: " + minDegree);
					if (gestureMetaData.IsClosestFit(maxTime, maxSpeed)) {
						maxTime = gestureComponent.ElapsedTime;
						maxSpeed = gestureComponent.Speed;
						// if (!gestureComponent.Direction.Equals(anyDirection)) {
						// 	minDegree = Calcs.GetAngle3D(gestureComponent.Direction, transitionPath.Direction);
						// }
						newTransition = paths[gestureComponent];
	//                    Log.Info("NEW minDegree: " + minDegree);
					}
				}
			}
			return newTransition;
		}

		public override string ToString() {
			return "Node[ isGesture=" + isGesture + ", paths=" + paths + "]";
		}
	}

}
