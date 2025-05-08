using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Xml.Serialization;
using Mono.Cecil.Rocks;

using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace WPR
{
    public class ApplicationPatcher
    {
        public static int Version => 0;

        private AssemblyNameReference FNACompRef;
        private AssemblyNameReference FNARef;
        private AssemblyNameReference SystemRunTimeRef;
        
        private AssemblyNameReference WindowsCompRef;
        
        private AssemblyNameReference StandardCompRef;
        private AssemblyNameReference ServiceModelPrimitivesRef;
        private AssemblyNameReference ServiceModelHTTPRef;
        //private AssemblyNameReference SystemSecurityCryptographyRef; //!
        //private AssemblyNameReference SystemWindowsMediaImagingRef; //!

        private class TypePatchInfo
        {
            public String? NewName;
            public String? NewNamespace;
            public AssemblyNameReference? Reference;
        }

        private Dictionary<string, TypePatchInfo> Patches;
        private Dictionary<string, Type> MemberPatches;

        public ApplicationPatcher()
        {
            FNARef = AssemblyNameReference.Parse("FNA");
            FNACompRef = AssemblyNameReference.Parse("WPR.XnaCompability");
            SystemRunTimeRef = AssemblyNameReference.Parse("System.Runtime");
            WindowsCompRef = AssemblyNameReference.Parse("WPR.WindowsCompability");
            
            ServiceModelPrimitivesRef = AssemblyNameReference.Parse("System.ServiceModel.Primitives");
            ServiceModelHTTPRef = AssemblyNameReference.Parse("System.ServiceModel.Http");
            
            StandardCompRef = AssemblyNameReference.Parse("WPR.StandardCompability");


            //SystemSecurityCryptographyRef = AssemblyNameReference.Parse(
            //    "WPR.WindowsCompability");//!

            //SystemWindowsMediaImagingRef =  AssemblyNameReference.Parse(
            //    "WPR.WindowsCompability");//!

            Patches = new Dictionary<string, TypePatchInfo>()
            {
                { "System.Diagnostics.Stopwatch", new TypePatchInfo()
                {
                    Reference = SystemRunTimeRef
                }
                },
                { "Microsoft.Xna.Framework.GraphicsDeviceManager", new TypePatchInfo()
                {
                    NewName = "GraphicsDeviceManager2",
                    NewNamespace = "WPR.XnaCompability",
                    Reference = FNACompRef
                }
                },
                { "System.Windows.Application", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "System.Windows.ApplicationUnhandledExceptionEventArgs", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "System.IO.IsolatedStorage.IsolatedStorageSettings", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewName="IsolatedStorageSettings2", //RnD
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "Microsoft.Xna.Framework.Media.MediaSource", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.MediaSourceType", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.SongCollection", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.Artist", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.ArtistCollection", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.Album", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.AlbumCollection", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.Genre", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.MediaLibrary", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "System.Windows.Media.SolidColorBrush", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability.Media"
                }
                },
                { "System.Windows.Media.Color", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability.Media"
                }
                },
                { "System.Windows.Thickness", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability.Media"
                }
                },
                { "System.Windows.ResourceDictionary", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "System.ServiceModel.XmlSerializerFormatAttribute", new TypePatchInfo()
                {
                    Reference = ServiceModelPrimitivesRef
                }
                },
                { "System.ServiceModel.BasicHttpBinding", new TypePatchInfo()
                {
                    Reference = ServiceModelHTTPRef
                }
                },
                { "System.ServiceModel.BasicHttpSecurity", new TypePatchInfo()
                {
                    Reference = ServiceModelHTTPRef
                }
                },
                { "System.ServiceModel.BasicHttpSecurityMode", new TypePatchInfo()
                {
                    Reference = ServiceModelHTTPRef
                }
                },
                //!
                { "System.Security.Cryptography.ProtectedData", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    //RnD : if uncomment it, WPR.WindowsCompabilityProtectedData class will be used
                    NewName = "ProtectedData", 
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                //!
                { "System.Windows.Media.Imaging.BitmapImage", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewName = "BitmapImage",//RnD
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                //!
                { "System.Windows.Media.Imaging.WriteableBitmap", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                 //!
                { "System.Windows.Media.Imaging.BitmapSource", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "System.Windows.MessageBox", new TypePatchInfo()
                {
                    Reference = WindowsCompRef
                }
                },
                { "System.Windows.MessageBoxResult", new TypePatchInfo()
                {
                    Reference = WindowsCompRef
                }
                },
                { "System.Windows.MessageBoxButton", new TypePatchInfo()
                {
                    Reference = WindowsCompRef
                }
                }
            };

            // *** Member Patches ***
            MemberPatches = new Dictionary<string, Type>
            {
                //TODO
                //{
                //    "System.Byte[] System.......::MethodName(System.Byte[],System.Byte[])",
                //    typeof(WPR.WindowsCompability.WebServices)
                //},

                //!                
                {
                    "System.Boolean System.IO.IsolatedStorage.IsolatedStorageSettings::TryGetValue(System.String, ByRef)",
                    typeof(WPR.WindowsCompability.IsolatedStorageSettings2)
                },
                {
                    "System.IO.IsolatedStorage.IsolatedStorageSettings System.IO.IsolatedStorage.IsolatedStorageSettings::get_ApplicationSettings()",
                    typeof(WPR.WindowsCompability.IsolatedStorageSettings2)
                },

                {
                    "System.Byte[] System.Security.Cryptography.ProtectedData::Protect(System.Byte[],System.Byte[])",
                    typeof(WPR.WindowsCompability.ProtectedData)
                },
                 //!
                {
                    "System.Byte[] System.Security.Cryptography.ProtectedData::Unprotect(System.Byte[],System.Byte[])",
                    typeof(WPR.WindowsCompability.ProtectedData)
                },
                 //!
                //{
                //    "System.Windows.Media.Imaging.WriteableBitmap System.Windows.Media.Imaging.WriteableBitmap(System.Integer,System.Integer)",
                //    typeof(WPR.WindowsCompability.WriteableBitmap)
                //},
                //{
                //    "System.Void System.Windows.Media.Imaging.BitmapSource::SetSource()",
                //    typeof(WPR.WindowsCompability.BitmapSource)
                //},

                {
                    "System.Type System.Type::GetType(System.String,System.Boolean)",
                    typeof(WPR.WindowsCompability.Type2)
                },
                {
                    "Microsoft.Xna.Framework.Graphics.DisplayMode Microsoft.Xna.Framework.Graphics.GraphicsDevice::get_DisplayMode()",
                    typeof(WPR.XnaCompability.Graphics.GraphicsDevice2)
                },
                {
                    "Microsoft.Xna.Framework.Graphics.DisplayMode Microsoft.Xna.Framework.Graphics.GraphicsAdapter::get_CurrentDisplayMode()",
                    typeof(WPR.XnaCompability.Graphics.GraphicsAdapter2)
                },
                
                {
                    "System.String System.IO.Path::GetDirectoryName(System.String)",
                    typeof(WPR.WindowsCompability.Path2)
                },
                {
                    "System.String System.IO.Path::GetFileName(System.String)",
                    typeof(WPR.WindowsCompability.Path2)
                },
                {
                    "System.String System.IO.Path::GetFileNameWithoutExtension(System.String)",
                    typeof(WPR.WindowsCompability.Path2)
                },
                {
                    "System.Void System.GC::Collect()",
                    typeof(WPR.WindowsCompability.GC2)
                },

                {
                    "System.Xml.Linq.XElement System.Xml.Linq.XElement::Load(System.String)",
                    typeof(WPR.StandardCompability.Xml.Linq.XElement2)
                },
              
            };

        }//ApplicationPatcher

        private void PatchRelaxedXmlNullableAttribTextSerialize(ModuleDefinition? module)
        {
            Queue<TypeDefinition> typeScanQueue = new Queue<TypeDefinition>();
            foreach (var typeDef in module!.Types)
            {
                typeScanQueue.Enqueue(typeDef);
            }

            CustomAttribute? xmlIgnoreAttrib = null;

            // Patch type for resolve XML library incompability
            while (typeScanQueue.Count != 0)
            {
                TypeDefinition type = typeScanQueue.Dequeue();

                if (type.HasNestedTypes)
                {
                    foreach (var typeNested in type.NestedTypes)
                    {
                        typeScanQueue.Enqueue(typeNested);
                    }
                }

                foreach (var field in type.Fields)
                {
                    CustomAttribute? xmlNonNullableProp = null;

                    foreach (var attrib in field.CustomAttributes)
                    {
                        if (attrib.AttributeType.FullName == typeof(XmlAttributeAttribute).FullName)
                        {
                            xmlNonNullableProp = attrib;
                            break;
                        }
                    }

                    if (xmlNonNullableProp == null)
                    {
                        continue;
                    }

                    if (field.FieldType.FullName.Contains("System.Nullable"))
                    {
                        var actualFieldType = (field.FieldType as GenericInstanceType)!.GenericArguments[0];

                        // Generate holder getter/setter
                        var getterMethod = new MethodDefinition($"get_{field.Name}SerializableHolder", 
                            MethodAttributes.Public, actualFieldType);

                        var getterGen = getterMethod.Body.GetILProcessor();

                        var nullableRefTypeGeneric = module.ImportReference(
                            Type.GetType("System.Nullable`1")!);

                        var nullableRefType = 
                            nullableRefTypeGeneric.MakeGenericInstanceType(new TypeReference[]
                            { actualFieldType });

                        // Emit getter
                        getterGen.Emit(OpCodes.Ldarg_0);
                        getterGen.Emit(OpCodes.Ldflda, field);
                        getterGen.Emit(OpCodes.Call, new MethodReference("get_Value", 
                            nullableRefTypeGeneric.GenericParameters[0])
                        {
                            HasThis = true,
                            DeclaringType = nullableRefType
                        });

                        getterGen.Emit(OpCodes.Ret);

                        // Emit setter
                        var setterMethod = new MethodDefinition($"set_{field.Name}SerializableHolder", 
                            MethodAttributes.Public, module.TypeSystem.Void)
                        {
                            Parameters = { new ParameterDefinition(actualFieldType) },
                            HasThis = true
                        };
                        var setterGen = setterMethod.Body.GetILProcessor();

                        setterGen.Emit(OpCodes.Ldarg_0);
                        setterGen.Emit(OpCodes.Ldarg_1);
                        setterGen.Emit(OpCodes.Newobj, new MethodReference(".ctor", 
                            module.TypeSystem.Void, nullableRefType)
                        {
                            Parameters = { new ParameterDefinition(
                                nullableRefTypeGeneric.GenericParameters[0]) },
                            HasThis = true
                        });

                        setterGen.Emit(OpCodes.Stfld, field);
                        setterGen.Emit(OpCodes.Ret);

                        // Emit skip serialize consideration
                        var shouldSerializeMethod = new MethodDefinition(
                            $"ShouldSerialize{field.Name}SerializableHolder", 
                            MethodAttributes.Public, module.TypeSystem.Boolean);

                        var shouldSerializeGen = shouldSerializeMethod.Body.GetILProcessor();

                        shouldSerializeGen.Emit(OpCodes.Ldarg_0);
                        shouldSerializeGen.Emit(OpCodes.Ldflda, field);
                        shouldSerializeGen.Emit(OpCodes.Call, new MethodReference(
                            "HasValue", module.TypeSystem.Boolean, nullableRefType)
                        {
                            HasThis = true
                        });
                        shouldSerializeGen.Emit(OpCodes.Ret);

                        type.Methods.Add(shouldSerializeMethod);
                        type.Methods.Add(getterMethod);
                        type.Methods.Add(setterMethod);

                        var propSeri = new PropertyDefinition(
                            $"{field.Name}SerializableHolder", PropertyAttributes.None, actualFieldType)
                        {
                            GetMethod = getterMethod,
                            SetMethod = setterMethod
                        };

                        type.Properties.Add(propSeri);

                        if (xmlIgnoreAttrib == null)
                        {
                            xmlIgnoreAttrib = new CustomAttribute(module.ImportReference(typeof(XmlIgnoreAttribute).
                                GetConstructor(Type.EmptyTypes)));
                        }

                        field.CustomAttributes.Remove(xmlNonNullableProp);
                        field.CustomAttributes.Add(xmlIgnoreAttrib);

                        // Add attribute if they already gave name, else we need to be creative
                        if (xmlNonNullableProp.HasConstructorArguments)
                        {
                            propSeri.CustomAttributes.Add(xmlNonNullableProp);
                        }
                        else
                        {
                            var attributeType = (xmlNonNullableProp.AttributeType.FullName 
                                == typeof(XmlAttributeAttribute).FullName)
                                    ? typeof(XmlAttributeAttribute) 
                                    : typeof(XmlTextAttribute);

                            MethodReference methodConstructor = module.ImportReference(attributeType
                                .GetConstructor(new Type[] { typeof(String) }));
                            
                            propSeri.CustomAttributes.Add(new CustomAttribute(methodConstructor)
                            {
                                ConstructorArguments = { 
                                    new CustomAttributeArgument(module.TypeSystem.String, field.Name) }
                            });
                        }
                    }
                }
            }
        }

        // PatchDll(string modulePath)
        public void PatchDll(string modulePath)
        {
            // ReadAssembly
            AssemblyDefinition assemblyData = 
                Mono.Cecil.AssemblyDefinition.ReadAssembly(modulePath);

            Mono.Cecil.ModuleDefinition module = assemblyData.MainModule;

            assemblyData.Name.Name = AssemblyNameStandardization.Process(assemblyData.Name.Name);

            string modulePathNameStandardized = Path.Combine(
                Path.GetDirectoryName(modulePath)!,
               AssemblyNameStandardization.Process(
                    Path.GetFileNameWithoutExtension(modulePath)) +
                Path.GetExtension(modulePath));

            AssemblyNameReference? xnaGameServices = null;
            //RnD
            AssemblyNameReference? xnaGameServicesExtensions = null;

            // Remove unneeded attribute (pretty sure!)
            foreach (var attrib in module.Assembly.CustomAttributes)
            {
                if (attrib.AttributeType.FullName == 
                    "System.Runtime.CompilerServices.CodeGenerationAttribute")
                {
                    module.Assembly.CustomAttributes.Remove(attrib);
                    break;
                }
            }

            // module.AssemblyReferences cycle 
            foreach (var refer in module.AssemblyReferences)
            {
                if (refer.Name.Contains("Microsoft.Xna"))
                {
                    if (refer.Name.Contains("GamerServices"))
                    {
                        xnaGameServices = refer;
                    }
                    else if (refer.Name.Contains("GamerServicesExtensions"))
                    {
                        //RnD
                        xnaGameServicesExtensions = refer;
                    }
                    else
                    {
                        refer.Name = FNARef.Name;
                        refer.Version = FNARef.Version;
                        refer.PublicKey = FNARef.PublicKey;
                    }
                } 
                else if (refer.Name.Equals("mscorlib.Extensions", 
                    StringComparison.OrdinalIgnoreCase))
                {
                    refer.Name = SystemRunTimeRef.Name;
                    refer.Version = SystemRunTimeRef.Version;
                    refer.PublicKey = SystemRunTimeRef.PublicKey;
                }
                else if (refer.Name.Equals("System.ServiceModel", 
                    StringComparison.OrdinalIgnoreCase))
                {
                    refer.Name = ServiceModelPrimitivesRef.Name;
                    refer.Version = ServiceModelPrimitivesRef.Version;
                    refer.PublicKey = ServiceModelPrimitivesRef.PublicKey;
                }
            }

            //RnD
            PatchRelaxedXmlNullableAttribTextSerialize(module);

            // Add AssemblyReferences
            module.AssemblyReferences.Add(FNACompRef);
            module.AssemblyReferences.Add(WindowsCompRef);
            module.AssemblyReferences.Add(SystemRunTimeRef);
            module.AssemblyReferences.Add(ServiceModelPrimitivesRef);
            module.AssemblyReferences.Add(ServiceModelHTTPRef);
            module.AssemblyReferences.Add(StandardCompRef);
            //module.AssemblyReferences.Add(SystemSecurityCryptographyRef);//!
            //module.AssemblyReferences.Add(SystemWindowsMediaImagingRef);//

            // create Ref. Patch Cache
            Dictionary<string, TypeReference> typeRefPatchCache 
                = new Dictionary<string, TypeReference>();

            // module.GetMemberReferences cycle
            foreach (var memberRef in module.GetMemberReferences())
            {
                //if (memberRef.FullName.Contains("Collect"))
                //{
                //    Debug.WriteLine("[Collect] memberRef fullname: "
                //        + memberRef.FullName);
                //}

                foreach (var patch in MemberPatches)
                {
                    /*
                    if (memberRef.FullName.Contains("Collect"))
                    {
                        //Debug.WriteLine("[TeSTING] memberRef.FullName.Contains : Collect");
                        Debug.WriteLine("[TeSTING] memberRef.FullName Contains Collect: " 
                            + memberRef.FullName);
                    }
                    */

                    if (memberRef.FullName == patch.Key)
                    {
                        if (typeRefPatchCache.ContainsKey(patch.Value.FullName!))
                        {
                            memberRef.DeclaringType = typeRefPatchCache[patch.Value.FullName!];
                        } 
                        else
                        {
                            memberRef.DeclaringType = module.ImportReference(patch.Value);
                            typeRefPatchCache.Add(patch.Value.FullName!, memberRef.DeclaringType);
                        }
                    }
                }
            }

            // cycle existing refs...
            foreach (var existingRef in module.GetTypeReferences())
            {
                existingRef.Name = AssemblyNameStandardization.Process(existingRef.Name);

                if (existingRef.FullName 
                    == "Microsoft.Xna.Framework.GamerServices.GamerServicesComponent")
                {
                    existingRef.Scope = xnaGameServices;
                }
                else if(existingRef.FullName
                    == "Microsoft.Xna.Framework.GamerServicesExtensions.GamerServicesComponent")
                {
                    //RnD

                    existingRef.Scope = xnaGameServicesExtensions;
                }
                else
                {
                    if (Patches.ContainsKey(existingRef.FullName))
                    {
                        TypePatchInfo patch = Patches[existingRef.FullName];
                        if (patch != null)
                        {
                            if (patch.NewName != null)
                            {
                                existingRef.Name = patch.NewName;
                            }

                            if (patch.NewNamespace != null)
                            {
                                existingRef.Namespace = patch.NewNamespace;
                            }

                            if (patch.Reference != null)
                            {
                                existingRef.Scope = patch.Reference;
                            }
                        }
                    }
                }
            }//for...
            

            // create .dll.new
            try
            {
                assemblyData.Write(modulePath + ".new");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] assemblyData.Write : " + ex.Message);
                Debug.WriteLine("[error] " + modulePath + "can't patch normally :(");

                assemblyData.Dispose();
                return;
            }

            assemblyData.Dispose();

            // .dll -> .dll.original
            File.Move(modulePath, modulePathNameStandardized + ".original", true);

            // .dll.new - > .dll
            File.Move(modulePath + ".new", modulePathNameStandardized, true);
        }//PatchDll

        public void Patch(string appRootPath, Action<int> progress, CancellationToken token)
        {
            List<string> filenameList = Directory.EnumerateFiles(appRootPath, 
                "*.dll", SearchOption.AllDirectories).ToList();
            int totalCount = filenameList.Count;
            int current = 0;

            foreach (var filename in filenameList)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    PatchDll(filename);
                    Debug.WriteLine($"[i] Patching DLL with path: {filename}.\n");
                } 
                catch (Exception ex)
                {
                    //Common.Log.Error(Common.LogCategory.AppPatcher,
                    //$"Fail to patch DLL with path: {filename}. Error:\n{ex}");
                    Debug.WriteLine($"Fail to patch DLL with path: {filename}. Error:\n{ex}");
                    Debug.WriteLine($"Error Message :\n{ex.Message}");
                    continue;
                }

                current++;
                progress((int)(current * 100.0 / totalCount));
            }
        }
    }
}
