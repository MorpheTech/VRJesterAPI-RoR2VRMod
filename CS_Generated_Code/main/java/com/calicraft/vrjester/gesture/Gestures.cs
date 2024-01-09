//====================================================================================================
//The Free Edition of Java to C# Converter limits conversion output to 100 lines per file.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com.calicraft.vrjester.gesture
{
	using Config = com.calicraft.vrjester.config.Config;
	using Constants = com.calicraft.vrjester.config.Constants;
	using Node = com.calicraft.vrjester.gesture.radix.Node;
	using Path = com.calicraft.vrjester.gesture.radix.Path;
	using RadixTree = com.calicraft.vrjester.gesture.radix.RadixTree;
	using GestureComponentDeserializer = com.calicraft.vrjester.utils.tools.GestureComponentDeserializer;
	using GestureComponentSerializer = com.calicraft.vrjester.utils.tools.GestureComponentSerializer;
	using com.google.gson;
	using TypeToken = com.google.gson.reflect.TypeToken;
	using JsonReader = com.google.gson.stream.JsonReader;
	using JsonToken = com.google.gson.stream.JsonToken;
	using JsonWriter = com.google.gson.stream.JsonWriter;


	public class Gestures
	{
		// Class for storing the gestures in a namespace for each VRDevice
		// The gesture store, stores gestures for reading and writing to/from JSON
		// The namespace determines what's recognized as a complete gesture
		// The mappings determine which devices recognize which gestures
		// The radix trees store the actual gestures

		private readonly File gestureStoreFile;
		public readonly GestureStore gestureStore = new GestureStore();
		public Dictionary<string, string> gestureNameSpace = new Dictionary<string, string>();
		public Dictionary<int, string> hmdGestureMapping = new Dictionary<int, string>();
		public Dictionary<int, string> rcGestureMapping = new Dictionary<int, string>();
		public Dictionary<int, string> lcGestureMapping = new Dictionary<int, string>();
		public RadixTree hmdGestures = new RadixTree(Constants.HMD);
		public RadixTree rcGestures = new RadixTree(Constants.RC);
		public RadixTree lcGestures = new RadixTree(Constants.LC);
		public Dictionary<string, IList<string>> eitherDeviceGestures = new Dictionary<string, IList<string>>();

		public Config config;

		public Gestures(Config config, string gesture_store_path)
		{
			this.config = config;
			gestureStoreFile = new File(gesture_store_path);
		}

		// Read in gestures from gesture store file and return GestureStore object
		public virtual GestureStore read()
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				Scanner myReader = new Scanner(gestureStoreFile);
				while (myReader.hasNextLine())
				{
					string data = myReader.nextLine();
					sb.Append(data);
				}
				myReader.close();
				GsonBuilder builder = new GsonBuilder();
				builder.registerTypeAdapter(typeof(GestureComponent), new GestureComponentDeserializer());
				builder.setPrettyPrinting();
				Gson gson = builder.create();
				return gson.fromJson(sb.ToString(), typeof(GestureStore));
			}
			catch (Exception e) when (e is FileNotFoundException || e is JsonSyntaxException)
			{
				Console.WriteLine("An error occurred reading gesture store json!");
				e.printStackTrace();
			}
			return null;
		}

		// Load up all gestures from gesture store into the radix trees & namespaces
		public virtual void load()
		{
			GestureStore gestureStore = read();
			clear();
			if (gestureStore != null)
			{
				ISet<string> gestureNames = gestureStore.GESTURES.Keys;
				foreach (string gestureName in gestureNames)
				{ // Iterate through & store each gesture
					try
					{
						Gesture gesture = new Gesture(gestureStore.GESTURES[gestureName]);
						store(gesture, gestureName);
					}
					catch (System.NullReferenceException e)
					{
						Console.Error.WriteLine(e);
						Console.WriteLine("SKIPPING LOADING GESTURE: " + gestureName);
					}
				}
			}
	//        FOR DEBUGGING:
	//        System.out.println("GESTURE NAMESPACE: " + gestureNameSpace);
	//        System.out.println("LOADED GESTURES:");
	//        hmdGestures.printAllGestures(hmdGestureMapping);
	//        rcGestures.printAllGestures(rcGestureMapping);
	//        lcGestures.printAllGestures(lcGestureMapping);
	//        System.out.println("HMD TREE:");
	//        hmdGestures.printAllPaths();
	//        System.out.println("RC TREE:");
	//        rcGestures.printAllPaths();
	//        System.out.println("LC TREE:");
	//        lcGestures.printAllPaths();
		}

		// Store a new gesture encompassing all VRDevices
		public virtual void store(Gesture gesture, string name)
		{
			string id;
			StringBuilder sb = new StringBuilder();
			foreach (string vrDevice in Constants.DEVICES)
			{
				if (gesture.validDevices.Count > 0)
				{ // Gestures for either OR, VRDevice storage instance handler
					eitherDeviceGestures[name] = gesture.validDevices;
					string newId = storeToMapping(gesture, name, vrDevice);
					if (!sb.ToString().Contains(newId))
					{
						sb.Append(newId);
					}
				}
				else
				{
					sb.Append(storeToMapping(gesture, name, vrDevice));
				}
			}
			id = sb.ToString();
			gestureNameSpace[id] = name;
		}

		// Store a new gesture into a specified VRDevice namespace
		public virtual void store(RadixTree gestureTree, Dictionary<int, string> gestureMapping, IList<GestureComponent> gesture, string name)
		{
			gestureTree.insert(gesture);
			gestureMapping[gesture.GetHashCode()] = name;
			string id = "" + gesture.GetHashCode();
			gestureNameSpace[id] = name;
		}

		public virtual string storeToMapping(Gesture gesture, string name, string vrDevice)
		{
			string id = "";
			RadixTree gestureTree = getRadixTree(vrDevice);
			IList<GestureComponent> deviceGesture = gesture.getGesture(vrDevice);
			Dictionary<int, string> gestureMapping = getGestureMapping(vrDevice);
			if (deviceGesture.Count > 0)
			{
				gestureTree.insert(deviceGesture);
				if (!gestureMapping.ContainsKey(deviceGesture.GetHashCode()))
				{
					gestureMapping[deviceGesture.GetHashCode()] = name;
				}
				id += deviceGesture.GetHashCode();
			}
			return id;
		}

		// Write all stored gestures to gesture store file
		public virtual void write()
		{
			writeGestures(Constants.HMD, hmdGestures.root, new List<GestureComponent>());
			writeGestures(Constants.RC, rcGestures.root, new List<GestureComponent>());
			writeGestures(Constants.LC, lcGestures.root, new List<GestureComponent>());
			try
			{
					using (StreamWriter writer = new StreamWriter(gestureStoreFile.getPath()))
					{
					GsonBuilder builder = new GsonBuilder();
					builder.registerTypeAdapter(typeof(GestureComponent), new GestureComponentSerializer());

//====================================================================================================
//End of the allowed output for the Free Edition of Java to C# Converter.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================
