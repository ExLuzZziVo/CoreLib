#region

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.AssemblyHelpers
{
    public sealed class SearchAssembliesBinder : SerializationBinder
    {
        private readonly Assembly _currentAssembly;
        private readonly bool _searchInDlls;

        public SearchAssembliesBinder(Assembly currentAssembly, bool searchInDlls)
        {
            _currentAssembly = currentAssembly ?? Assembly.GetCallingAssembly();
            _searchInDlls = searchInDlls;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            var assemblyNames = new List<AssemblyName> {_currentAssembly.GetName()};
            // EXE

            if (_searchInDlls) assemblyNames.AddRange(_currentAssembly.GetReferencedAssemblies()); // DLLs

            foreach (var an in assemblyNames)
            {
                var typeToDeserialize = GetTypeToDeserialize(typeName, an);
                if (typeToDeserialize != null) return typeToDeserialize; // found
            }

            return null; // not found
        }

        private static Type GetTypeToDeserialize(string typeName, AssemblyName an)
        {
            var fullTypeName = $"{typeName}, {an.FullName}";
            var typeToDeserialize = Type.GetType(fullTypeName);
            return typeToDeserialize;
        }
    }
}