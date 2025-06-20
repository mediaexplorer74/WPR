// Import necessary namespaces for assembly manipulation and loading
using System.Reflection;
using System.Runtime.Loader;
using Mono.Cecil;

namespace WPR
{
    // Internal utility class for assembly-related operations
    internal class AssemblyUtils
    {
        // Method to clone an assembly with a new name and reload it into the context
        public static Assembly? SaveExistingAssemblyAsAndReload(AssemblyLoadContext context, string currentName, AssemblyNameDefinition newName)
        {
            // Parse the original assembly name to create a reference
            AssemblyNameReference reference = AssemblyNameReference.Parse(currentName);

            // Create a default assembly resolver for dependency resolution
            DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();

            // Attempt to resolve the original assembly using the resolver
            AssemblyDefinition newAsm = resolver.Resolve(reference);

            // Return null if assembly resolution failed
            if (newAsm == null)
            {
                return null;
            }

            // Get the main module of the resolved assembly
            ModuleDefinition mainModule = newAsm.MainModule;

            // Rename the main module with the new assembly name (appending .dll extension)
            mainModule.Name = newName.Name + ".dll";

            // Update the assembly's identity with the new name
            newAsm.Name = newName;

            // Write the modified assembly to disk with the new filename
            newAsm.Write(newName.Name + ".dll");

            // Load the newly created assembly from hardcoded output path
            // NOTE: This path should be parameterized for production use
            return context.LoadFromAssemblyPath(
                "C:\\Users\\Admin\\source\\repos\\!WPR\\WPR0\\WPR\\bin\\x64\\Debug\\net6.0-windows\\" +
                            $"{newName.Name}.dll");
        }
    }
}