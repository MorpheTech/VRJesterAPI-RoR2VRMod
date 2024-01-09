using System.Collections.Generic;
using System.Diagnostics;

namespace com.calicraft.vrjester.utils.vrdata
{
	using PositionTracker = com.calicraft.vrjester.tracker.PositionTracker;
	using IVRPlayer = net.blf02.vrapi.api.data.IVRPlayer;


	public class VRDataAggregator
	{
		// Class for consuming VR Data from Tracker
		private readonly IList<VRDataState> data = new List<VRDataState>();
		private readonly VRDataType vrDataType;
		private readonly bool saveState;

		public VRDataAggregator(VRDataType vrDataType, bool saveState)
		{
			this.vrDataType = vrDataType;
			this.saveState = saveState;
		}

		public virtual IList<VRDataState> Data
		{
			get
			{
				return data;
			}
		}

		// Consume Vivecraft VRDevicePose data from TRACKER
		public virtual VRDataState listen()
		{
			IVRPlayer ivrPlayer = vrDataType switch
			{
				com.calicraft.vrjester.utils.vrdata.VRDataType.VRDATA_ROOM_PRE => PositionTracker.VRDataRoomPre,
				com.calicraft.vrjester.utils.vrdata.VRDataType.VRDATA_WORLD_PRE => PositionTracker.VRDataWorldPre,
				_ => null
			};
			Debug.Assert(ivrPlayer != null);
			VRDataState dataState = new VRDataState(ivrPlayer);
			if (saveState)
			{
				data.Add(dataState);
			}
			return dataState;
		}

		// Clear data only after sending
		public virtual void clear()
		{
			data.Clear();
		}
	}

}
