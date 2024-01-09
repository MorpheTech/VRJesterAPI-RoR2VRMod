using System.Collections.Generic;

namespace com.calicraft.vrjester.utils.tools
{
	using com.calicraft.vrjester.gesture;
	using com.google.gson;


	public class GestureComponentDeserializer : JsonDeserializer<GestureComponent>
	{
		public GestureComponentDeserializer()
		{
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public com.calicraft.vrjester.gesture.GestureComponent deserialize(JsonElement json, java.lang.reflect.Type typeOfT, JsonDeserializationContext context) throws JsonParseException
		public virtual GestureComponent deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context)
		{
			JsonObject jsonObject = json.getAsJsonObject();
			JsonObject direction = jsonObject.get("direction").getAsJsonObject();
			JsonObject devices = jsonObject.get("devicesInProximity").getAsJsonObject();
			Dictionary<string, int> devicesInProximity = new Dictionary<string, int>();

			foreach (string device in devices.keySet())
			{
				devicesInProximity[device] = devices.get(device).getAsInt();
			}

			Vec3 vec3 = new Vec3(direction.get("x").getAsDouble(), direction.get("y").getAsDouble(), direction.get("z").getAsDouble());

			return new GestureComponent(jsonObject.get("vrDevice").getAsString(), jsonObject.get("movement").getAsString(), jsonObject.get("elapsedTime").getAsLong(), jsonObject.get("speed").getAsDouble(), vec3, devicesInProximity);
		}
	}

}
