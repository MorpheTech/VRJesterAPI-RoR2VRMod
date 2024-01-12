using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;


namespace VRJester {
	public class Config {
		public string RECOGNIZE_ON = Constants.RECOGNIZE_ON;
		public string GESTURE_NAME = Constants.SAMPLE_GESTURE_NAME;
		public bool RECORD_MODE = Constants.RECORD_MODE;
		public bool READ_DATA = Constants.READ_DATA;
		public bool WRITE_DATA = Constants.WRITE_DATA;
		public bool DEMO_MODE = Constants.DEMO_MODE;
		public bool DEBUG_MODE = Constants.DEBUG_MODE;
		public float VIRTUAL_SPHERE_RADIUS = Constants.VIRTUAL_SPHERE_RADIUS;
		public int INTERVAL_DELAY = Constants.INTERVAL_DELAY;
		public int MAX_LISTENING_TIME = Constants.MAX_LISTENING_TIME;
		public Dictionary<string, Dictionary<string, string>> GESTURE_KEY_MAPPINGS = [];

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
                    ["KEY_MAPPING"] = "examplemod.key.ability_1",
                    ["KEY_ACTION"] = "click"
                };
				config.GESTURE_KEY_MAPPINGS["GESTURE 1"] = keyMappingContext;
				Dictionary<string, string> keyMappingContext2 = new() {
                    ["KEY_MAPPING"] = "key.keyboard.keypad.2",
                    ["KEY_ACTION"] = "hold"
                };
				config.GESTURE_KEY_MAPPINGS["GESTURE 2"] = keyMappingContext2;
				Dictionary<string, string> keyMappingContext3 = new() {
                    ["KEY_MAPPING"] = "key.keyboard.j",
                    ["KEY_ACTION"] = "click"
                };
				config.GESTURE_KEY_MAPPINGS["GESTURE 3"] = keyMappingContext3;
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
				Log.Error(e.ToString());
				Log.Error(e.StackTrace);
			}
		}

		public static void WriteGestureStore() {
		}
	}
}
