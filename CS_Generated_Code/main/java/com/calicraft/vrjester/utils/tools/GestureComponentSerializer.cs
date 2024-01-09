namespace com.calicraft.vrjester.utils.tools
{
	using com.calicraft.vrjester.gesture;
	using com.google.gson;


	public class GestureComponentSerializer : JsonSerializer<GestureComponent>
	{
		public GestureComponentSerializer()
		{
		}

		public virtual JsonElement serialize(GestureComponent src, Type typeOfSrc, JsonSerializationContext context)
		{
			JsonObject obj = new JsonObject();
			JsonObject vec3 = new JsonObject();
			JsonObject devicesInProximity = new JsonObject();

			vec3.addProperty("x", src.direction().x);
			vec3.addProperty("y", src.direction().y);
			vec3.addProperty("z", src.direction().z);

			foreach (string device in src.devicesInProximity().keySet())
			{
				devicesInProximity.addProperty(device, src.devicesInProximity().get(device));
			}

			obj.addProperty("vrDevice", src.vrDevice());
			obj.addProperty("movement", src.movement());
			obj.addProperty("elapsedTime", src.elapsedTime());
			obj.addProperty("speed", src.speed());
			obj.add("direction", vec3);
			obj.add("devicesInProximity", devicesInProximity);
			return obj;
		}
	}

}
