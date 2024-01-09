using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture.Radix
{
	using Constants = com.calicraft.vrjester.config.Constants;
	using com.calicraft.vrjester.gesture;
	using RadixTree = com.calicraft.vrjester.gesture.radix.RadixTree;
	using Vec3 = com.calicraft.vrjester.utils.tools.Vec3;
	using BeforeAll = org.junit.jupiter.api.BeforeAll;
	using Test = org.junit.jupiter.api.Test;


	using static org.junit.jupiter.api.Assertions.assertEquals;

	public class RadixTreeTest
	{
		public static RadixTree TreeTest = new RadixTree("vrDevice");
		public static IList<GestureComponent> TreeArray = new List<GestureComponent>();

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeforeAll public static void setup()
		public static void setup()
		{
			IDictionary<string, int> HM = new Dictionary<string, int>();
			Vec3 v = new Vec3(1,1,1);
			GestureComponent punch = new GestureComponent(Constants.LC, "up", 5, 1.5,v, HM);
			TreeArray.Add(punch);
			TreeTest.insert(TreeArray);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void insertTest()
		public virtual void insertTest()
		{
			IList<GestureComponent> array = new List<GestureComponent>();
			IDictionary<string, int> HM = new Dictionary<string, int>();
			Vec3 v = new Vec3(0,1,0);
			GestureComponent punch = new GestureComponent(Constants.LC, "up", 2, 2.5,v, HM);
			array.Add(punch);
			TreeTest.insert(array);
			assertEquals(array, TreeTest.search(array));
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void searchTest()
		public virtual void searchTest()
		{
			IList<GestureComponent> array = new List<GestureComponent>();
			IDictionary<string, int> HM = new Dictionary<string, int>();
			Vec3 v = new Vec3(1,1,3);
			GestureComponent punch = new GestureComponent(Constants.LC, "up", 6, 3.5, v, HM);
			array.Add(punch);
			assertEquals(TreeArray, TreeTest.search(array));

		}
	}

}
