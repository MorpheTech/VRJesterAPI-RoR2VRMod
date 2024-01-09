﻿using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture
{
	using Constants = com.calicraft.vrjester.config.Constants;
	using Vec3 = com.calicraft.vrjester.utils.tools.Vec3;
	using VRDevice = com.calicraft.vrjester.utils.vrdata.VRDevice;
	using Test = org.junit.jupiter.api.Test;


	using static org.junit.jupiter.api.Assertions.assertEquals;

	public class GestureTraceTest
	{

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void completeTraceTest()
		internal virtual void completeTraceTest()
		{
			Vec3[] centroidPose = new Vec3[2]; //initialized array of 2 vectors
			centroidPose[0] = new Vec3(0,0,1);
			centroidPose[1] = new Vec3(0,0,1);
			Vec3[] endPose = new Vec3[2]; //initialized array of 2 vectors
			endPose[0] = new Vec3(100,0,1000);
			endPose[1] = (new Vec3(0,0,1)).normalize();
			Dictionary<string, int> gesturesTraced = new Dictionary<string, int>();
			Vec3 facingDirection = new Vec3(1,0,2);
			GestureTrace ges = new GestureTrace("000", VRDevice.RIGHT_CONTROLLER, centroidPose, facingDirection);
			ges.completeTrace(endPose);
			GestureComponent val1 = new GestureComponent(Constants.RC, "forward", 0, 0.0, centroidPose[1], gesturesTraced);
			GestureComponent val2 = ges.toGestureComponent();
			assertEquals(val1.movement(), val2.movement());
			assertEquals(val1.vrDevice(), val2.vrDevice());
			assertEquals(val1.direction(), val2.direction());

		}
	}

}
