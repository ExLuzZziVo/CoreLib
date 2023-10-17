#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#endregion

namespace CoreLib.CORE.Helpers.AssemblyHelpers
{
    /// <summary>
    /// A class that searches for a type by its name
    /// </summary>
    public class SearchTypeHelper
    {
        /// <summary>
        /// List of assemblies to search for
        /// </summary>
        private readonly List<AssemblyName> _assemblyNames;

        /// <summary>
        /// Initializing parameters to find a type by its name
        /// </summary>
        /// <param name="currentAssembly">Current assembly</param>
        /// <param name="searchInDlls">Enable search in dependent assemblies</param>
        /// <remarks>
        /// If <paramref name="currentAssembly"/> is null, then <see cref="StackFrame"/> is used to get initial assembly. If <see cref="StackFrame"/> is null, then executing assembly is used
        /// </remarks>
        public SearchTypeHelper(Assembly currentAssembly, bool searchInDlls)
        {
            if (currentAssembly == null)
            {
                var stackTrace = new StackTrace().GetFrames();

                if (stackTrace == null)
                {
                    currentAssembly = Assembly.GetExecutingAssembly();
                }
                else
                {
                    _assemblyNames = new List<AssemblyName>();

                    foreach (var a in stackTrace.Select(a => a.GetMethod().ReflectedType.Assembly).Distinct().ToArray())
                    {
                        _assemblyNames.Add(a.GetName());
                        _assemblyNames.AddRange(a.GetReferencedAssemblies());
                    }

                    if (searchInDlls)
                    {
                        _assemblyNames.AddRange(AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName())
                            .ToArray());

                        _assemblyNames = _assemblyNames.Distinct().ToList();
                    }

                    return;
                }
            }

            _assemblyNames = new List<AssemblyName> { currentAssembly.GetName() };

            if (searchInDlls)
            {
                _assemblyNames.AddRange(currentAssembly.GetReferencedAssemblies());
            }
        }

        /// <summary>
        /// Recursively searches for a type by name using a regular expression
        /// </summary>
        /// <param name="typeName">Name of the type to search</param>
        /// <returns>The type that matches the supplied name. If the type is not found, returns null</returns>
        public Type GetTypeToDeserialize(string typeName)
        {
            var isCollectionRegex = Regex.Match(typeName,
                @"^(?<gen>[^\[]+)\[\[(?<type>[^\]]*)\](,\[(?<type>[^\]]*)\])*\]$");

            foreach (var an in _assemblyNames)
            {
                if (isCollectionRegex.Success)
                {
                    var collectionTypeName = RemoveAssemblyNameFromTypeName(isCollectionRegex.Groups["gen"].Value);
                    var collectionType = GetTypeToDeserialize(collectionTypeName);

                    if (collectionType == null)
                    {
                        continue;
                    }

                    var genericTypes = new List<Type>(isCollectionRegex.Groups["type"].Length);

                    foreach (var c in isCollectionRegex.Groups["type"].Captures)
                    {
                        var objectTypeName = RemoveAssemblyNameFromTypeName(c.ToString());
                        var objectType = GetTypeToDeserialize(objectTypeName);

                        if (objectType == null)
                        {
                            continue;
                        }

                        genericTypes.Add(objectType);
                    }

                    if (!genericTypes.Any())
                    {
                        continue;
                    }

                    //var objectTypeName = RemoveAssemblyNameFromTypeName(isCollectionRegex.Groups["type"].Value);
                    //var objectType = GetTypeToDeserialize(objectTypeName);
                    //if (objectType == null)
                    //    continue;
                    var typeToDeserialize = collectionType.MakeGenericType(genericTypes.ToArray());

                    return typeToDeserialize;
                }
                else
                {
                    var fullTypeName = $"{typeName}, {an.FullName}";
                    var typeToDeserialize = Type.GetType(fullTypeName);

                    if (typeToDeserialize == null)
                    {
                        continue;
                    }

                    return typeToDeserialize;
                }
            }

            return null;
        }

        /// <summary>
        /// Removes the assembly name from the type name
        /// </summary>
        /// <param name="typeName">Type name</param>
        /// <returns>Type name without the assembly name</returns>
        private static string RemoveAssemblyNameFromTypeName(string typeName)
        {
            if (typeName.Contains(","))
            {
                return typeName.Substring(0, typeName.IndexOf(','));
            }

            return typeName;
        }
    }
}