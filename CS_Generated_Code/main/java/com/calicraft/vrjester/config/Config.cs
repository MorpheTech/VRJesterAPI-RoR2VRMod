//====================================================================================================
//The Free Edition of Java to C# Converter limits conversion output to 100 lines per file.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com.calicraft.vrjester.config
{
	using Gesture = com.calicraft.vrjester.gesture.Gesture;
	using com.calicraft.vrjester.gesture;
	using TriggerEventHandler = com.calicraft.vrjester.handlers.TriggerEventHandler;
	using Vec3 = com.calicraft.vrjester.utils.tools.Vec3;
	using Gson = com.google.gson.Gson;
	using GsonBuilder = com.google.gson.GsonBuilder;
	using JsonSyntaxException = com.google.gson.JsonSyntaxException;
	using MalformedJsonException = com.google.gson.stream.MalformedJsonException;


	public class Config
	{
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
		public Dictionary<string, Dictionary<string, string>> GESTURE_KEY_MAPPINGS = new Dictionary<string, Dictionary<string, string>>();
		public Dictionary<string, ParticleContext> TESTING_GESTURES = new Dictionary<string, ParticleContext>();
	//    public Log LOG = new Log();

		public class Log
		{
			private readonly Config outerInstance;

			public Log(Config outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			// Class that represents log configuration
			public string name;
			public string gesture;
			public int pose;
			public string[] devices = new string[]{};
		}

		public class ParticleContext
		{
			private readonly Config outerInstance;

			// Class that represents a gesture event configuration
			public double velocity;
			public int rcParticle;
			public int lcParticle;

			public ParticleContext(Config outerInstance)
			{
				this.outerInstance = outerInstance;
				this.velocity = 1.0;
				this.rcParticle = -1;
				this.lcParticle = -1;
			}

			public ParticleContext(Config outerInstance, double velocity, int rcParticle, int lcParticle)
			{
				this.outerInstance = outerInstance;
				this.velocity = velocity;
				this.rcParticle = rcParticle;
				this.lcParticle = lcParticle;
			}
		}

		public static Config readConfig()
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				File configFile = new File(Constants.CONFIG_PATH);
				Scanner myReader = new Scanner(configFile);
				while (myReader.hasNextLine())
				{
					string data = myReader.nextLine();
					sb.Append(data);
	//                System.out.println("CONFIG: " + data);
				}
				myReader.close();
				GsonBuilder builder = new GsonBuilder();
				builder.setPrettyPrinting();
				Gson gson = builder.create();
				return gson.fromJson(sb.ToString(), typeof(Config));
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("An error occurred reading config json! Attempting to generate new config...");
				writeConfig();
			}
			catch (JsonSyntaxException)
			{
				Console.WriteLine("An error occurred reading config json!  Check if VRJesterAPI.cfg is malformed.");
			}
			return new Config();
		}

		public static Config readConfig(string configPath)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				File configFile = new File(configPath);
				Scanner myReader = new Scanner(configFile);
				while (myReader.hasNextLine())
				{
					string data = myReader.nextLine();
					sb.Append(data);
	//                System.out.println("CONFIG: " + data);
				}
				myReader.close();
				GsonBuilder builder = new GsonBuilder();
				builder.setPrettyPrinting();
				Gson gson = builder.create();
				return gson.fromJson(sb.ToString(), typeof(Config));
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("An error occurred reading config json! Attempting to generate new config...");
				writeConfig();
			}
			catch (JsonSyntaxException)
			{
				Console.WriteLine("An error occurred reading config json! Check if VRJesterAPI.cfg is malformed.");
			}
			return readConfig(); // Use default Minecraft config path
		}

		public static void writeConfig()
		{
			try
			{
				Config config = new Config();
				File configFile = new File(Constants.CONFIG_PATH);
				ParticleContext strikeContext = new com.calicraft.vrjester.config.Config.ParticleContext(config, 1.0, 0, 0);
				ParticleContext burstContext = new com.calicraft.vrjester.config.Config.ParticleContext(config, 1.0, 3, 3);
				ParticleContext uppercutContext = new com.calicraft.vrjester.config.Config.ParticleContext(config, 0.25, 3, 3);
				Dictionary<string, string> keyMappingContext = new Dictionary<string, string>();
				keyMappingContext["KEY_MAPPING"] = "examplemod.key.ability_1";
				keyMappingContext["KEY_ACTION"] = "click";
				config.GESTURE_KEY_MAPPINGS["GESTURE 1"] = keyMappingContext;
				Dictionary<string, string> keyMappingContext2 = new Dictionary<string, string>();
				keyMappingContext2["KEY_MAPPING"] = "key.keyboard.keypad.2";

//====================================================================================================
//End of the allowed output for the Free Edition of Java to C# Converter.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================
