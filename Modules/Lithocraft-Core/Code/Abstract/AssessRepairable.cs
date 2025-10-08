namespace Lithocraft.Code.Abstract
{
    internal static class AssessRepairable
    {
        /// <summary>
        /// <para>Checks if the item in the slot is repairable by this item.</para>
        /// </summary>
        /// <typeparam name="T">The return type, usually bool.</typeparam>
        /// <param name="slot">The item slot to check.</param>
        /// <returns>True if the item is repairable, false otherwise.</returns>
        public static T AssessRepairable<T>(ItemSlot slot)
        {
            if (slot.Itemstack == null) return (T)(object)false;

            string[] validTools = LithocraftGlobals.RepairsValidTools;

            ItemStack handledStack = slot.Itemstack;

            int? dura = slot.Itemstack.Attributes.TryGetInt("durability");
            int? maxDura = slot.Itemstack.Collectible.GetMaxDurability(slot.Itemstack);

            if (dura != null && maxDura != null && dura < maxDura)
            {
                foreach (string toolType in validTools)
                {
                    if (handledStack.Collectible.ToolTypes != null && handledStack.Collectible.ToolTypes.Contains(toolType))
                    {
                        return (T)(object)true;
                    }
                }
            }
            return (T)(object)false;
        }
    }
}