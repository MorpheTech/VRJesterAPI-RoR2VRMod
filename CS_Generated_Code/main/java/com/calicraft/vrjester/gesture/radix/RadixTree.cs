//====================================================================================================
//The Free Edition of Java to C# Converter limits conversion output to 100 lines per file.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================

using System;
using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture.radix
{
	using com.calicraft.vrjester.gesture;


	public class RadixTree
	{
		// Class that represents a gesture pattern namespace with each tree corresponding to a VRDevice

		// NOTE - Gestures are inserted distinctly based on hashed field values in GestureComponent record
		//      However, searches aren't distinct as they match based on order of tree traversal. Whichever
		//      gesture was inserted first will likely get found first since this RadixTree
		//      can store gestures that overlap. This is an intended function of being able to search
		//      using the same "tracked gesture" object based on whether their elapsedTime & speed are within
		//      a "stored gesture's" elapsedTime & speed range.

		public string vrDevice;
		private const int NO_MISMATCH = -1;
		public Node root;

		public RadixTree(string vrDevice)
		{
			this.vrDevice = vrDevice;
			root = new Node(false);
		}

		private int getFirstMismatchGestureComponent(IList<GestureComponent> gesture, IList<GestureComponent> edgeGestureComponent)
		{
			int LENGTH = Math.Min(gesture.Count, edgeGestureComponent.Count);
			for (int i = 1; i < LENGTH; i++)
			{
				if (!gesture[i].Equals(edgeGestureComponent[i]))
				{
					return i;
				}
			}
			return NO_MISMATCH;
		}

		// Helpful method to debug and to see all the gestures
		public virtual void printAllGestures(Dictionary<int, string> gestureMapping)
		{
			printAllGestures(root, new List<GestureComponent>(), gestureMapping);
		}

		private void printAllGestures(Node current, IList<GestureComponent> result, Dictionary<int, string> gestureMapping)
		{
			if (current.isGesture)
			{
				Console.WriteLine(gestureMapping[result.GetHashCode()] + ": " + result);
			}

			foreach (Path path in current.paths.Values)
			{
				printAllGestures(path.next, GestureComponent.concat(result, path.gesture), gestureMapping);
			}
		}

		// Helpful method to debug and to see all the gestures' paths in tree format
		public virtual void printAllPaths()
		{
			printAllPaths(root, "");
		}

		private void printAllPaths(Node current, string indent)
		{
			int lastValue = current.totalGestureComponent() - 1;
			int i = 0;
			foreach (Path path in current.paths.Values)
			{
				if (i == lastValue)
				{
					Console.WriteLine(indent.Replace("+", "L") + path.gesture);
				}
				else
				{
					Console.WriteLine(indent.Replace("+", "|") + path.gesture);
				}
				int length1 = indent.Length / 2 == 0 ? 4 : indent.Length / 2;
				int length2 = path.gesture.ToString().Length / 3;
				string oldIndent = (new string(new char[length1])).Replace("\0", " ");
				string lineIndent = (new string(new char[length2])).Replace("\0", "-");
				string newIndent = oldIndent + "+" + lineIndent + "->";
				i++;
				printAllPaths(path.next, newIndent);
			}
		}

		public virtual void insert(IList<GestureComponent> gesture)
		{
			Node current = root;
			int currIndex = 0;

			//Iterative approach
			while (currIndex < gesture.Count)
			{
				GestureComponent transitionGestureComponent = gesture[currIndex];
				Path currentPath = current.getTransition(transitionGestureComponent);
				//Updated version of the input gesture
				IList<GestureComponent> currGesture = gesture.subList(currIndex, gesture.Count);

				//There is no associated edge with the first character of the current string
				//so simply add the rest of the string and finish
				if (currentPath == null)
				{
					current.paths[transitionGestureComponent] = new Path(currGesture);
					break;
				}

				int splitIndex = getFirstMismatchGestureComponent(currGesture, currentPath.gesture);
				if (splitIndex == NO_MISMATCH)
				{
					//The edge and leftover string are the same length
					//so finish and update the next node as a gesture node
					if (currGesture.Count == currentPath.gesture.Count)
					{
						currentPath.next.isGesture = true;
						break;
					}
					else if (currGesture.Count < currentPath.gesture.Count)
					{
						//The leftover gesture is a prefix to the edge string, so split
						IList<GestureComponent> suffix = currentPath.gesture.subList(currGesture.Count - 1, currGesture.Count);
						currentPath.gesture = currGesture;
						Node newNext = new Node(true);
						Node afterNewNext = currentPath.next;
						currentPath.next = newNext;
						newNext.addGestureComponent(suffix, afterNewNext);
						break;
					}
					else
					{ //currStr.length() > currentEdge.label.length()
						//There is leftover string after a perfect match
						splitIndex = currentPath.gesture.Count;
					}
				}
				else
				{
					//The leftover string and edge string differed, so split at point
					IList<GestureComponent> suffix = currentPath.gesture.subList(splitIndex, currentPath.gesture.Count);
					currentPath.gesture = currentPath.gesture.subList(0, splitIndex);
					Node prevNext = currentPath.next;
					currentPath.next = new Node(false);
					currentPath.next.addGestureComponent(suffix, prevNext);
				}

				//Traverse the tree
				current = currentPath.next;
				currIndex += splitIndex;
			}
		}

		public virtual void delete(IList<GestureComponent> gesture)
		{
			root = delete(root, gesture);
		}

		private Node delete(Node current, IList<GestureComponent> gesture)
		{
			//base case, all the characters have been matched from previous checks
			if (gesture.Count == 0)
			{
				//Has no other edges,
				if (current.paths.Count == 0 && current != root)
				{
					return null;
				}
				current.isGesture = false;
				return current;
			}

			GestureComponent transitionGestureComponent = gesture[0];
			Path path = current.getTransition(transitionGestureComponent);
			//Has no edge for the current gesture or the gesture doesn't exist
			if (path == null || !GestureComponent.startsWith(gesture, path.gesture))
			{
				return current;
			}
			Node deleted = delete(path.next, gesture.subList(path.gesture.Count, gesture.Count));
			if (deleted == null)
			{
				current.paths.Remove(transitionGestureComponent);
				if (current.totalGestureComponent() == 0 && !current.isGesture && current != root)
				{
					return null;
				}
			}
			else if (deleted.totalGestureComponent() == 1 && !deleted.isGesture)
			{
				current.paths.Remove(transitionGestureComponent);
				foreach (Path afterDeleted in deleted.paths.Values)
				{
					current.addGestureComponent(GestureComponent.concat(path.gesture, afterDeleted.gesture), afterDeleted.next);
				}
			}
			return current;
		}

		// Returns matched gesture is found and null if not found
		public virtual IList<GestureComponent> search(IList<GestureComponent> gesture)
		{
			IList<GestureComponent> ret = null;
			Node current = root;

//====================================================================================================
//End of the allowed output for the Free Edition of Java to C# Converter.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================
