namespace com.calicraft.vrjester.vox
{
	using Constants = com.calicraft.vrjester.config.Constants;
	using VRDevice = com.calicraft.vrjester.utils.vrdata.VRDevice;
	using Vec3 = net.minecraft.world.phys.Vec3;
	using Test = org.junit.jupiter.api.Test;

	using static org.junit.jupiter.api.Assertions.assertTrue;

	public class VhereTest
	{

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void hasPointTest()
		public virtual void hasPointTest()
		{
			Vec3[] centroidPose = new Vec3[2];
			centroidPose[0] = new Vec3(0,0,0);
			centroidPose[1] = new Vec3(0,0,0);
			Vhere testVhere = new Vhere(VRDevice.RIGHT_CONTROLLER, centroidPose, Constants.DEV_CONFIG_PATH);
			Vec3 point = new Vec3(Constants.VIRTUAL_SPHERE_RADIUS, 0, 0);
			assertTrue(testVhere.hasPoint(point));
		}
	}

}
