using System;
using System.Collections.Generic;

namespace com.calicraft.vrjester
{
	using Config = com.calicraft.vrjester.config.Config;
	using Constants = com.calicraft.vrjester.config.Constants;
	using GestureEventHandler = com.calicraft.vrjester.handlers.GestureEventHandler;
	using TriggerEventHandler = com.calicraft.vrjester.handlers.TriggerEventHandler;
	using GestureCommand = com.calicraft.vrjester.utils.tools.GestureCommand;
	using InputConstants = com.mojang.blaze3d.platform.InputConstants;
	using KeyMappingRegistry = dev.architectury.registry.client.keymappings.KeyMappingRegistry;
	using KeyMapping = net.minecraft.client.KeyMapping;
	using Minecraft = net.minecraft.client.Minecraft;
	using LogManager = org.apache.logging.log4j.LogManager;
	using Logger = org.apache.logging.log4j.Logger;


	public class VrJesterApi
	{
		// Main entry point
		public static readonly Logger LOGGER = LogManager.getLogger();
		public static bool VIVECRAFT_LOADED = false;
		public const string MOD_ID = "vrjester";
		public static readonly KeyMapping MOD_KEY = new KeyMapping("key.vrjester.gesture_listener", InputConstants.Type.KEYSYM, 71, "key.vrjester.category");
		public static Dictionary<string, KeyMapping> KEY_MAPPINGS = new Dictionary<string, KeyMapping>();

		public static void init()
		{
			LOGGER.info("Initializing VR Jester API");
			Console.WriteLine("Config Path: " + ModExpectPlatform.ConfigDirectory.toAbsolutePath().normalize().ToString());
			setupConfig();
			setupEvents();
	//        setupClient();
		}

		public static Minecraft MCI
		{
			get
			{
				return Minecraft.getInstance();
			}
		}

		private static void setupConfig()
		{
			LOGGER.info("Setting up commands...");
			GestureCommand.init();
			LOGGER.info("Setting up keybindings...");
			KeyMappingRegistry.register(MOD_KEY);
			LOGGER.info("Setting up config files...");
			File configFile = new File(Constants.CONFIG_PATH);
			File gestureStoreFile = new File(Constants.GESTURE_STORE_PATH);
			if (!configFile.exists())
			{
				Config.writeConfig();
			}
			if (!gestureStoreFile.exists())
			{
				Config.writeGestureStore();
			}
		}

		public static void setupClient()
		{
			Config config = Config.readConfig();
			KeyMapping[] keyMappings = MCI.options.keyMappings;
			Dictionary<string, Dictionary<string, string>> gestureMappings = config.GESTURE_KEY_MAPPINGS;
			foreach (Dictionary<string, string> gestureMapping in gestureMappings.Values)
			{
				IList<string> gestureKeyMappings = new List<string> {gestureMapping["KEY_MAPPING"].Split(",", true)};
				foreach (string gestureKeyMapping in gestureKeyMappings)
				{
					foreach (KeyMapping keyMapping in keyMappings)
					{
						if (keyMapping.getName().Equals(gestureKeyMapping))
						{
							LOGGER.info("Adding gesture key mapping -> mapping name: " + gestureKeyMapping + " | key name: " + keyMapping.saveString());
							KEY_MAPPINGS[gestureKeyMapping] = keyMapping;
						}
					}
				}
			}
		}

		private static void setupEvents()
		{
			LOGGER.info("Setting up events...");
			GestureEventHandler.init();
			TriggerEventHandler.init();
		}
	}
}
