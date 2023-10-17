#region

using System.Text.Json;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    internal class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return string.IsNullOrEmpty(name) ? name : name.ToUpper();
        }
    }
}