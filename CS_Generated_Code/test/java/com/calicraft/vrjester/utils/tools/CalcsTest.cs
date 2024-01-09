namespace com.calicraft.vrjester.utils.tools
{
	using Test = org.junit.jupiter.api.Test;

	using static org.junit.jupiter.api.Assertions.assertEquals;
	using static org.junit.jupiter.api.Assertions.assertTrue;

	public class CalcsTest
	{

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void getMagnitude2DTest()
		public virtual void getMagnitude2DTest()
		{
			Vec3 v = new Vec3(1, 2, 3);
			assertEquals(3.1622776601683795,Calcs.getMagnitude2D(v));
		}
//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void getMagnitude3dTest()
		public virtual void getMagnitude3dTest()
		{
			Vec3 v = new Vec3(1,1,1);
			assertEquals(1.7320508075688772, Calcs.getMagnitude3D(v));
		}
//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void getAngle2DTest()
		public virtual void getAngle2DTest()
		{
			Vec3 v1 = new Vec3(1,1,1);
			Vec3 v2 = new Vec3(1,1,1);
			assertEquals(1.2074182697257333E-6, Calcs.getAngle2D(v1, v2));
		}
//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void getAngle3DTest()
		public virtual void getAngle3DTest()
		{
			Vec3 v1 = new Vec3(1,2,1);
			Vec3 v2 = new Vec3(2,1,2);
			assertEquals(35.26438968275465, Calcs.getAngle3D(v1, v2));
		}
	}

}
