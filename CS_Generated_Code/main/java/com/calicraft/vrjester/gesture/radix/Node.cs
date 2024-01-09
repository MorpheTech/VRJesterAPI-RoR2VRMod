using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture.radix
{
	using com.calicraft.vrjester.gesture;
	using Calcs = com.calicraft.vrjester.utils.tools.Calcs;
	using Vec3 = com.calicraft.vrjester.utils.tools.Vec3;


	public class Node
	{
		// Class that represents a Node in a RadixTree with paths leading to next Nodes

		public bool isGesture;
		public Dictionary<GestureComponent, Path> paths;

		public Node(bool isGesture)
		{
			this.isGesture = isGesture;
			paths = new Dictionary<GestureComponent, Path>();
		}

		public virtual Path getTransition(GestureComponent transitionGestureComponent)
		{
			return paths[transitionGestureComponent];
		}

		public virtual void addGestureComponent(IList<GestureComponent> gestureComponent, Node next)
		{
			paths[gestureComponent[0]] = new Path(gestureComponent, next);
		}

		public virtual int totalGestureComponent()
		{
			return paths.Count;
		}

		public virtual Path getMatchedPath(GestureComponent transitionPath)
		{
			Path newTransition = null;
			long maxTime = 0;
			double maxSpeed = 0.0;
			double minDegree = 180.0D;
			Vec3 anyDirection = new Vec3(0,0,0);
			foreach (GestureComponent gestureComponent in paths.Keys)
			{
	//            System.out.println("MATCHES: " + gestureComponent.matches(transitionPath));
	//            System.out.println("gestureComponent: " + gestureComponent);
	//            System.out.println("transitionPath: " + transitionPath);
				if (gestureComponent.matches(transitionPath))
				{
					MetaData gestureMetaData = new MetaData(gestureComponent.elapsedTime(), gestureComponent.speed(), gestureComponent.direction(), gestureComponent.devicesInProximity());
	//                System.out.println("HERE: " + gestureMetaData.isClosestFit(maxTime, maxSpeed, minDegree, transitionPath.direction()));
	//                System.out.println("minDegree: " + minDegree);
					if (gestureMetaData.isClosestFit(maxTime, maxSpeed, minDegree, transitionPath.direction()))
					{
						maxTime = gestureComponent.elapsedTime();
						maxSpeed = gestureComponent.speed();
						if (!gestureComponent.direction().Equals(anyDirection))
						{
							minDegree = Calcs.getAngle3D(gestureComponent.direction(), transitionPath.direction());
						}
						newTransition = paths[gestureComponent];
	//                    System.out.println("NEW minDegree: " + minDegree);
					}
				}
			}
			return newTransition;
		}

		public override string ToString()
		{
			return "Node[ isGesture=" + isGesture + ", paths=" + paths + "]";
		}
	}

}
