#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2022 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region CASE_SENSITIVITY_HACK Option
// #define CASE_SENSITIVITY_HACK
/* On Linux, the file system is case sensitive.
 * This means that unless you really focused on it, there's a good chance that
 * your filenames are not actually accurate! The result: File/DirectoryNotFound.
 * This is a quick alternative to MONO_IOMAP=all, but the point is that you
 * should NOT depend on either of these two things. PLEASE fix your paths!
 * -flibit
 */
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
#endregion

namespace Microsoft.Xna.Framework
{
	public static class TitleContainer
	{
		#region Public Static Methods

		public static Stream OpenStream(string name)
		{
			string safeName = MonoGame.Utilities.FileHelpers.NormalizeFilePathSeparators(name);

#if CASE_SENSITIVITY_HACK
			if (Path.IsPathRooted(safeName))
			{
				safeName = GetCaseName(safeName);
			}
			safeName = GetCaseName(Path.Combine(TitleLocation.Path, safeName));
#endif
			string realName = Path.IsPathRooted(safeName)
				? safeName
				: Path.Combine(TitleLocation.Path, safeName);
			string resolvedName = ResolvePackagedPath(realName, TitleLocation.Path);
			return File.OpenRead(resolvedName);
		}

		#endregion

		#region Internal Static Methods

		internal static IntPtr ReadToPointer(string name, out IntPtr size)
		{
			string safeName = MonoGame.Utilities.FileHelpers.NormalizeFilePathSeparators(name);

#if CASE_SENSITIVITY_HACK
			if (Path.IsPathRooted(safeName))
			{
				safeName = GetCaseName(safeName);
			}
			safeName = GetCaseName(Path.Combine(TitleLocation.Path, safeName));
#endif
			string realName;
			if (Path.IsPathRooted(safeName))
			{
				realName = safeName;
			}
			else
			{
				realName = Path.Combine(TitleLocation.Path, safeName);
			}
			realName = ResolvePackagedPath(realName, TitleLocation.Path);
			if (!File.Exists(realName))
			{
				throw new FileNotFoundException(realName);
			}
			return FNAPlatform.ReadFileToPointer(realName, out size);
		}

		#endregion

		private static string ResolvePackagedPath(string path, string titleRoot)
		{
			if (File.Exists(path))
			{
				return path;
			}

			string fullPath = Path.GetFullPath(path);
			string root = Path.GetFullPath(titleRoot);
			string rootPrefix = root.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal)
				? root
				: root + Path.DirectorySeparatorChar;
			if (!fullPath.StartsWith(rootPrefix,
				OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
			{
				return path;
			}

			string relative = fullPath.Substring(rootPrefix.Length);
			string[] segments = relative.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			for (int index = 0; index < segments.Length - 1; index += 1)
			{
				string segment = segments[index];
				bool cultureSegment = segment.Length == 2 &&
					char.IsLetter(segment[0]) && char.IsLetter(segment[1]);
				if (!cultureSegment && segment.Length == 5 && segment[2] == '-')
				{
					cultureSegment = char.IsLetter(segment[0]) && char.IsLetter(segment[1]) &&
						char.IsLetter(segment[3]) && char.IsLetter(segment[4]);
				}
				if (!cultureSegment)
				{
					continue;
				}

				string candidate = Path.Combine(root,
					Path.Combine(segments.Where((_, candidateIndex) => candidateIndex != index).ToArray()));
				if (File.Exists(candidate))
				{
					return candidate;
				}
			}

			return path;
		}

		#region Private Static fcaseopen Method

#if CASE_SENSITIVITY_HACK
		private static string GetCaseName(string name)
		{
			if (File.Exists(name))
			{
				return name;
			}

			string[] splits = name.Split(Path.DirectorySeparatorChar);
			splits[0] = "/";
			int i;

			// The directories...
			for (i = 1; i < splits.Length - 1; i += 1)
			{
				splits[0] += SearchCase(
					splits[i],
					Directory.GetDirectories(splits[0])
				);
			}

			// The file...
			splits[0] += SearchCase(
				splits[i],
				Directory.GetFiles(splits[0])
			);

			// Finally.
			splits[0] = splits[0].Remove(0, 1);
			FNALoggerEXT.LogError(
				"Case sensitivity!\n\t" +
				name.Substring(TitleLocation.Path.Length) + "\n\t" +
				splits[0].Substring(TitleLocation.Path.Length)
			);
			return splits[0];
		}

		private static string SearchCase(string name, string[] list)
		{
			foreach (string l in list)
			{
				string li = l.Substring(l.LastIndexOf("/") + 1);
				if (name.ToLower().Equals(li.ToLower()))
				{
					return Path.DirectorySeparatorChar + li;
				}
			}
			// If you got here, get ready to crash!
			return Path.DirectorySeparatorChar + name;
		}
#endif

		#endregion
	}
}

