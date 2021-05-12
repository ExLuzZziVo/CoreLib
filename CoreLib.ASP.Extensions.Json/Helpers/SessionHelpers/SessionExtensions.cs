#region

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

#endregion

namespace CoreLib.ASP.Extensions.Json.Helpers.SessionHelpers
{
    public static class SessionExtensions
    {
        /// <summary>
        /// Stores user data in the session
        /// </summary>
        /// <param name="session">Session</param>
        /// <param name="key">Session key</param>
        /// <param name="value">User data to store</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Loads stored user data from the session
        /// </summary>
        /// <param name="session">Session</param>
        /// <param name="key">Session key</param>
        /// <returns>An object stored in the session using the provided session key. If it doesn't exist, the method returns the default value of <typeparamref name="T"/></returns>
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}