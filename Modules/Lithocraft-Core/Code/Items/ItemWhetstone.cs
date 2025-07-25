using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace Lithocraft.Items
{
    internal class ItemWhetstone : Item
    {
        public override void OnConsumedByCrafting(ItemSlot[] allInputSlots, ItemSlot stackInSlot, GridRecipe gridRecipe, CraftingRecipeIngredient fromIngredient, IPlayer byPlayer, int quantity)
        {
            // we use reinforcementStrength in the json definition to keep things to a vanilla standard
            int ReinfStr = stackInSlot.Itemstack.Attributes.GetInt("reinforcementStrength");
            int DurabilityToApply;

            ItemSlot[] inputs = allInputSlots;
            
            foreach (ItemSlot slot in inputs)
            {
                if (!slot.Empty)
                {
                    if (slot.Itemstack.Collectible.Tool != null && slot.Itemstack.Attributes.HasAttribute("durability"))
                    {
                        int curDura = slot.Itemstack.Attributes.GetInt("durability");
                        int maxDura = slot.Itemstack.Collectible.GetMaxDurability(slot.Itemstack);

                        if (ReinfStr + curDura > maxDura)
                        {
                            DurabilityToApply = maxDura - curDura;
                        }
                        else
                        {
                            DurabilityToApply = ReinfStr;
                        }

                        slot.Itemstack.Attributes.SetInt("durability", DurabilityToApply);
                        slot.MarkDirty();
                    }
                }
            }
            base.OnConsumedByCrafting(allInputSlots, stackInSlot, gridRecipe, fromIngredient, byPlayer, quantity);
        }
    }
}
