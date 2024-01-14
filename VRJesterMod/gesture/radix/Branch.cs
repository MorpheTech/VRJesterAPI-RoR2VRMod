using System.Collections.Generic;


namespace VRJester.Core.Radix {

    public class Branch(List<GestureComponent> gesture, Node next) {
        // Class that represents a valid gesture path leading from a previous Node and stores the GestureComponent(s)

        public List<GestureComponent> gesture = gesture;
        public Node next = next;

        public Branch(List<GestureComponent> gesture) : this(gesture, new Node(true)) {}

        public override string ToString() {
            return "Branch[gesture=" + gesture + "]";
        }
    }
}
