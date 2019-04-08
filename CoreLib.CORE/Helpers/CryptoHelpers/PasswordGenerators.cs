#region

using System;

#endregion

namespace CoreLib.CORE.Helpers.CryptoHelpers
{
    public static class PasswordGenerators
    {
        public static string GenerateGuidPassword()
        {
            return "A" + Guid.NewGuid().ToString("D").Substring(5, 11);
        }
    }
}