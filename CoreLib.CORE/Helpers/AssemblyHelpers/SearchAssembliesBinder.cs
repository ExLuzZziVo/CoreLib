#region

using System;
using System.Reflection;
using System.Runtime.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.AssemblyHelpers
{
    /// <summary>
    /// <see cref="SerializationBinder"/> that uses <see cref="SearchTypeHelper"/> to search the type of object
    /// </summary>
    public sealed class SearchAssembliesBinder : SerializationBinder
    {
        private readonly SearchTypeHelper _searchTypeHelper;

        /// <summary>
        /// <see cref="SerializationBinder"/> that uses <see cref="SearchTypeHelper"/> to search the type of object
        /// </summary>
        /// <param name="currentAssembly">Current assembly</param>
        /// <param name="searchInDlls">Enable search in dependent assemblies</param>
        public SearchAssembliesBinder(Assembly currentAssembly, bool searchInDlls)
        {
            _searchTypeHelper = new SearchTypeHelper(currentAssembly, searchInDlls);
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            return _searchTypeHelper.GetTypeToDeserialize(typeName);
        }
    }
}