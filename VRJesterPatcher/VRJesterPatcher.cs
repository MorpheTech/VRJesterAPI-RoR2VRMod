using BepInEx;
using BepInEx.Logging;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace VRJesterPatcher {
    //Class by MrPurple, adapted by DrBibop, modified by Caliburs

    /// <summary>
    /// A patcher which runs ahead of UnityPlayer to copy a dll before plugins are initialized
    /// </summary>
    public static class VRJesterPatcher {
        internal static string VRPatcherPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static string ManagedPath => Paths.ManagedPath;
        private static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("VRJesterPatcher");

        /// <summary>
        /// Called from BepInEx while patching, our entry point for patching.
        /// Do not change the method name as it is identified by BepInEx. Method must remain public.
        /// </summary>
        [Obsolete("Should not be used!", true)]
        public static void Initialize() {
            Logger.LogInfo("Checking for VRJester Libraries...");

            string[] managedLibraries = new string[] { "WindowsInput.dll" };

            bool copyManagedLibrariesResult = CopyFiles(ManagedPath, managedLibraries, "libs.");

            if (copyManagedLibrariesResult)
                Logger.LogInfo("Successfully copied VRJester plugins!");
            else
                Logger.LogInfo("VRJester plugins already present");
        }

        private static bool CopyFiles(string destinationPath, string[] fileNames, string embedFolder) {
            DirectoryInfo directoryInfo = new DirectoryInfo(destinationPath);
            FileInfo[] files = directoryInfo.GetFiles();
            bool flag = false;
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string name = executingAssembly.GetName().Name;
            string[] array = fileNames;
            for (int i = 0; i < array.Length; i++) {
                string fileName = array[i];
                if (!Array.Exists<FileInfo>(files, (FileInfo file) => fileName == file.Name)) {
                    flag = true;
                    using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name + "." + embedFolder + fileName)) {
                        using (FileStream fileStream = new FileStream(Path.Combine(directoryInfo.FullName, fileName), FileMode.Create, FileAccess.ReadWrite, FileShare.Delete)) {
                            Logger.LogInfo("Copying " + fileName);
                            manifestResourceStream.CopyTo(fileStream);
                        }
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// For BepInEx to identify your patcher as a patcher, it must match the patcher contract as outlined in the BepInEx docs:
        /// https://bepinex.github.io/bepinex_docs/v5.0/articles/dev_guide/preloader_patchers.html#patcher-contract
        /// It must contain a list of managed assemblies to patch as a public static <see cref="IEnumerable{T}"/> property named TargetDLLs
        /// </summary>
        [Obsolete("Should not be used!", true)]
        public static IEnumerable<string> TargetDLLs { get; } = new string[0];

        /// <summary>
        /// For BepInEx to identify your patcher as a patcher, it must match the patcher contract as outlined in the BepInEx docs:
        /// https://bepinex.github.io/bepinex_docs/v5.0/articles/dev_guide/preloader_patchers.html#patcher-contract
        /// It must contain a public static void method named Patch which receives an <see cref="AssemblyDefinition"/> argument,
        /// which patches each of the target assemblies in the TargetDLLs list.
        /// 
        /// We don't actually need to patch any of the managed assemblies, so we are providing an empty method here.
        /// </summary>
        /// <param name="ad"></param>
        [Obsolete("Should not be used!", true)]
        public static void Patch(AssemblyDefinition ad) { }
    }
}
