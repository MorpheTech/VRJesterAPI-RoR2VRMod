using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture.radix
{
	using com.calicraft.vrjester.gesture;

	public class Path
	{
		// Class that represents a valid gesture path leading from a previous Node and stores the GestureComponent(s)

		public IList<GestureComponent> gesture;
		public Node next;

		public Path(IList<GestureComponent> gesture) : this(gesture, new Node(true))
		{
		}

		public Path(IList<GestureComponent> gesture, Node next)
		{
			this.gesture = gesture;
			this.next = next;
		}

		public override string ToString()
		{
			return "Path[gesture=" + gesture + "]";
		}

	}

}
