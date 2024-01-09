namespace com.calicraft.vrjester.api
{
	using VRDataState = com.calicraft.vrjester.utils.vrdata.VRDataState;
	using PlayerEvent = dev.architectury.@event.events.common.PlayerEvent;

	public class VRPlayerEvent
	{
		// This class packages the VRData of the player

		private readonly VRDataState vrDataRoomPre;
		private readonly VRDataState vrDataWorldPre;

		public VRPlayerEvent(VRDataState vrDataRoomPre, VRDataState vrDataWorldPre)
		{
			this.vrDataRoomPre = vrDataRoomPre;
			this.vrDataWorldPre = vrDataWorldPre;
		}

		public virtual VRDataState VrDataRoomPre
		{
			get
			{
				return vrDataRoomPre;
			}
		}

		public virtual VRDataState VrDataWorldPre
		{
			get
			{
				return vrDataWorldPre;
			}
		}
	}

}
