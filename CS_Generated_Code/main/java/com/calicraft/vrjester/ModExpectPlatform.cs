namespace com.calicraft.vrjester
{
	using ExpectPlatform = dev.architectury.injectables.annotations.ExpectPlatform;
	using Platform = dev.architectury.platform.Platform;

	public class ModExpectPlatform
	{
		/// <summary>
		/// We can use <seealso cref="Platform.getConfigFolder()"/> but this is just an example of <seealso cref="ExpectPlatform"/>.
		/// <para>
		/// This must be a <b>public static</b> method. The platform-implemented solution must be placed under a
		/// platform sub-package, with its class suffixed with {@code Impl}.
		/// </para>
		/// <para>
		/// Example:
		/// Expect: net.examplemod.ExampleExpectPlatform#getConfigDirectory()
		/// Actual Fabric: net.examplemod.fabric.ExampleExpectPlatformImpl#getConfigDirectory()
		/// Actual Forge: net.examplemod.forge.ExampleExpectPlatformImpl#getConfigDirectory()
		/// </para>
		/// <para>
		/// <a href="https://plugins.jetbrains.com/plugin/16210-architectury">You should also get the IntelliJ plugin to help with @ExpectPlatform.</a>
		/// </para>
		/// </summary>
//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ExpectPlatform public static java.nio.file.Path getConfigDirectory()
		public static Path ConfigDirectory
		{
			get
			{
				// Just throw an error, the content should get replaced at runtime.
				throw new AssertionError();
			}
		}
	}

}
