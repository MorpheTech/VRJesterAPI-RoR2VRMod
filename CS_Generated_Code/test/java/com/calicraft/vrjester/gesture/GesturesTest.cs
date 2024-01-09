using System.Collections.Generic;

namespace com.calicraft.vrjester.gesture
{
	using Config = com.calicraft.vrjester.config.Config;
	using Constants = com.calicraft.vrjester.config.Constants;
	using Recognition = com.calicraft.vrjester.gesture.recognition.Recognition;
	using Test = org.junit.jupiter.api.Test;

	using static org.junit.jupiter.api.Assertions.assertEquals;

	public class GesturesTest
	{

		private static readonly Config devConfig = Config.readConfig(Constants.DEV_CONFIG_PATH);
		private static readonly Gestures gestures = new Gestures(devConfig, Constants.DEV_GESTURE_STORE_PATH);

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test void gestureLoadTest()
		internal virtual void gestureLoadTest()
		{
			Dictionary<string, string> gestureNamespace = new Dictionary<string, string>();
			gestureNamespace["-409853157"] = "KAMEHAMEHA";
			gestureNamespace["-747232807-747232807"] = "BLOCK";
			gestureNamespace["21469640052146964005"] = "PULL";
			gestureNamespace["-295780073"] = "UPPERCUT";
			gestureNamespace["1679261955"] = "STRIKE";
			gestureNamespace["1744444741-1543099174"] = "SHRINK";
			gestureNamespace["16792619551679261955"] = "PUSH";
			gestureNamespace["17388439551738843955"] = "BURST";
			gestureNamespace["156397030156397030"] = "IDLE UP";
			gestureNamespace["-813293095-813293095"] = "RAISE";
			gestureNamespace["14531206081453120608"] = "LOWER";
			gestureNamespace["-15430991741744444741"] = "GROW";
			gestures.load();
			assertEquals(gestureNamespace, gestures.gestureNameSpace);
		}
	}

}
