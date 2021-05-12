#region

using System;
using System.Threading.Tasks;

#endregion

namespace CoreLib.CORE.Helpers.EventHelpers
{
    /// <summary>
    /// Simple delegates for events
    /// </summary>
    public static class EventHandlerDelegates
    {
        public delegate bool BoolEventHandler(object sender, EventArgs args);

        public delegate Task<bool> BoolEventHandlerAsync(object sender, EventArgs args);
    }
}