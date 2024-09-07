using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using VRJester.Core;
using VRJester.Utils.VRData;


namespace VRJester {

    public class Config {
        public string GESTURE_NAME = Constants.SAMPLE_GESTURE_NAME;
        public bool RECORD_MODE = Constants.RECORD_MODE;
        public bool READ_DATA = Constants.READ_DATA;
        public bool WRITE_DATA = Constants.WRITE_DATA;
        public bool DEBUG_MODE = Constants.DEBUG_MODE;
        public float VHERE_RADIUS = Constants.VHERE_RADIUS;
        public int INTERVAL_DELAY = Constants.INTERVAL_DELAY;
        public int MAX_LISTENING_TIME = Constants.MAX_LISTENING_TIME;
        public Dictionary<string, Dictionary<string, string>> GESTURE_ACTIONS = [];

        public Config() {}

        public static Config ReadConfig() {
            try {
                StringBuilder sb = new();
                StreamReader myReader = new(Constants.CONFIG_PATH);
                string data = myReader.ReadLine();
                sb.Append(data);
                while (data != null) {
                    data = myReader.ReadLine();
                    sb.Append(data);
                }
                myReader.Close();
                Console.ReadLine();
                return JsonConvert.DeserializeObject<Config>(sb.ToString());
            }
            catch (FileNotFoundException) {
                Log.Error("An error occurred reading config json! Attempting to generate new config...");
                WriteConfig();
            }
            catch (JsonException) {
                Log.Error("An error occurred reading config json!  Check if VRJesterAPI.cfg is malformed.");
            }
            return new Config();
        }

        public static Config ReadConfig(string configPath) {
            try {
                StringBuilder sb = new();
                StreamReader myReader = new(configPath);
                string data = myReader.ReadLine();
                sb.Append(data);
                while (data != null) {
                    data = myReader.ReadLine();
                    sb.Append(data);
                }
                myReader.Close();
                Console.ReadLine();
                
                return JsonConvert.DeserializeObject<Config>(sb.ToString());
            }
            catch (FileNotFoundException) {
                Log.Error("An error occurred reading config json! Attempting to generate new config...");
                WriteConfig();
            }
            catch (JsonException) {
                Log.Error("An error occurred reading config json!  Check if VRJesterAPI.cfg is malformed.");
            }
            return ReadConfig(); // Use default Minecraft config path
        }

        public static void WriteConfig() {
            try {
                Config config = new();
                Dictionary<string, string> keyMappingContext = new() {
                    ["KEY_BIND"] = "R",
                    ["KEY_ACTION"] = "click"
                };
                config.GESTURE_ACTIONS["STRIKE"] = keyMappingContext;
                Dictionary<string, string> keyMappingContext2 = new() {
                    ["KEY_BIND"] = "M2",
                    ["KEY_ACTION"] = "hold"
                };
                config.GESTURE_ACTIONS["UPPERCUT"] = keyMappingContext2;
                Dictionary<string, string> keyMappingContext3 = new() {
                    ["KEY_BIND"] = "LSHIFT",
                    ["KEY_ACTION"] = "click"
                };
                config.GESTURE_ACTIONS["BURST"] = keyMappingContext3;
                using StreamWriter sw = new(@Constants.CONFIG_PATH);
                using JsonTextWriter jw = new(sw);
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new();
                serializer.Serialize(jw, config);
            }
            catch (IOException e) {
                Log.Error("An error occurred writing config json!");
                Log.Error(e.ToString());
                Log.Error(e.StackTrace);
            }
        }

        public static void WriteConfig(Config config) {
            try {
                using StreamWriter sw = new(@Constants.CONFIG_PATH);
                using JsonTextWriter jw = new(sw);
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new();
                serializer.Serialize(jw, config);
            }
            catch (IOException e) {
                Log.Error("An error occurred writing config json!");
                Log.Error(e.StackTrace);
            }
        }

        public static void WriteGestureStore() {
            List<GestureComponent> hmdGesture = [];
            List<GestureComponent> rcGesture = [];
            List<GestureComponent> rcGesture2 = []; // To reproduce null error, use same rcGesture object
            List<GestureComponent> rcGesture3 = [];
            List<GestureComponent> lcGesture = [];
            List<GestureComponent> lcGesture2 = [];
            List<GestureComponent> lcGesture3 = [];
            IDictionary<string, int> devices = new Dictionary<string, int>();
            string dir = "*";

            GestureComponent gestureComponentR1 = new(Constants.RC, "forward", 0, 0.0, dir, devices);
            GestureComponent gestureComponentR2 = new(Constants.RC, "up", 0, 0.0, dir, devices);
            GestureComponent gestureComponentR3 = new(Constants.RC, "forward", 2000, 0.0, dir, devices);
            GestureComponent gestureComponentL1 = new(Constants.LC, "forward", 0, 0.0, dir, devices);
            GestureComponent gestureComponentL2 = new(Constants.LC, "up", 0, 0.0, dir, devices);
            GestureComponent gestureComponentL3 = new(Constants.LC, "forward", 2000, 0.0, dir, devices);

            rcGesture.Add(gestureComponentR1);
            lcGesture.Add(gestureComponentL1);
            Gesture strikeGesture = new(hmdGesture, rcGesture, lcGesture);
            strikeGesture.validDevices.Add(Constants.RC);
            strikeGesture.validDevices.Add(Constants.LC);
            GestureHandler.gestures.Store(strikeGesture, "STRIKE");
            rcGesture2.Add(gestureComponentR1);
            rcGesture2.Add(gestureComponentR2);
            lcGesture2.Add(gestureComponentL1);
            lcGesture2.Add(gestureComponentL2);
            Gesture uppercutGesture = new(hmdGesture, rcGesture2, lcGesture2);
            uppercutGesture.validDevices.Add(Constants.RC);
            uppercutGesture.validDevices.Add(Constants.LC);
            GestureHandler.gestures.Store(uppercutGesture, "UPPERCUT");
            rcGesture3.Add(gestureComponentR3);
            lcGesture3.Add(gestureComponentL3);
            Gesture burstGesture = new(hmdGesture, rcGesture3, lcGesture3);
            GestureHandler.gestures.Store(burstGesture, "BURST");
            GestureHandler.gestures.Write();

            // -- Gesture Recognition Tests --
            GestureFactory gestureFactory = new(Constants.RC, "forward", 0, 0.0, dir, devices);
            GestureComponent gestureComponent = gestureFactory.CreateGestureComponent("", "back");
            // hmdGesture.Clear(); lcGesture.Clear();
            // Dictionary<string, string> recognizedGesture = GestureHandler.recognition.Recognize(strikeGesture);
            // recognizedGesture.TryGetValue("gestureName", out string gestureName);
            // Log.Info("RECOGNIZED strikeGesture -> " + gestureName);

            // List<GestureComponent> rcGesture4 = [];
            // GestureComponent gestureComponent = new(Constants.RC, "forward", 43, 2.1, "forward", devices);
            // rcGesture4.Add(gestureComponent);
            // Gesture strikeGesture2 = new(hmdGesture, rcGesture4, lcGesture);

            // recognizedGesture = GestureHandler.recognition.Recognize(strikeGesture2);
            // recognizedGesture.TryGetValue("gestureName", out string gestureName2);
            // Log.Info("RECOGNIZED strikeGesture2 -> " + gestureName2);
        }
    }
}
