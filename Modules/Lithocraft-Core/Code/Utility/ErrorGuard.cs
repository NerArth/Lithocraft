using System;
using Vintagestory.API.Common;

namespace Lithocraft.Core.Utility
{
    public static class ErrorGuard
    {
        public static void TryExecute(ILogger logger, Action action, string contextMessage = "")
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                logger.Error($"[Lithocraft-Core ErrorGuard] Exception encountered: {contextMessage}. It may have been broken by a game update. Message: {ex.Message}");
                // We gracefully ignore the error so gameplay isn't completely halted.
            }
        }

        public static T? TryGet<T>(ILogger logger, Func<T> func, T? fallback = default, string contextMessage = "")
        {
            try
            {
                return func != null ? func.Invoke() : fallback;
            }
            catch (Exception ex)
            {
                logger.Error($"[Lithocraft-Core ErrorGuard] Exception encountered while fetching value: {contextMessage}. Returning fallback. Message: {ex.Message}");
                return fallback;
            }
        }
    }
}