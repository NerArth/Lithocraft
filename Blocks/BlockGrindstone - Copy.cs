using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.ServerMods.NoObf;

// Remember to comment out logging before release

namespace Lithocraft.CodeContent
{
    internal class BlockGrindstone : Block
    {
        
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            BlockEntityGrindstone beGrindstone = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityGrindstone;
            if (beGrindstone != null)
            {
                beGrindstone.SetBusy(byPlayer, true);
            }

            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }
        
        // Everything about the OnBlockInteract functions need to be redone to integrate a BlockEntityGrindstone!
        /*
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            api.Logger.Debug("LC Grindstone interaction start");
            ItemStack heldStack = byPlayer.InventoryManager.GetHotbarItemstack(byPlayer.InventoryManager.ActiveHotbarSlotNumber);
            ItemSlot itemSlot = byPlayer.InventoryManager.ActiveHotbarSlot; if (itemSlot is null) { return false; }

            // Check if the held item stack is valid and then check if it's a tool before continuing.
            if (heldStack is null) { api.Logger.Error("Repair: No held stack?"); return false; }
            if (heldStack.Item.Tool is null) { api.Logger.Error("Repair: Held is not a tool."); return false; }

            return true; //base.OnBlockInteractStart(world, byPlayer, blockSel);
        }
        */

        public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            //api.Logger.Debug("LC Grindstone interaction step");
            ItemStack heldStack = byPlayer.InventoryManager.GetHotbarItemstack(byPlayer.InventoryManager.ActiveHotbarSlotNumber);
            ItemSlot itemSlot = byPlayer.InventoryManager.ActiveHotbarSlot; if (itemSlot is null) { return false; }
            
            /*
            // Check if the held item stack is valid and then check if it's a tool before continuing.
            if (heldStack is null) { api.Logger.Error("Repair: No held stack?"); return false; }
            if (heldStack.Item.Tool is null) { api.Logger.Error("Repair: Held is not a tool.");  return false; }
            */

            // Local vars. Existing durability to be added to. Max for convenience.
            var prevDurability = GetRemainingDurability(heldStack);
            var maxDurability = heldStack.Collectible.GetMaxDurability(heldStack);
            var timesRepaired = 0; // init only
            var repairAmount = 10; // basically think of how many seconds it would take to fully repair a tool; if the amount was 1/s and the tool had a 3000 durability it would take that many seconds to repair! Perhaps this could be a figure based on the grindstone's material quality in itself.
            int repairTolerance = (int)Math.Round(4.0f * maxDurability);
            var timeTolerance = 0.05f;

            // How much has this tool been repaired before?
            if (heldStack.Attributes.GetInt("timesRepaired") == 0) // if never repaired, set the attribute to exist
            {
                heldStack.Attributes.SetInt("timesRepaired", 0);
            }
            else if ( timesRepaired >= repairTolerance ) { api.Logger.Error("Repair: Tool already fixed too many times.");  return false; } // if repaired too much just stop (and should make a message)
            // Otherwise, it's probably within acceptable margin of repairs, so just set the local variable based on attribute
            else { timesRepaired = heldStack.Attributes.GetInt("timesRepaired"); }

            // If for whatever reason the durability is already AT max or over it, set the durability to max and stop. Maybe with a message.
            if (prevDurability >= maxDurability) { heldStack.Item.Durability = maxDurability; api.Logger.Error("Repair: Held already at max or over max! Max reports to be: " + maxDurability); return false; }

            // Then if the tool is a type that makes sense to repair...
            if ( heldStack.Item.WildCardMatch("axe-*") )
            {
                api.Logger.Debug("LC grindstone heldstack info: dura" + prevDurability + " maxdura" + maxDurability + " times" + timesRepaired + " type" + heldStack.Item.Code );
                // If the seconds passed are FULL seconds, then increment durability and repaired times count.
                if ( Math.Abs(secondsUsed % 1) <= timeTolerance)
                {
                    api.Logger.Debug("LC grindstone used for " + secondsUsed + "sec");
                    // Send an audit log saying who's repairing a tool and where.
                    api.Logger.Debug(byPlayer.PlayerName + " is repairing a tool with a Grindstone at " + blockSel.FullPosition);
                    if (prevDurability + repairAmount > maxDurability) // if the durability would go over max, just set it to max and finish
                    {
                        api.Logger.Debug("Repair: Tool durability would go over max!");
                        //heldStack.Item.Durability = maxDurability;
                        heldStack.Attributes.SetInt("durability", maxDurability);
                        heldStack.Attributes.SetInt("timesRepaired", timesRepaired + repairAmount);
                        return false;
                    } 
                    else // otherwise, just add durability
                    {
                        api.Logger.Debug("Repair: Tool durability increased succesfully.");
                        heldStack.Attributes.SetInt("durability", prevDurability + repairAmount);
                        heldStack.Attributes.SetInt("timesRepaired", timesRepaired + repairAmount);

                        //world.SpawnCubeParticles(byPlayer.SidedPos.XYZ.Add(byPlayer.SelectionBox.Y2 / 2f), heldStack, 0.25f, 30, 1f, byPlayer); //copied/modified from collectible object
                    }
                }
            }
            else
            {
                api.Logger.Error("Repair: Tool did not match a valid type?");
                return false;
            }

            return true; //base.OnBlockInteractStep(secondsUsed, world, byPlayer, blockSel);
        }
    }
}
