using System;
using Vintagestory.API.Common;

namespace Lithocraft.Core.Utility
{
    public static class SafeRegistrar
    {
        public static void RegisterBlockClass(ICoreAPI api, string className, Type blockType)
        {
            try
            {
                api.RegisterBlockClass(className, blockType);
            }
            catch (Exception ex)
            {
                api.Logger.Error($"[Lithocraft-Core] Failed to register block class '{className}' with type '{blockType.Name}'. It may have been broken by a game update. Exception: {ex.Message}");
            }
        }

        public static void RegisterItemClass(ICoreAPI api, string className, Type itemType)
        {
            try
            {
                api.RegisterItemClass(className, itemType);
            }
            catch (Exception ex)
            {
                api.Logger.Error($"[Lithocraft-Core] Failed to register item class '{className}' with type '{itemType.Name}'. It may have been broken by a game update. Exception: {ex.Message}");
            }
        }

        public static void RegisterBlockEntityClass(ICoreAPI api, string className, Type blockEntityType)
        {
            try
            {
                api.RegisterBlockEntityClass(className, blockEntityType);
            }
            catch (Exception ex)
            {
                api.Logger.Error($"[Lithocraft-Core] Failed to register block entity class '{className}' with type '{blockEntityType.Name}'. It may have been broken by a game update. Exception: {ex.Message}");
            }
        }

        public static void RegisterEntity(ICoreAPI api, string className, Type entityType)
        {
            try
            {
                api.RegisterEntity(className, entityType);
            }
            catch (Exception ex)
            {
                api.Logger.Error($"[Lithocraft-Core] Failed to register entity '{className}' with type '{entityType.Name}'. It may have been broken by a game update. Exception: {ex.Message}");
            }
        }

        public static void RegisterBlockBehaviorClass(ICoreAPI api, string className, Type blockBehaviorType)
        {
            try
            {
                api.RegisterBlockBehaviorClass(className, blockBehaviorType);
            }
            catch (Exception ex)
            {
                api.Logger.Error($"[Lithocraft-Core] Failed to register block behavior class '{className}' with type '{blockBehaviorType.Name}'. It may have been broken by a game update. Exception: {ex.Message}");
            }
        }
    }
}