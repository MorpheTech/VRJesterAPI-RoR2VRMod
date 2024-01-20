using System.Collections.Generic;
using BepInEx;


namespace VRJester {

    public abstract class Constants {
        public const string CONFIG_FILE = "VRJesterMod.cfg";
        public static readonly string CONFIG_PATH = System.IO.Path.Combine(Paths.ConfigPath, CONFIG_FILE);
        public const string GESTURE_STORE_FILE = "VRGestureStore.json";
        public static readonly string GESTURE_STORE_PATH = System.IO.Path.Combine(Paths.ConfigPath, GESTURE_STORE_FILE);
        public const string DEV_ROOT_PATH = "";
        public const string DEV_CONFIG_PATH = DEV_ROOT_PATH + "";
        public const string DEV_GESTURE_STORE_PATH = DEV_ROOT_PATH + "";
        public const string DEV_ARCHIVE_PATH = DEV_ROOT_PATH + "";

        // RECOGNIZE -> fire event right when recognized | RELEASE -> fire event when key is released
        public const string SAMPLE_GESTURE_NAME = "GESTURE 1";
        public const bool RECORD_MODE = false;
        public const bool READ_DATA = false;
        public const bool WRITE_DATA = false;
        public const bool DEBUG_MODE = false;
        public const float VOX_LENGTH = 0.6F;
        public const float VHERE_RADIUS = 0.3F;
        public const int INTERVAL_DELAY = 20;
        public const int MAX_LISTENING_TIME = 400;

        public const double VERTICAL_DEGREE_SPAN = 0.85D;
        public const float CARDINAL_DEGREE_SPAN = 45.0F;
        public const float DIRECTION_DEGREE_SPAN = 30.0F;

        public const string HMD = "HEAD_MOUNTED_DISPLAY";
        public const string RC = "RIGHT_CONTROLLER";
        public const string LC = "LEFT_CONTROLLER";
        public const string C2 = "EXTRA_TRACKER";
        public static IList<string> DEVICES = [HMD, RC, LC];
    }
}
