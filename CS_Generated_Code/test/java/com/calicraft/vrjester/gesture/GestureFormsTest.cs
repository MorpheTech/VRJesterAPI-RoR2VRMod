//====================================================================================================
//The Free Edition of Java to C# Converter limits conversion output to 100 lines per file.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================

using System;
using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture
{
	using Config = com.calicraft.vrjester.config.Config;
	using Constants = com.calicraft.vrjester.config.Constants;
	using Recognition = com.calicraft.vrjester.gesture.recognition.Recognition;
	using Calcs = com.calicraft.vrjester.utils.tools.Calcs;
	using Vec3 = com.calicraft.vrjester.utils.tools.Vec3;
	using BeforeAll = org.junit.jupiter.api.BeforeAll;
	using Test = org.junit.jupiter.api.Test;


	using static org.junit.jupiter.api.Assertions.assertEquals;

	internal class GestureFormsTest
	{
		private static readonly Config devConfig = Config.readConfig(Constants.DEV_CONFIG_PATH);
		private static readonly Gestures gestures = new Gestures(devConfig, Constants.DEV_GESTURE_STORE_PATH);
		private static readonly Recognition recognition = new Recognition(gestures);

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeforeAll static void checkDevConfig()
		internal static void checkDevConfig()
		{
			gestures.load();
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void testStrikeGesture()
		internal virtual void testStrikeGesture()
		{
			IList<GestureComponent> hmdGesture = new List<GestureComponent>();
			IList<GestureComponent> rcGesture = new List<GestureComponent>();
			IList<GestureComponent> lcGesture = new List<GestureComponent>();
			Vec3 dir = new Vec3((0), (0), (0));
			Dictionary<string, int> devices = new Dictionary<string, int>();
			GestureComponent gestureComponent1 = new GestureComponent(Constants.RC, "forward", 0, 0.0, dir, devices);
			rcGesture.Add(gestureComponent1);
			Gesture strikeGesture = new Gesture(hmdGesture, rcGesture, lcGesture);
			assertEquals("STRIKE", recognition.recognize(strikeGesture)["gestureName"]);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void testPushGesture()
		internal virtual void testPushGesture()
		{
			IList<GestureComponent> hmdGesture = new List<GestureComponent>();
			IList<GestureComponent> rcGesture = new List<GestureComponent>();
			IList<GestureComponent> lcGesture = new List<GestureComponent>();
			Vec3 dir = new Vec3((0), (0), (0));
			Dictionary<string, int> devices = new Dictionary<string, int>();
			GestureComponent gestureComponent1 = new GestureComponent(Constants.RC, "forward", 0, 0.0, dir, devices);
			GestureComponent gestureComponent2 = new GestureComponent(Constants.LC, "forward", 0, 0.0, dir, devices);
			rcGesture.Add(gestureComponent1);
			lcGesture.Add(gestureComponent2);
			Gesture pushGesture = new Gesture(hmdGesture, rcGesture, lcGesture);
			assertEquals("PUSH", recognition.recognize(pushGesture)["gestureName"]);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void testPullGesture()
		internal virtual void testPullGesture()
		{
			IList<GestureComponent> hmdGesture = new List<GestureComponent>();
			IList<GestureComponent> rcGesture = new List<GestureComponent>();
			IList<GestureComponent> lcGesture = new List<GestureComponent>();
			Vec3 dir = new Vec3((0), (0), (0));
			Dictionary<string, int> devices = new Dictionary<string, int>();
			GestureComponent gestureComponent1 = new GestureComponent(Constants.RC, "back", 0, 0.0, dir, devices);
			GestureComponent gestureComponent2 = new GestureComponent(Constants.LC, "back", 0, 0.0, dir, devices);
			rcGesture.Add(gestureComponent1);
			lcGesture.Add(gestureComponent2);
			Gesture lowerGesture = new Gesture(hmdGesture, rcGesture, lcGesture);
			assertEquals("PULL", recognition.recognize(lowerGesture)["gestureName"]);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void testLowerGesture()
		internal virtual void testLowerGesture()
		{
			IList<GestureComponent> hmdGesture = new List<GestureComponent>();
			IList<GestureComponent> rcGesture = new List<GestureComponent>();
			IList<GestureComponent> lcGesture = new List<GestureComponent>();
			Vec3 dir = new Vec3((0), (0), (0));
			Dictionary<string, int> devices = new Dictionary<string, int>();
			GestureComponent gestureComponent1 = new GestureComponent(Constants.RC, "down", 0, 0.0, dir, devices);
			GestureComponent gestureComponent2 = new GestureComponent(Constants.LC, "down", 0, 0.0, dir, devices);
			rcGesture.Add(gestureComponent1);
			lcGesture.Add(gestureComponent2);
			Gesture lowerGesture = new Gesture(hmdGesture, rcGesture, lcGesture);
			assertEquals("LOWER", recognition.recognize(lowerGesture)["gestureName"]);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void testRaiseGesture()
		internal virtual void testRaiseGesture()
		{
			IList<GestureComponent> hmdGesture = new List<GestureComponent>();
			IList<GestureComponent> rcGesture = new List<GestureComponent>();
			IList<GestureComponent> lcGesture = new List<GestureComponent>();
			Vec3 dir = new Vec3((0), (0), (0));
			Dictionary<string, int> devices = new Dictionary<string, int>();
			GestureComponent gestureComponent1 = new GestureComponent(Constants.RC, "up", 0, 0.0, dir, devices);
			GestureComponent gestureComponent2 = new GestureComponent(Constants.LC, "up", 0, 0.0, dir, devices);
			rcGesture.Add(gestureComponent1);
			lcGesture.Add(gestureComponent2);
			Gesture lowerGesture = new Gesture(hmdGesture, rcGesture, lcGesture);
			assertEquals("RAISE", recognition.recognize(lowerGesture)["gestureName"]);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void testGrowGesture()
		internal virtual void testGrowGesture()
		{
			IList<GestureComponent> hmdGesture = new List<GestureComponent>();
			IList<GestureComponent> rcGesture = new List<GestureComponent>();
			IList<GestureComponent> lcGesture = new List<GestureComponent>();
			Vec3 dir = new Vec3((0), (0), (0));
			Dictionary<string, int> devices = new Dictionary<string, int>();
			GestureComponent gestureComponent1 = new GestureComponent(Constants.RC, "right", 0, 0.0, dir, devices);
			GestureComponent gestureComponent2 = new GestureComponent(Constants.LC, "left", 0, 0.0, dir, devices);
			rcGesture.Add(gestureComponent1);
			lcGesture.Add(gestureComponent2);
			Gesture lowerGesture = new Gesture(hmdGesture, rcGesture, lcGesture);
			assertEquals("GROW", recognition.recognize(lowerGesture)["gestureName"]);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void testShrinkGesture()
		internal virtual void testShrinkGesture()
		{
			IList<GestureComponent> hmdGesture = new List<GestureComponent>();
			IList<GestureComponent> rcGesture = new List<GestureComponent>();
			IList<GestureComponent> lcGesture = new List<GestureComponent>();
			Vec3 dir = new Vec3((0), (0), (0));
			Dictionary<string, int> devices = new Dictionary<string, int>();
			GestureComponent gestureComponent1 = new GestureComponent(Constants.RC, "left", 0, 0.0, dir, devices);
			GestureComponent gestureComponent2 = new GestureComponent(Constants.LC, "right", 0, 0.0, dir, devices);
			rcGesture.Add(gestureComponent1);
			lcGesture.Add(gestureComponent2);
			Gesture lowerGesture = new Gesture(hmdGesture, rcGesture, lcGesture);
			assertEquals("SHRINK", recognition.recognize(lowerGesture)["gestureName"]);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void testBurstGesture()
		internal virtual void testBurstGesture()
		{

//====================================================================================================
//End of the allowed output for the Free Edition of Java to C# Converter.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================
