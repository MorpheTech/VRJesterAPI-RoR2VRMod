using System;

namespace com.calicraft.vrjester.utils.vrdata
{
	using Tracker = com.calicraft.vrjester.tracker.Tracker;
	using IVRPlayer = net.blf02.vrapi.api.data.IVRPlayer;
	using Vec3 = net.minecraft.world.phys.Vec3;

	public class VRDataState
	{
		// Class for encapsulating VRData devices

		private readonly Vec3 origin;
		private readonly Vec3[] hmd, rc, lc, c2;

		public VRDataState(IVRPlayer ivrPlayer)
		{
			origin = new Vec3((0), (0), (0));
			hmd = Tracker.getPose(ivrPlayer.getHMD());
			rc = Tracker.getPose(ivrPlayer.getController0());
			lc = Tracker.getPose(ivrPlayer.getController1());
			c2 = null;
		}

		public override string ToString()
		{
			return "data:" + "\r\n \t origin: " + origin + "\r\n \t hmd: " + "[" + string.Join(", ", hmd) + "]" + "\r\n \t rc: " + "[" + string.Join(", ", rc) + "]" + "\r\n \t lc: " + "[" + string.Join(", ", lc) + "]" + "\r\n \t c2: " + c2;
		}

		public virtual Vec3 Origin
		{
			get
			{
				return origin;
			}
		}
		public virtual Vec3[] Hmd
		{
			get
			{
				return hmd;
			}
		}
		public virtual Vec3[] Rc
		{
			get
			{
				return rc;
			}
		}
		public virtual Vec3[] Lc
		{
			get
			{
				return lc;
			}
		}
		public virtual Vec3[] C2
		{
			get
			{
				return c2;
			}
		}

		// Return position or direction based on VRDevice
		public static Vec3 getVRDevicePose(VRDataState vrDataState, VRDevice vrDevice, int pose)
		{
			Vec3 ret;
			switch (vrDevice)
			{
				case com.calicraft.vrjester.utils.vrdata.VRDevice.HEAD_MOUNTED_DISPLAY:
					ret = vrDataState.Hmd[pose];
					break;
				case com.calicraft.vrjester.utils.vrdata.VRDevice.RIGHT_CONTROLLER:
					ret = vrDataState.Rc[pose];
					break;
				case com.calicraft.vrjester.utils.vrdata.VRDevice.LEFT_CONTROLLER:
					ret = vrDataState.Lc[pose];
					break;
				case com.calicraft.vrjester.utils.vrdata.VRDevice.EXTRA_TRACKER:
					ret = vrDataState.C2[pose];
					break;
				default:
				{
					Console.Error.WriteLine("VRDevice not yet supported!");
					ret = new Vec3((0), (0), (0));

						break;
				}
			}
			return ret;
		}

		// Return pose based on VRDevice
		public static Vec3[] getVRDevicePose(VRDataState vrDataState, VRDevice vrDevice)
		{
			Vec3[] ret;
			switch (vrDevice)
			{
				case com.calicraft.vrjester.utils.vrdata.VRDevice.HEAD_MOUNTED_DISPLAY:
					ret = vrDataState.Hmd;
					break;
				case com.calicraft.vrjester.utils.vrdata.VRDevice.RIGHT_CONTROLLER:
					ret = vrDataState.Rc;
					break;
				case com.calicraft.vrjester.utils.vrdata.VRDevice.LEFT_CONTROLLER:
					ret = vrDataState.Lc;
					break;
				case com.calicraft.vrjester.utils.vrdata.VRDevice.EXTRA_TRACKER:
					ret = vrDataState.C2;
					break;
				default:
				{
					Console.Error.WriteLine("VRDevice not yet supported!");
					ret = new Vec3[]
					{
						new Vec3((0), (0), (0)),
						new Vec3((0), (0), (0))
					};

						break;
				}
			}
			return ret;
		}
	}

}
