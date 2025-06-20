/*using System;
using Avalonia;

namespace WPR.UI.Desktop;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}*/
using System.Reflection;
using WPR;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System.Runtime.Loader;
using System.Runtime;

using WPR.MonoGameCompability;
using System.Diagnostics;
using System.IO;
using System;

namespace WPR
{
    class WPR
    {
        static void Main()
        {
            // Initialize assembly resolver and load target assembly
            AppResolver resolver = new AppResolver();
            AssemblyDefinition newAsm = AssemblyDefinition.ReadAssembly("C:\\temp\\FNWP72.dll.original"); // ("C:\\temp\\FNWP72.dll");

            // Load MonoGame framework assembly for referenceMGCompatibility manipulation
            Assembly assemMono = AssemblyLoadContext.Default.LoadFromAssemblyName(
                new AssemblyName("MonoGame.Framework"));

            // Prepare references for compatibility patches

            AssemblyNameReference referenceMGCompatibility = AssemblyNameReference.Parse("WPR.MonoGameCompability");
            AssemblyNameReference referenceRuntime = AssemblyNameReference.Parse("System.Runtime");
            //AssemblyNameReference referenceGamerServices = AssemblyNameReference.Parse("Microsoft.Xna.Framework.GamerServices");

            DefaultAssemblyResolver resolver22 = new DefaultAssemblyResolver();
            AssemblyDefinition patchMono = resolver22.Resolve(referenceMGCompatibility);

            if (newAsm == null)
            {
                return; // Exit if target assembly couldn't be loaded
            }

            // Get type definition for patching from compatibility assembly
            TypeDefinition typedef = patchMono.MainModule.GetType("WPF.MonoGameCompabilityPatch",
                "SpriteBatchPatch");

            // Scan type references in target assembly for XNA Graphics types
            foreach (TypeReference? refer in newAsm.MainModule.GetTypeReferences())
            {
                if (refer.Module.Name == "Microsoft.Xna.Framework.Graphics")
                {
                    MetadataToken t = refer.MetadataToken; // Capture metadata token for potential processing
                }
            }
            ;


            // Modify assembly references to redirect XNA to MonoGame 
            ModuleDefinition module = newAsm.Modules[0];
            foreach (AssemblyNameReference? refer in module.AssemblyReferences)
            {
                if (refer.Name.Contains("Microsoft.Xna") && (!refer.Name.Contains("GamerServices")))
                {
                    // Replace XNA assembly referenceMGCompatibility with MonoGame equivalent
                    refer.Name = assemMono.GetName().Name;
                    refer.Version = assemMono.GetName().Version;
                    refer.PublicKey = assemMono.GetName().GetPublicKey();
                }
            }

            // Add compatibility framework references to target assembly
            Mono.Collections.Generic.Collection<TypeDefinition> typess = patchMono.MainModule.Types;
            module.AssemblyReferences.Add(referenceMGCompatibility);
            module.AssemblyReferences.Add(referenceRuntime);

            // Update type references to point to compatibility layer
            TypeReference typeRef = null;
            foreach (TypeReference? existingRef in module.GetTypeReferences())
            {
                if (existingRef.Name == "SpriteBatch")
                {
                    // Redirect SpriteBatch to compatibility version
                    existingRef.Name = "SpriteBatch2";
                    existingRef.Namespace = "WPR.MonoGameCompability.Graphics";
                    existingRef.Scope = referenceMGCompatibility;
                }
                else if (existingRef.FullName == "System.Diagnostics.Stopwatch")
                {
                    // Ensure proper runtime referenceMGCompatibility for Stopwatch
                    existingRef.Scope = referenceRuntime;
                }
                else if (existingRef.Name == "GraphicsDeviceManager")
                {
                    // Redirect GraphicsDeviceManager to compatibility version
                    existingRef.Name = "GraphicsDeviceManager2";
                    existingRef.Namespace = "WPR.MonoGameCompability";
                    existingRef.Scope = referenceMGCompatibility;
                }
                else if (existingRef.Name == "GamerServicesComponent")
                {
                    // Redirect GraphicsDeviceManager to compatibility version
                    existingRef.Name = "GamerServicesComponent";
                    existingRef.Namespace = "WPR.MonoGameCompability.GamerServices";//"Microsoft.Xna.Framework.GamerServices";
                    existingRef.Scope = referenceMGCompatibility;// referenceGamerServices;
                }
                else if (existingRef.Name == "SignedInGamerCollection")
                {
                    // Redirect GraphicsDeviceManager to compatibility version
                    existingRef.Name = "SignedInGamerCollection";
                    existingRef.Namespace = "WPR.MonoGameCompability.GamerServices";//"Microsoft.Xna.Framework.GamerServices";
                    existingRef.Scope = referenceMGCompatibility;// referenceGamerServices;
                }
                else if (existingRef.Name == "Gamer")
                {
                    // Redirect GraphicsDeviceManager to compatibility version
                    existingRef.Name = "SignedInGamerCollection";
                    existingRef.Namespace = "WPR.MonoGameCompability.GamerServices";//"Microsoft.Xna.Framework.GamerServices";
                    existingRef.Scope = referenceMGCompatibility;// referenceGamerServices;
                }
                else if (existingRef.Name == "SignedInGamer")
                {
                    // Redirect GraphicsDeviceManager to compatibility version
                    existingRef.Name = "SignedInGamerCollection";
                    existingRef.Namespace = "WPR.MonoGameCompability.GamerServices";//"Microsoft.Xna.Framework.GamerServices";
                    existingRef.Scope = referenceMGCompatibility;// referenceGamerServices;
                }
                else if (existingRef.Name == "SignedInEventArgs")
                {
                    // Redirect GraphicsDeviceManager to compatibility version
                    existingRef.Name = "SignedInGamerCollection";
                    existingRef.Namespace = "WPR.MonoGameCompability.GamerServices";//"Microsoft.Xna.Framework.GamerServices";
                    existingRef.Scope = referenceMGCompatibility;// referenceGamerServices;
                }
            }

            // Write modified assembly to memory stream
            MemoryStream stream = new MemoryStream();
            newAsm.Write(stream);

            //DEBUG
            newAsm.Write("C:\\Temp\\FNWP72.dll");

            stream.Position = 0;

            // Set working directory for content loading
            Directory.SetCurrentDirectory("C:\\Temp\\");

            // Use reflection to set TitleContainer location for MonoGame content
            Type type = typeof(TitleContainer);
            PropertyInfo? prop = type.GetProperty("Location", BindingFlags.NonPublic | BindingFlags.Static);
            prop.GetSetMethod(true).Invoke(null, new object[] { "C:\\Temp\\" });

            // Load and instantiate modified game assembly
            //Assembly assem = AssemblyLoadContext.Default.LoadFromStream(stream);
            Assembly assem = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath: "C:\\Temp\\FNWP72.dll");

            Type tt = assem.GetType("Mortar.TheGame");
            //Type tt = assem.GetType("DoodleJump");

            Game obj = (Game)Activator.CreateInstance(tt);

            try
            {
                // Configure and run the game
                obj.IsMouseVisible = true;
                obj.Run();
            }
            catch (Exception ex)
            {
                // Handle any runtime errors
                Debug.WriteLine("[ex] Process error: " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
                return;
            }

            Debug.WriteLine("[!] Process ok!");
        }
    }
}
