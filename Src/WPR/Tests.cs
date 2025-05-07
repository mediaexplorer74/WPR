using WPR;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System.Runtime;
using Microsoft.Xna.Framework.Graphics;
using WPR.MonoGameCompability;
using System.IO;
using System;
using System.Runtime.Loader;
using System.Reflection;

namespace WPR
{
    class WPR
    {
        /*static*/ void Tests()
        {
            // Initialize assembly resolver and load target assembly
            AppResolver resolver = new AppResolver();
            AssemblyDefinition newAsm = AssemblyDefinition.ReadAssembly("C:\\temp\\FNWP72.dll");
            
            // Load MonoGame framework assembly for reference manipulation
            Assembly assemMono = AssemblyLoadContext.Default.LoadFromAssemblyName(
                new AssemblyName("MonoGame.Framework"));

            // Prepare references for compatibility patches
            AssemblyNameReference reference = AssemblyNameReference.Parse("WPR.MonoGameCompability");
            AssemblyNameReference referenceRuntime = AssemblyNameReference.Parse("System.Runtime");
            DefaultAssemblyResolver resolver22 = new DefaultAssemblyResolver();
            AssemblyDefinition patchMono = resolver22.Resolve(reference);

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
            };


            // Modify assembly references to redirect XNA to MonoGame
            ModuleDefinition module = newAsm.Modules[0];
            foreach (AssemblyNameReference? refer in module.AssemblyReferences) 
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
            Mono.Collections.Generic.Collection<TypeDefinition> typess = patchMono.MainModule.Types;
            module.AssemblyReferences.Add(reference);
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
            MemoryStream stream = new MemoryStream();
            newAsm.Write(stream);
            stream.Position = 0;

            // Set working directory for content loading
            Directory.SetCurrentDirectory("C:\\temp\\");

            // Use reflection to set TitleContainer location for MonoGame content
            Type type = typeof(TitleContainer);
            PropertyInfo? prop = type.GetProperty("Location", BindingFlags.NonPublic | BindingFlags.Static);
            prop.GetSetMethod(true).Invoke(null, new object[] { "C:\\temp\\" });

            // Load and instantiate modified game assembly
            Assembly assem = AssemblyLoadContext.Default.LoadFromStream(stream);
            Type tt = assem.GetType("Mortar.TheGame");
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
                Console.Write("[ex] Bug: " + ex.ToString());
                Console.WriteLine(ex.StackTrace);
            }

            Console.Write("Ok!");
        }
    }
}