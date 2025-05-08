using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime;

using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using WPR;
using WPR.MonoGameCompability;
using MCecil = Mono.Cecil;


namespace WPR
{
    static class FilePatcher
    {
        public static async Task<bool> PatchFile(string FilePath)
        {
            // Get the installation folder of the UWP app
            StorageFolder installFolder = Package.Current.InstalledLocation;

            // Get the app data folder (local app store / sandbox) of the UWP app
            StorageFolder AppDataFolder = ApplicationData.Current.LocalFolder;

            // Initialize assembly resolver and load target assembly
            AppResolver resolver = new AppResolver();
            MCecil.AssemblyDefinition newAsm = null;

            

            // check that source dll is in app data folder... 
            if (await AppDataFolder.TryGetItemAsync("FNWP72.dll") != null)
            {
                // true

                //AssemblyDefinition newAsm = AssemblyDefinition.ReadAssembly("C:\\temp\\FNWP72.dll");
                newAsm = MCecil.AssemblyDefinition.ReadAssembly(
                    AppDataFolder.Path + "\\FNWP72.dll");
            }
            else
            {
                // false 

                Debug.WriteLine("[warn] No FNWP72.dll in AppData Folder. File patcher process stopped!");
                return false;
            }

            // Load MonoGame framework assembly for reference manipulation
            //Assembly assemMono = AssemblyLoadContext.Default.LoadFromAssemblyName(
            //    new AssemblyName("MonoGame.Framework"));
            // Get the installation folder of the UWP app
           
            StorageFile monoGameFile = await installFolder.GetFileAsync("MonoGame.Framework.dll");

            // Load the assembly using Assembly.LoadFile
            Assembly assemMono = Assembly.LoadFile(monoGameFile.Path);


            // Prepare references for compatibility patches
            MCecil.AssemblyNameReference reference = MCecil.AssemblyNameReference.Parse("WPR.MonoGameCompability");
            MCecil.AssemblyNameReference referenceRuntime = MCecil.AssemblyNameReference.Parse("System.Runtime");
            MCecil.DefaultAssemblyResolver resolver22 = new MCecil.DefaultAssemblyResolver();
            MCecil.AssemblyDefinition patchMono = resolver22.Resolve(reference);

            if (newAsm == null)
            {
                return false; // Exit if target assembly couldn't be loaded
            }

            // Get type definition for patching from compatibility assembly
            MCecil.TypeDefinition typedef = patchMono.MainModule.GetType("WPF.MonoGameCompabilityPatch",
                "SpriteBatchPatch");

            // Scan type references in target assembly for XNA Graphics types
            foreach (MCecil.TypeReference? refer in newAsm.MainModule.GetTypeReferences())
            {
                if (refer.Module.Name == "Microsoft.Xna.Framework.Graphics")
                {
                    MCecil.MetadataToken t = refer.MetadataToken; // Capture metadata token for potential processing
                }
            }
            ;


            // Modify assembly references to redirect XNA to MonoGame
            MCecil.ModuleDefinition module = newAsm.Modules[0];
            foreach (MCecil.AssemblyNameReference? refer in module.AssemblyReferences)
            {
                if (refer.Name.Contains("Microsoft.Xna") && (!refer.Name.Contains("GamerServices")))
                {
                    // Replace XNA assembly reference with MonoGame equivalent
                    refer.Name = assemMono.GetName().Name;
                    refer.Version = assemMono.GetName().Version;
                    refer.PublicKey = assemMono.GetName().GetPublicKey();
                }
            }

            // Add compatibility framework references to target assembly
            Mono.Collections.Generic.Collection<MCecil.TypeDefinition> typess = patchMono.MainModule.Types;
            module.AssemblyReferences.Add(reference);
            module.AssemblyReferences.Add(referenceRuntime);

            // Update type references to point to compatibility layer
            MCecil.TypeReference typeRef = null;
            foreach (MCecil.TypeReference? existingRef in module.GetTypeReferences())
            {
                if (existingRef.Name == "SpriteBatch")
                {
                    // Redirect SpriteBatch to compatibility version
                    existingRef.Name = "SpriteBatch2";
                    existingRef.Namespace = "WPR.MonoGameCompability.Graphics";
                    existingRef.Scope = reference;
                }
                else if (existingRef.FullName == "System.Diagnostics.Stopwatch")
                {
                    // Ensure proper runtime reference for Stopwatch
                    existingRef.Scope = referenceRuntime;
                }
                else if (existingRef.Name == "GraphicsDeviceManager")
                {
                    // Redirect GraphicsDeviceManager to compatibility version
                    existingRef.Name = "GraphicsDeviceManager2";
                    existingRef.Namespace = "WPR.MonoGameCompability";
                    existingRef.Scope = reference;
                }
            }

            // Write modified assembly to memory stream
            //MemoryStream stream = new MemoryStream(); // write to stream
            //newAsm.Write(stream);
            //stream.Position = 0;
            newAsm.Write(AppDataFolder.Path + "\\FNWP72_new.dll"); // write to file

            // Set working directory for content loading
            //Directory.SetCurrentDirectory("C:\\temp\\");
            Directory.SetCurrentDirectory( AppDataFolder.Path );

            // Use reflection to set TitleContainer location for MonoGame content
            Type type = typeof(TitleContainer);
            PropertyInfo? prop = type.GetProperty("Location", BindingFlags.NonPublic | BindingFlags.Static);

            prop.GetSetMethod(true).Invoke
            (
                null,
                new object[]
               {
                   AppDataFolder.Path
               }
            );

            // ** Load and instantiate modified game assembly **

            // Create an assembly name
            //AssemblyName assemblyName = new AssemblyName();

            string assemblyPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "FNWP72_new.dll"); ///RnD

            // Load the assembly using Assembly.LoadFile
            Assembly assem = Assembly.LoadFile(assemblyPath);

            //Assembly assem = AssemblyLoadContext.Default.LoadFromStream(stream); // load from stream
            //Assembly assem = AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName); // load from file

            Type tt = assem.GetType("Mortar.TheGame");
            Microsoft.Xna.Framework.Game MonoGameObj = (Game)Activator.CreateInstance(tt);

            try
            {
                // Configure and run the game
                MonoGameObj.IsMouseVisible = true;
                MonoGameObj.Run();
            }
            catch (Exception ex)
            {
                // Handle any runtime errors
                Debug.WriteLine("[ex] Bug: " + ex.ToString());
                Debug.WriteLine(ex.StackTrace);
                return false;
            }

            Debug.WriteLine("FileTouch procedure: success !");
            return true; // Indicate success
        }
    }
}