#region

using System.Text.Json;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    internal class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return string.IsNullOrEmpty(name) ? name : name.ToLower();
        }
    }
}