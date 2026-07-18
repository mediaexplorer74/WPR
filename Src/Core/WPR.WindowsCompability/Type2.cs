using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace WPR.WindowsCompability
{

    public abstract class Type2
    {
        // The patcher rewrites every game call to Type.GetType(String, Boolean) into a call
        // to this method. On the phone CLR an unqualified name resolved against the *calling*
        // assembly plus corelib, so a game could ask for one of its own types by simple name.
        // Here the calling assembly is WPR.WindowsCompability, so that lookup can never see a
        // game type and silently returns null. Probe the loaded game assemblies as well,
        // preferring the caller, so the phone's resolution scope is restored.
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Type? GetType(string typeName, bool throwOnError)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException("Type name is null!");
            }

            var stuffs = typeName.Split(',');
            bool assemblyQualified = stuffs.Length >= 2;
            if (assemblyQualified)
            {
                bool patched = false;
                for (int i = 1; i < stuffs.Length; i += 4)
                {
                    if (stuffs[i].Contains("Microsoft.Xna.Framework"))
                    {
                        if (!stuffs[i].Equals("Microsoft.Xna.Framework.GamerServices"))
                        {
                            stuffs[i] = "FNA";
                            patched = true;
                        }
                    }
                }
                if (patched)
                {
                    typeName = stuffs[0];
                    for (int i = 1; i < stuffs.Length; i += 4)
                    {
                        typeName += $", {stuffs[i]}";
                    }
                }
            }

            Type? resolved = Type.GetType(typeName);
            if (resolved != null)
            {
                return resolved;
            }

            // An assembly-qualified name already names its assembly; only a simple name
            // depends on the caller's scope, so that is the only case worth probing.
            if (!assemblyQualified)
            {
                foreach (Assembly candidate in GetProbeAssemblies())
                {
                    resolved = candidate.GetType(typeName, throwOnError: false);
                    if (resolved != null)
                    {
                        return resolved;
                    }
                }
            }

            if (throwOnError)
            {
                throw new TypeLoadException($"Could not load type '{typeName}'.");
            }

            return null;
        }

        private static IEnumerable<Assembly> GetProbeAssemblies()
        {
            var seen = new HashSet<Assembly>();

            // The immediate caller is the game assembly whose call site was patched, which is
            // the scope the phone CLR would have searched first.
            Assembly? caller = GetCallerAssembly();
            if (caller != null && seen.Add(caller))
            {
                yield return caller;
            }

            // Games spread transpiled/engine types across sibling assemblies, so fall back to
            // everything the launcher has loaded for this title.
            foreach (Assembly loaded in AssemblyLoadContext.Default.Assemblies)
            {
                if (seen.Add(loaded))
                {
                    yield return loaded;
                }
            }
        }

        private static Assembly? GetCallerAssembly()
        {
            try
            {
                // Frame 0 is GetCallerAssembly, 1 is GetProbeAssemblies' iterator, so walk out
                // to the first frame that is not part of this facade.
                var stack = new StackTrace(fNeedFileInfo: false);
                for (int i = 0; i < stack.FrameCount; i++)
                {
                    Type? declaring = stack.GetFrame(i)?.GetMethod()?.DeclaringType;
                    if (declaring == null || declaring == typeof(Type2) ||
                        declaring.DeclaringType == typeof(Type2))
                    {
                        continue;
                    }

                    return declaring.Assembly;
                }
            }
            catch
            {
                // Stack walking is best effort; the loaded-assembly sweep still applies.
            }

            return null;
        }
    }
}
