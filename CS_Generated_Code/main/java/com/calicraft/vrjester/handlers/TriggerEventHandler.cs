//====================================================================================================
//The Free Edition of Java to C# Converter limits conversion output to 100 lines per file.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace com.calicraft.vrjester.handlers
{
	using VrJesterApi = com.calicraft.vrjester.VrJesterApi;
	using GestureEvent = com.calicraft.vrjester.api.GestureEvent;
	using GestureEventCallback = com.calicraft.vrjester.api.GestureEventCallback;
	using Config = com.calicraft.vrjester.config.Config;
	using Constants = com.calicraft.vrjester.config.Constants;
	using Gesture = com.calicraft.vrjester.gesture.Gesture;
	using Gestures = com.calicraft.vrjester.gesture.Gestures;
	using Recognition = com.calicraft.vrjester.gesture.recognition.Recognition;
	using PositionTracker = com.calicraft.vrjester.tracker.PositionTracker;
	using TestJester = com.calicraft.vrjester.utils.demo.TestJester;
	using VRDataAggregator = com.calicraft.vrjester.utils.vrdata.VRDataAggregator;
	using VRDataState = com.calicraft.vrjester.utils.vrdata.VRDataState;
	using VRDataType = com.calicraft.vrjester.utils.vrdata.VRDataType;
	using InputConstants = com.mojang.blaze3d.platform.InputConstants;
	using EventResult = dev.architectury.@event.EventResult;
	using ClientRawInputEvent = dev.architectury.@event.events.client.ClientRawInputEvent;
	using ClientTickEvent = dev.architectury.@event.events.client.ClientTickEvent;
	using KeyMapping = net.minecraft.client.KeyMapping;
	using LocalPlayer = net.minecraft.client.player.LocalPlayer;
	using Component = net.minecraft.network.chat.Component;

//JAVA TO C# CONVERTER TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.calicraft.vrjester.VrJesterApi.*;


	public class TriggerEventHandler
	{
		private static bool msgSentOnce = false;
		private static readonly TestJester test = new TestJester();

		public static Config config = Config.readConfig();
		private static VRDataState vrDataRoomPre;
		private static VRDataState vrDataWorldPre;
		private static readonly VRDataAggregator preRoomDataAggregator = new VRDataAggregator(VRDataType.VRDATA_ROOM_PRE, false);
		private static readonly VRDataAggregator preWorldDataAggregator = new VRDataAggregator(VRDataType.VRDATA_WORLD_PRE, false);
		private static readonly int DELAY = config.INTERVAL_DELAY; // 0.75 second (15 ticks)
		private static int sleep = DELAY;
		private static int limiter = config.MAX_LISTENING_TIME; // 10 seconds (400 ticks)
		private static bool fireEventThisTick = false;
		private static bool listening = false;
		private static bool nonVrListening = false;
		public static bool oneRecorded = false;
		private static long elapsedTime = 0;
		private static string previousGesture = "";
		private static Gesture gesture;
		public static readonly Gestures gestures = new Gestures(config, Constants.GESTURE_STORE_PATH);
		private static readonly Recognition recognition = new Recognition(gestures);
		private static LocalPlayer player;

		public static void init()
		{
			ClientRawInputEvent.KEY_PRESSED.register((client, keyCode, scanCode, action, modifiers) =>
			{
//            if (keyCode != 71)
//                System.out.println("KEY CODE: " + keyCode + " | ACTION: " + action);
			if (keyCode == MOD_KEY.key.getValue())
			{
				if (jesterSetupComplete())
				{
					// Trigger the gesture listening phase
					if (VIVECRAFT_LOADED)
					{
						handleVrJester();
					}
					else
					{
						handleNonVrJester();
					}
				}
			}
			return EventResult.pass();
			});
			ClientTickEvent.CLIENT_POST.register(minecraft =>
			{
			gestureListener();
			});
		}

		private static void gestureListener()
		{
			if (listening)
			{ // Capture VR data in real time after trigger
				vrDataRoomPre = preRoomDataAggregator.listen();
				vrDataWorldPre = preWorldDataAggregator.listen();
				if (gesture == null)
				{ // For initial tick from trigger
					gesture = new Gesture(vrDataRoomPre);
				}
				else
				{
					gesture.track(vrDataRoomPre);
				}
				if (config.RECOGNIZE_ON.Equals("RECOGNIZE"))
				{ // Recognize gesture within delay interval.
					// If a gesture is recognized, wait for the next interval to see if another gesture is recognized
					Dictionary<string, string> recognizedGesture = recognition.recognize(gesture);
					if (sleep != 0)
					{ // Execute every tick
						if (recognizedGesture.Count > 0)
						{
							Dictionary<string, string> gestureAction = config.GESTURE_KEY_MAPPINGS[recognizedGesture["gestureName"]];
							if (!previousGesture.Equals(recognizedGesture["gestureName"]))
							{ // Initial gesture recognition
								previousGesture = recognizedGesture["gestureName"];
								GestureEventCallback.EVENT.invoker().post(new GestureEvent(recognizedGesture, gesture, vrDataRoomPre, vrDataWorldPre));
								sleep = DELAY; // Reset ticker to extend listening time
								limiter = config.MAX_LISTENING_TIME; // Reset limiter
							}
							else
							{ // Handle gesture recognition after initial if gesture hasn't changed (hold down key)
								if (gestureAction != null && gestureAction["KEY_ACTION"].Equals("hold"))
								{
									GestureEventCallback.EVENT.invoker().post(new GestureEvent(recognizedGesture, gesture, vrDataRoomPre, vrDataWorldPre));
									sleep = DELAY; // Reset ticker to extend listening time
									limiter = config.MAX_LISTENING_TIME; // Reset limiter
								}
							}
						}
					}
					else
					{ // Reset trigger at the end of the delay interval
	//                    System.out.println("JESTER DONE LISTENING");
						sleep = DELAY;
						if (recognizedGesture.Count > 0)
						{ // Final gesture recognition check after delay interval reset
							if (!previousGesture.Equals(recognizedGesture["gestureName"]))
							{
								GestureEventCallback.EVENT.invoker().post(new GestureEvent(recognizedGesture, gesture, vrDataRoomPre, vrDataWorldPre));
								if (config.DEBUG_MODE)
								{
									sendDebugMsg("RECOGNIZED: " + recognizedGesture["gestureName"]);
								}
								if (config.DEMO_MODE)
								{
									test.trigger(recognizedGesture, vrDataWorldPre, config);
								}
								limiter = config.MAX_LISTENING_TIME;
								stopJesterListener();
							}
						}
					}

//====================================================================================================
//End of the allowed output for the Free Edition of Java to C# Converter.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================
