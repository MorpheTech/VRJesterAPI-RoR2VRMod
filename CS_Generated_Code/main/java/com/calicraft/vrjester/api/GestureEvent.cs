using System.Collections.Generic;

namespace com.calicraft.vrjester.api
{
	using Gesture = com.calicraft.vrjester.gesture.Gesture;
	using VRDataState = com.calicraft.vrjester.utils.vrdata.VRDataState;
	using Player = net.minecraft.world.entity.player.Player;

	public class GestureEvent : VRPlayerEvent
	{
		// This class packages gestures that were recognized along with the attributes that come with it

		private readonly string gestureName;
		private readonly Gesture gesture;

		public GestureEvent(Dictionary<string, string> gestureCtx, Gesture gesture, VRDataState vrDataRoomPre, VRDataState vrDataWorldPre) : base(vrDataRoomPre, vrDataWorldPre)
		{
			this.gestureName = gestureCtx["gestureName"];
			this.gesture = gesture;
		}

		public virtual string GestureName
		{
			get
			{
				return gestureName;
			}
		}

		public virtual Gesture Gesture
		{
			get
			{
				return gesture;
			}
		}
	}

}
