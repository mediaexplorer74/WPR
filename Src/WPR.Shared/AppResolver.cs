// Import necessary namespaces for assembly manipulation and loading
using System.Reflection;
using System.Runtime.Loader;
using Mono.Cecil;

namespace WPR
{
    /// <summary>
    /// Custom AssemblyLoadContext to handle runtime resolution of XNA Framework assemblies
    /// by redirecting them to MonoGame equivalents.
    /// </summary>
    internal class AppResolver : AssemblyLoadContext, IDisposable
    {
        // Constructor subscribes to the Resolving event to handle missing assembly loads
        public AppResolver()
        {
            this.Resolving += this.ResolveMissingDependencies;
        }

        // Cleanup: Unsubscribe from the Resolving event to prevent memory leaks
        public void Dispose()
        {
            this.Resolving -= this.ResolveMissingDependencies;
        }

        /// <summary>
        /// Loads a MonoGame assembly as a substitute for the requested XNA assembly.
        /// </summary>
        /// <param name="context">The AssemblyLoadContext requesting the resolution</param>
        /// <param name="requiredName">Identity of the requested assembly</param>
        /// <returns>Loaded MonoGame assembly or null</returns>
        private Assembly LoadXnaAssembly(AssemblyLoadContext context, AssemblyName requiredName)
        {
            // Create a new assembly name definition with matching name and version
            AssemblyNameDefinition def = new AssemblyNameDefinition(requiredName.Name, requiredName.Version);

            // Copy public key token to maintain strong naming compatibility
            def.PublicKeyToken = requiredName.GetPublicKeyToken();

            // Use utility method to locate/save/reload MonoGame assembly as XNA substitute
            return AssemblyUtils.SaveExistingAssemblyAsAndReload(context, "MonoGame.Framework", def);
        }

        /// <summary>
        /// Event handler for resolving missing XNA Framework dependencies
        /// </summary>
        /// <returns>Substitute assembly if XNA is requested, otherwise null</returns>
        private Assembly ResolveMissingDependencies(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
        {
            string assemblyNameInString = assemblyName.Name ?? "";

            // Check if requested assembly is part of XNA Framework we need to redirect
            if (assemblyNameInString.Equals("Microsoft.Xna.Framework", StringComparison.OrdinalIgnoreCase) ||
                assemblyNameInString.Equals("Microsoft.Xna.Framework.Game", StringComparison.OrdinalIgnoreCase) ||
                assemblyNameInString.Equals("Microsoft.Xna.Framework.Graphics", StringComparison.OrdinalIgnoreCase))
            {
                // Load and return the corresponding MonoGame assembly
                Assembly asm = LoadXnaAssembly(assemblyLoadContext, assemblyName);
                return asm;
            }

            // Let other resolution mechanisms handle non-XNA assemblies
            return null;
        }
    }
}