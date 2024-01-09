namespace com.calicraft.vrjester.tracker
{
	using Minecraft = net.minecraft.client.Minecraft;

	public class VRPluginStatus
	{
		public static bool hasPlugin = false;

		public static bool clientInVR()
		{
			return hasPlugin && checkPlayerInVR();
		}

		public static bool checkPlayerInVR()
		{
			return Minecraft.getInstance().player == null || PositionTracker.vrAPI.playerInVR(Minecraft.getInstance().player);
		}
	}
}
