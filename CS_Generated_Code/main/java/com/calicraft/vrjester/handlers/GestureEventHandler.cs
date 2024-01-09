using System;
using System.Collections.Generic;

namespace com.calicraft.vrjester.handlers
{
	using GestureEventCallback = com.calicraft.vrjester.api.GestureEventCallback;
	using InputConstants = com.mojang.blaze3d.platform.InputConstants;
	using ClientRawInputEvent = dev.architectury.@event.events.client.ClientRawInputEvent;
	using ClientTickEvent = dev.architectury.@event.events.client.ClientTickEvent;
	using ExpectPlatform = dev.architectury.injectables.annotations.ExpectPlatform;
	using KeyMapping = net.minecraft.client.KeyMapping;


	using static com.calicraft.vrjester.VrJesterApi.KEY_MAPPINGS;
	using static com.calicraft.vrjester.VrJesterApi.getMCI;

	public class GestureEventHandler
	{
		// A class for handling received GestureEvents

		private static bool nextTick = false;
		private static IList<string> gestureKeyMappings;

		public static void init()
		{
			GestureEventCallback.EVENT.register((gestureEvent) =>
			{
			handleGestureEvent(gestureEvent.getGestureName());
			});
			ClientTickEvent.CLIENT_POST.register(minecraft =>
			{
			if (nextTick)
			{
				triggerKeyPress(KEY_MAPPINGS[gestureKeyMappings.RemoveAndReturn(0)]);
				if (gestureKeyMappings.Count == 0)
				{
					nextTick = false;
				}
			}
			});
		}

		public static void handleGestureEvent(string gestureName)
		{
			Console.WriteLine("GESTURE EVENT POSTED & RECEIVED! " + gestureName);
			Dictionary<string, string> gestureAction = TriggerEventHandler.config.GESTURE_KEY_MAPPINGS[gestureName];
			if (gestureAction != null)
			{
				string gestureMapping = gestureAction["KEY_MAPPING"];
				gestureKeyMappings = new LinkedList<string>(List.of(gestureMapping.Split(",", true)));
				if (gestureKeyMappings.Count > 1 && gestureKeyMappings[0].Contains("key.hotbar"))
				{
					// Ensure first key is pressed first if it's a hot bar key
					triggerKeyPress(KEY_MAPPINGS[gestureKeyMappings.RemoveAndReturn(0)]);
					nextTick = true;
				}
				else
				{ // Perform key presses as usual
					foreach (string gestureKeyMapping in gestureKeyMappings)
					{
						triggerKeyPress(KEY_MAPPINGS[gestureKeyMapping]);
					}
				}
			}
		}

		public static void triggerKeyPress(KeyMapping keyMapping)
		{
			if (keyMapping != null)
			{
				if (keyMapping.key.getType() == InputConstants.Type.MOUSE)
				{
					keyMapping.setDown(true);
					ClientRawInputEvent.MOUSE_CLICKED_PRE.invoker().mouseClicked(VrJesterApi.MCI, keyMapping.key.getValue(), 1, 0);
					KeyMapping.click(keyMapping.key);
				}
				else
				{
					VrJesterApi.MCI.keyboardHandler.keyPress(VrJesterApi.MCI.getWindow().getWindow(), keyMapping.key.getValue(), 0, 1, 0);
				}
			}
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ExpectPlatform public static void bruteForceKeyPress(net.minecraft.client.KeyMapping keyMapping)
		public static void bruteForceKeyPress(KeyMapping keyMapping)
		{
			throw new AssertionError();
		}
	}
}
