using System;
using System.Collections.Generic;
using System.Linq;


namespace VRJester.Core.Radix {

    public class RadixTree(string vrDevice) {
        // Class that represents a gesture pattern namespace with each tree corresponding to a VRDevice

        // NOTE - Gestures are inserted distinctly based on Hashed field values in GestureComponent record
        //      However, searches aren't distinct as they match based on order of tree traversal. Whichever
        //      gesture was inserted first will likely get found first since this RadixTree
        //      can store gestures that overlap. This is an intended function of being able to search
        //      using the same "tracked gesture" object based on whether their elapsedTime & speed are within
        //      a "stored gesture's" elapsedTime & speed range.

        public string vrDevice = vrDevice;
        private const int NO_MISMATCH = -1;
        public Node root = new(false);

        private int GetFirstMismatchGestureComponent(List<GestureComponent> gesture, List<GestureComponent> edgeGestureComponent) {
            int LENGTH = Math.Min(gesture.Count, edgeGestureComponent.Count);
            for (int i = 1; i < LENGTH; i++) {
                if (!gesture[i].Equals(edgeGestureComponent[i])) {
                    return i;
                }
            }
            return NO_MISMATCH;
        }

        // Helpful method to debug and to see all the gestures
        public virtual void PrintAllGestures(Dictionary<int, string> gestureMapping) {
            PrintAllGestures(root, [], gestureMapping);
        }

        private void PrintAllGestures(Node current, List<GestureComponent> result, Dictionary<int, string> gestureMapping) {
            if (current.isGesture) {
                gestureMapping.TryGetValue(result.HashCode(), out string gestureName);
                string components = string.Join(",", result.Select(c => c.ToString()));
                Log.Info(gestureName + ": " + components);
            }

            foreach (Branch path in current.paths.Values) {
                PrintAllGestures(path.next, GestureComponent.Concat(result, path.gesture), gestureMapping);
            }
        }

        // Helpful method to debug and to see all the gestures' paths in tree format
        public virtual void PrintAllPaths() {
            PrintAllPaths(root, "");
        }

        private void PrintAllPaths(Node current, string indent) {
            int lastValue = current.TotalGestureComponent() - 1;
            int i = 0;
            foreach (Branch path in current.paths.Values) {
                if (i == lastValue) {
                    Log.Info(indent.Replace("+", "L") + string.Join(",", path.gesture));
                }
                else {
                    Log.Info(indent.Replace("+", "|") + string.Join(",", path.gesture));
                }
                int length1 = indent.Length / 2 == 0 ? 4 : indent.Length / 2;
                int length2 = path.gesture.ToString().Length / 3;
                string oldIndent = new string(new char[length1]).Replace("\0", " ");
                string lineIndent = new string(new char[length2]).Replace("\0", "-");
                string newIndent = oldIndent + "+" + lineIndent + "->";
                i++;
                PrintAllPaths(path.next, newIndent);
            }
        }

        public virtual void Insert(List<GestureComponent> gesture) {
            Node current = root;
            int currIndex = 0;

            // Iterative approach
            while (currIndex < gesture.Count) {
                GestureComponent transitionGestureComponent = gesture[currIndex];
                Branch currentPath = current.GetTransition(transitionGestureComponent);
                // Log.Debug("gesture:");
                // foreach (GestureComponent item in gesture) Log.Debug(item);
                // Log.Debug("currIndex: " + currIndex + " | count: " + (gesture.Count - currIndex));
                // Updated version of the input gesture
                List<GestureComponent> currGesture = gesture.GetRange(currIndex, gesture.Count - currIndex);

                // There is no associated edge with the first character of the current string
                // so simply add the rest of the string and finish
                if (currentPath == null) {
                    current.paths[transitionGestureComponent] = new Branch(currGesture);
                    break;
                }

                int splitIndex = GetFirstMismatchGestureComponent(currGesture, currentPath.gesture);
                if (splitIndex == NO_MISMATCH) {
                    // The edge and leftover string are the same length
                    // so finish and update the next node as a gesture node
                    if (currGesture.Count == currentPath.gesture.Count) {
                        currentPath.next.isGesture = true;
                        break;
                    }
                    else if (currGesture.Count < currentPath.gesture.Count) {
                        // The leftover gesture is a prefix to the edge string, so split
                        List<GestureComponent> suffix = currentPath.gesture.GetRange(currGesture.Count - 1, currGesture.Count);
                        currentPath.gesture = currGesture;
                        Node newNext = new Node(true);
                        Node afterNewNext = currentPath.next;
                        currentPath.next = newNext;
                        newNext.AddGestureComponent(suffix, afterNewNext);
                        break;
                    }
                    else { // currStr.length() > currentEdge.label.length()
                        // There is leftover string after a perfect match
                        splitIndex = currentPath.gesture.Count;
                    }
                }
                else {
                    // The leftover string and edge string differed, so split at point
                    List<GestureComponent> suffix = currentPath.gesture.GetRange(splitIndex, currentPath.gesture.Count);
                    currentPath.gesture = currentPath.gesture.GetRange(0, splitIndex);
                    Node prevNext = currentPath.next;
                    currentPath.next = new Node(false);
                    currentPath.next.AddGestureComponent(suffix, prevNext);
                }

                // Traverse the tree
                current = currentPath.next;
                currIndex += splitIndex;
            }
        }

        public virtual void Delete(List<GestureComponent> gesture) {
            root = Delete(root, gesture);
        }

        private Node Delete(Node current, List<GestureComponent> gesture) {
            // Base case, all the characters have been matched from previous checks
            if (gesture.Count == 0) {
                // Has no other edges,
                if (current.paths.Count == 0 && current != root) {
                    return null;
                }
                current.isGesture = false;
                return current;
            }

            GestureComponent transitionGestureComponent = gesture[0];
            Branch path = current.GetTransition(transitionGestureComponent);
            // Has no edge for the current gesture or the gesture doesn't exist
            if (path == null || !GestureComponent.StartsWith(gesture, path.gesture)) {
                return current;
            }
            Node deleted = Delete(path.next, gesture.GetRange(path.gesture.Count, gesture.Count));
            if (deleted == null) {
                current.paths.Remove(transitionGestureComponent);
                if (current.TotalGestureComponent() == 0 && !current.isGesture && current != root) {
                    return null;
                }
            }
            else if (deleted.TotalGestureComponent() == 1 && !deleted.isGesture) {
                current.paths.Remove(transitionGestureComponent);
                foreach (Branch afterDeleted in deleted.paths.Values) {
                    current.AddGestureComponent(GestureComponent.Concat(path.gesture, afterDeleted.gesture), afterDeleted.next);
                }
            }
            return current;
        }

        // Returns matched gesture if found and null if not found
        public virtual List<GestureComponent> Search(List<GestureComponent> gesture) {
            List<GestureComponent> ret = null;
            Node current = root;
            int currIndex = 0;

            while (currIndex < gesture.Count) {
                GestureComponent currentGestureComponent = gesture[currIndex];
                Branch path = current.GetMatchedPath(currentGestureComponent);
                if (path == null) {
                    return null;
                }

                List<GestureComponent> currSubGesture = gesture.GetRange(currIndex, gesture.Count - currIndex);
                if (!GestureComponent.MatchesWith(currSubGesture, path.gesture)) {
                    return null;
                }

                currIndex += path.gesture.Count;
                current = path.next;
                ret = GestureComponent.Concat(ret, path.gesture);
            }
            return ret;
        }
    }
}