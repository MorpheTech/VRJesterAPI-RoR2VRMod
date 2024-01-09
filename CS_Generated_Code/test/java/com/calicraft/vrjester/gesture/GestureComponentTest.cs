using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture
{
	using Constants = com.calicraft.vrjester.config.Constants;
	using Vec3 = com.calicraft.vrjester.utils.tools.Vec3;
	using Test = org.junit.jupiter.api.Test;

	using static org.junit.jupiter.api.Assertions.assertEquals;
	using static org.junit.jupiter.api.Assertions.assertTrue;

	public class GestureComponentTest
	{

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void equalsTest()
		public virtual void equalsTest()
		{
			Vec3 dir = new Vec3((0),(0),(0));
			Dictionary<string, int> devices = new Dictionary<string, int>();
			GestureComponent storedGesture = new GestureComponent(Constants.RC, "forward", 0, 0.0, dir, devices);
			GestureComponent userGesture = new GestureComponent(Constants.RC, "forward", 0, 0.0, dir, devices);
			assertEquals(storedGesture, userGesture);

		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void matchesTest()
		public virtual void matchesTest()
		{
			Vec3 dir = new Vec3((0),(0),(0));
			Dictionary<string, int> devicesInProximity = new Dictionary<string, int>();
			devicesInProximity[Constants.LC] = 0;
			GestureComponent storedGesture = new GestureComponent(Constants.RC, "forward", 0, 0.0, dir, devicesInProximity);
			GestureComponent userGesture = new GestureComponent(Constants.RC, "forward", 5, 5.0, dir, devicesInProximity);
			assertTrue(storedGesture.matches(userGesture));
		}

	}

}
