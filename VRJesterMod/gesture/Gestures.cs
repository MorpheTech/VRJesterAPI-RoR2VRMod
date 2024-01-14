using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using VRJester.Core.Radix;


namespace VRJester.Core {

    public class Gestures(Config config, string gestureStorePath) {
		// Class for storing the gestures in a namespace for each VRDevice
		// The gesture store, stores gestures for reading and writing to/from JSON
		// The namespace determines what's recognized as a complete gesture
		// The mappings determine which devices recognize which gestures
		// The radix trees store the actual gestures

		private readonly string gestureStorePath = gestureStorePath;
		public readonly GestureStore gestureStore = new();
		public Dictionary<string, string> gestureNameSpace = [];
		public Dictionary<int, string> hmdGestureMapping = [];
		public Dictionary<int, string> rcGestureMapping = [];
		public Dictionary<int, string> lcGestureMapping = [];
		public RadixTree hmdGestures = new(Constants.HMD);
		public RadixTree rcGestures = new(Constants.RC);
		public RadixTree lcGestures = new(Constants.LC);
		public Dictionary<string, IList<string>> eitherDeviceGestures = [];

		public Config config = config;

        // Read in gestures from gesture store file and return GestureStore object
        public GestureStore Read() {
			try {
				StringBuilder sb = new();
				StreamReader myReader = new(gestureStorePath);
				string data = myReader.ReadLine();
				sb.Append(data);
				while (data != null) {
					data = myReader.ReadLine();
					sb.Append(data);
				}
				myReader.Close();
				Console.ReadLine();
				JsonConvert.DeserializeObject<GestureStore>(sb.ToString());
			}
			catch (Exception e) when (e is FileNotFoundException || e is JsonException) {
				Log.Error("An error occurred reading gesture store json!");
				Log.Error(e);
			}
			return null;
		}

		// Load up all gestures from gesture store into the radix trees & namespaces
		public void Load() {
			GestureStore gestureStore = Read();
			Clear();
			if (gestureStore != null) {
				HashSet<string> gestureNames = new(gestureStore.GESTURES.Keys);
				foreach (string gestureName in gestureNames) { // Iterate through & store each gesture
					try {
						Gesture gesture = new(gestureStore.GESTURES[gestureName]);
						Store(gesture, gestureName);
					}
					catch (NullReferenceException e) {
						Log.Error(e);
						Log.Error("SKIPPING LOADING GESTURE: " + gestureName);
					}
				}
			}
		}

		// Store a new gesture encompassing all VRDevices
		public void Store(Gesture gesture, string name) {
			string id;
			StringBuilder sb = new();
			foreach (string vrDevice in Constants.DEVICES) {
				if (gesture.validDevices.Count > 0) { // Gestures for either OR, VRDevice storage instance handler
					eitherDeviceGestures[name] = gesture.validDevices;
					string newId = StoreToMapping(gesture, name, vrDevice);
					if (!sb.ToString().Contains(newId)) {
						sb.Append(newId);
					}
				}
				else {
					sb.Append(StoreToMapping(gesture, name, vrDevice));
				}
			}
			id = sb.ToString();
			gestureNameSpace[id] = name;
		}

		// Store a new gesture into a specified VRDevice namespace
		public void Store(RadixTree gestureTree, Dictionary<int, string> gestureMapping, List<GestureComponent> gesture, string name) {
			gestureTree.Insert(gesture);
			gestureMapping[gesture.HashCode()] = name;
			string id = "" + gesture.HashCode();
			gestureNameSpace[id] = name;
		}

		public string StoreToMapping(Gesture gesture, string name, string vrDevice) {
			string id = "";
			RadixTree gestureTree = GetRadixTree(vrDevice);
			List<GestureComponent> deviceGesture = gesture.GetGesture(vrDevice);
			Dictionary<int, string> gestureMapping = GetGestureMapping(vrDevice);
			if (deviceGesture.Count > 0) {
				gestureTree.Insert(deviceGesture);
				if (!gestureMapping.ContainsKey(deviceGesture.HashCode())) {
					gestureMapping.Add(deviceGesture.HashCode(), name);
				}
				id += deviceGesture.HashCode();
			}
			return id;
		}

		// Write all stored gestures to gesture store file
		public void Write() {
			WriteGestures(Constants.HMD, hmdGestures.root, []);
			WriteGestures(Constants.RC, rcGestures.root, []);
			WriteGestures(Constants.LC, lcGestures.root, []);
			try {
				using StreamWriter sw = new(@gestureStorePath);
                using JsonTextWriter jw = new(sw);
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new();
                serializer.Serialize(jw, gestureStore);
			} catch (IOException e) {
				Log.Error("An error occurred writing config json!");
				Log.Error(e.StackTrace);
			}
		}

		// Add each gesture to GestureStore object for writing to gesture store file
		private void WriteGestures(string vrDevice, Node current, List<GestureComponent> result) {
			if (current.isGesture) {
				GetGestureMapping(vrDevice).TryGetValue(result.HashCode(), out string gestureName);
				eitherDeviceGestures.TryGetValue(gestureName, out IList<string> eitherDeviceGesture);
				gestureStore.AddGesture(vrDevice, gestureName, result, eitherDeviceGesture);
			}
			foreach (Branch path in current.paths.Values) {
				WriteGestures(vrDevice, path.next, GestureComponent.Concat(result, path.gesture));
			}
		}

		// Reset the gestures namespace
		public void Clear() {
			gestureNameSpace = [];
			hmdGestures = new RadixTree(Constants.HMD);
			rcGestures = new RadixTree(Constants.RC);
			lcGestures = new RadixTree(Constants.LC);
			hmdGestureMapping = [];
			rcGestureMapping = [];
			lcGestureMapping = [];
			eitherDeviceGestures = [];
		}

		// Return gesture mapping based on VRDevice
		private Dictionary<int, string> GetGestureMapping(string vrDevice) {
			Dictionary<int, string> gestureMapping = [];
			switch (vrDevice) {
				case Constants.HMD:
					gestureMapping = hmdGestureMapping;
					break;
				case Constants.RC:
					gestureMapping = rcGestureMapping;
					break;
				case Constants.LC:
					gestureMapping = lcGestureMapping;
					break;
			}
			return gestureMapping;
		}

		private RadixTree GetRadixTree(string vrDevice) {
			RadixTree gestureTree = null;
			switch (vrDevice) {
				case Constants.HMD:
					gestureTree = hmdGestures;
					break;
				case Constants.RC:
					gestureTree = rcGestures;
					break;
				case Constants.LC:
					gestureTree = lcGestures;
					break;
			}
			return gestureTree;
		}
	}
}