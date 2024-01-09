namespace com.calicraft.vrjester.api
{
	using Event = dev.architectury.@event.Event;
	using EventFactory = dev.architectury.@event.EventFactory;

	public interface GestureEventCallback
	{
		internal static Event<GestureEventCallback> EVENT = EventFactory.createLoop();

		void post(GestureEvent gestureEvent);
	}

}
