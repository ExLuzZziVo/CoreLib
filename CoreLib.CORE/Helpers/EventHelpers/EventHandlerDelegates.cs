#region

using System;
using System.Threading.Tasks;

#endregion

namespace CoreLib.CORE.Helpers.EventHelpers
{
    public static class EventHandlerDelegates
    {
        public delegate Task<bool> BoolEventHandlerAsync(object sender, EventArgs args);

        public delegate bool BoolEventHandler(object sender, EventArgs args);
    }
}