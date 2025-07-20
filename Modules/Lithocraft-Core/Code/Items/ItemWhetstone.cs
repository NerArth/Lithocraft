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
            int ActualRepairAmount = stackInSlot.Itemstack.Attributes.GetInt("reinforcementStrength");
            // we don't want to use the base method
            //base.OnConsumedByCrafting(allInputSlots, stackInSlot, gridRecipe, fromIngredient, byPlayer, quantity);

            // it's always assumed the whetstone is marked as tool in the recipe
            if (fromIngredient.IsTool)
            {
                // unfortunately this is a duplicate of vanilla code but can't see a simpler way to handle this
                stackInSlot.Itemstack.Collectible.DamageItem(byPlayer.Entity.World, byPlayer.Entity, stackInSlot, fromIngredient.ToolDurabilityCost);

                if (stackInSlot.Itemstack.Collectible.Durability <= 1)
                {
                    stackInSlot.Itemstack.StackSize -= quantity;
                }
                else if (stackInSlot.Itemstack.StackSize <= 0)
                {
                    stackInSlot.Itemstack = null;
                    stackInSlot.MarkDirty();
                }
                for (int i = 0; i < allInputSlots.Length; i++)
                {
                    var otherStack = allInputSlots[i];
                    if (otherStack == null) { return; }
                    base.api.Logger.Debug("whetstone debug: " + allInputSlots[i].Itemstack.GetName());
                    var impossibleCheck = false;
                    
                    if (impossibleCheck)
                    {
                        if ((otherStack != stackInSlot) && (otherStack?.Itemstack.Collectible.Tool != null))
                        {
                            //gridRecipe.Output
                            var currDura = otherStack.Itemstack.Collectible.Durability;
                            otherStack.Itemstack.Attributes.SetInt("durability", currDura + ActualRepairAmount);
                            otherStack.MarkDirty();
                            return;
                        }
                    }
                }
                return;
            }

            else
            {
                return;
            }
        }
    }
}
