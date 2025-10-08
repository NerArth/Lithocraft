using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lithocraft.BlockEntities;
using Lithocraft.Blocks;
using Lithocraft.Code.Abstract;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Lithocraft.Items
{
    internal class ItemWhetstone : Item
    {
        /// <summary>
        /// <para>Attempts to adjust the durability of an item in a slot.</para>
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="value"></param>
        /// <returns>true if the durability was adjusted, false otherwise.</returns>
        public static bool AdjustDurability(ItemSlot slot, int value)
        {
            LogPassthrough.LithocraftModReference.Logger.Debug("ItemWhetstone: AdjustDurability called with value " + value);
            bool duraChanged = false;
            ItemStack handledStack = slot.Itemstack;
            int? maxDura = slot.Itemstack.Collectible.GetMaxDurability(slot.Itemstack);
            int? dura = slot.Itemstack.Attributes.TryGetInt("durability");
            if (value != 0)
            {
                if ( && dura != null && maxDura != null)
                {
                    if (dura < maxDura && dura + value <= maxDura)
                    {
                        slot.Itemstack.Collectible.SetDurability(handledStack, (int)dura + value);
                        duraChanged = true;
                    }
                    else if (dura >= maxDura || dura + value >= maxDura)
                    {
                        slot.Itemstack.Collectible.SetDurability(handledStack, (int)maxDura);
                        duraChanged = true;
                    }
                    else if (dura + value <= 0)
                    {
                        slot.TakeOut(1);
                        duraChanged = true;
                    }

                    if (duraChanged) slot.MarkDirty();

                    return duraChanged;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// <para>Checks if the item in the slot is repairable by this item.</para>
        /// <para>The list of repairable items is retrieved from the "validTools" attribute in (...)/blocktypes/grindstone.json</para>
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
//        public static T AssessRepairable<T>(ItemSlot? slot = null)
//        {
//#if DEBUG_BUILD
//            LogPassthrough.LithocraftModReference.Logger.Debug("ItemWhetstone: AssessRepairable called");
//#endif
//            // TODO: this check has to be turned into some kind of internal class because it's duplicating code from BEGrindstone
//            BlockGrindstone? bg = new();
//            string[] validTools = bg.Attributes["validTools"].AsObject<string[]>();

//            foreach (string tool in validTools)
//            {
//                if (slot == null || slot.Itemstack == null)
//                {
//                    LogPassthrough.LithocraftModReference.Logger.Debug("ItemWhetstone: AssessRepairable called with null slot or itemstack");
//                    return (T)(object)validTools;
//                }
//                if (slot.Itemstack.Collectible.WildCardMatch(tool + "-*"))
//                {
//                    bg = null; // reset bg to null so it can be handled by GC
//                    return (T)(object)true;
//                }
//            }

//            bg = null; // reset bg to null so it can be handled by GC
//            return (T)(object)false;
//        }

        // TODO: write something more appropriate if this method is needed, because this is a duplicate from vanilla ItemWearable
        public override int GetMergableQuantity(ItemStack sinkStack, ItemStack sourceStack, EnumMergePriority priority)
        {
            LogPassthrough.LithocraftModReference.Logger.Debug("ItemWhetstone: GetMergableQuantity called with priority " + priority);
            if (priority == EnumMergePriority.DirectMerge) // it gets here but no further, suggesting the main issue is the code below
            {
                string[] validTools = AssessRepairable<string[]>();
                var validTool = false;
                foreach (string tool in validTools)
                {
                    if (sourceStack.Collectible.WildCardMatch(tool + "-*"))
                    {
                        validTool = true;
                        break;
                    }
                }
                if (sourceStack.Collectible.FirstCodePart() == "whetstone" && validTool)
                {
                    return 1;
                }

                return base.GetMergableQuantity(sinkStack, sourceStack, priority);
            }

            return base.GetMergableQuantity(sinkStack, sourceStack, priority);
        }

        public override void TryMergeStacks(ItemStackMergeOperation op)
        {
            LogPassthrough.LithocraftModReference.Logger.Debug("ItemWhetstone: TryMergeStacks called with priority " + op.CurrentPriority);
            var sourceStack = op.SourceSlot.Itemstack;
            var sinkStack = op.SinkSlot.Itemstack;
            if (op.CurrentPriority == EnumMergePriority.DirectMerge)
            {
                int? maxDura = sinkStack.Collectible.GetMaxDurability(sinkStack);
                int? dura = sinkStack.Attributes.TryGetInt("durability");
                int repairAmt = sourceStack.Attributes.TryGetInt("reinforcementStrength") ?? 0;
                if (repairAmt > 0 && dura < maxDura)
                {
                    AdjustDurability(op.SinkSlot, repairAmt);
                    AdjustDurability(op.SourceSlot, -1);
                    return;
                }
            }

            base.TryMergeStacks(op);
        }
    }
}
