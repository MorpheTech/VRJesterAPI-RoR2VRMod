using System.Collections.Generic;


namespace VRJester {

	public abstract class Constants {
		public const string CONFIG_PATH = "Risk of Rain 2_Data/Config/VRJesterMod.cfg";
		public const string GESTURE_STORE_PATH = "Risk of Rain 2_Data/Config/gesture_store.json";
		public const string DEV_ROOT_PATH = "C:/Users/jakem/Documents/GitHub";
		public const string DEV_CONFIG_PATH = DEV_ROOT_PATH + "";
		public const string DEV_GESTURE_STORE_PATH = DEV_ROOT_PATH + "";
		public const string DEV_ARCHIVE_PATH = DEV_ROOT_PATH + "";

		// RECOGNIZE -> fire event right when recognized | RELEASE -> fire event when key is released
		public const string RECOGNIZE_ON = "RECOGNIZE";
		public const string SAMPLE_GESTURE_NAME = "GESTURE 1";
		public const bool RECORD_MODE = false;
		public const bool READ_DATA = false;
		public const bool WRITE_DATA = false;
		public const bool DEMO_MODE = true;
		public const bool DEBUG_MODE = false;
		public const float VOX_LENGTH = 0.6F;
		public const float VIRTUAL_SPHERE_RADIUS = 0.3F;
		public const int INTERVAL_DELAY = 15;
		public const int MAX_LISTENING_TIME = 400;

		public const float MOVEMENT_DEGREE_SPAN = 45.0F;
		public const float DIRECTION_DEGREE_SPAN = 30.0F;

		public const string HMD = "HEAD_MOUNTED_DISPLAY";
		public const string RC = "RIGHT_CONTROLLER";
		public const string LC = "LEFT_CONTROLLER";
		public const string C2 = "EXTRA_TRACKER";
		public static IList<string> DEVICES = [HMD, RC, LC];
	}
}
