using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lithocraft.BlockEntities;
using Lithocraft.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using Vintagestory.ServerMods.NoObf;

// Remember to comment out logging before release

namespace Lithocraft.Blocks
{
    internal class BlockGrindstone : Block
    {
        private LangUtility _langutil = new();
        public BlockEntityGrindstone? beGrindstone { get; private set;}

        public override void OnUnloaded(ICoreAPI api)
        {
            base.OnUnloaded(api);
            beGrindstone = null;
        }

        //internal bool StopFlag;
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (blockSel != null && !world.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use)) return false;

            //api.Logger.Debug("Grindstone interaction start");

            if (beGrindstone is null || beGrindstone.firstEvent)
            {
                beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);
            }

            //beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);

            /*
            if (beGrindstone == null)
            {
                api.Logger.Debug("beGrindstone is null at position: " + blockSel.Position);
                api.Logger.Debug("position reports it is a " + blockSel.Block.GetPlacedBlockName(world,blockSel.Position));
            }
            else if (beGrindstone.GetBusy())
            {
                api.Logger.Debug("beGrindstone is already busy");
            }
            */

            if (beGrindstone != null && !beGrindstone.GetBusy())
            {
                if (!beGrindstone.CheckRepairable(byPlayer)) return false;
                if (beGrindstone.GetBusy()) {_langutil.ProvideErrorFeedback("lithocraft:grindstone-alreadyinuse", "lithocraft:feedback-grindstone-alreadyinuse", api); return false; }
                else if (beGrindstone.CanRepair)
                {
                    beGrindstone.LoadTickListener();
                    //api.Logger.Debug("Grindstone set to busy by " + byPlayer.PlayerName);
                    beGrindstone.SetBusy(byPlayer, true);
                    beGrindstone.firstEvent = true;
                    beGrindstone.ToggleSound();
                    return true;
                }
                else
                {
                    beGrindstone.ToggleSound();
                    return false;
                }
            }
            else return false;
        }

        public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            base.OnBlockInteractStep(secondsUsed, world, byPlayer, blockSel);
            //api.Logger.Debug("Grindstone busy step by " + byPlayer.PlayerName);

            //if (beGrindstone is null || beGrindstone.firstEvent)
            //{
            //    beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);
            //}
            

            if (beGrindstone is null) return false;
            if (beGrindstone.BusyPlayer is null) return false;
            //if (StopFlag) { StopFlag = false; return false; }
            if (!beGrindstone.CheckRepairable(byPlayer)) return false;
            if (!beGrindstone.GetBusy()) return false;
            /*{
                beGrindstone.firstEvent = false;
                //beGrindstone.MarkDirty();
                return false;
            }*/
            /*else if (beGrindstone.GetBusy())
            {
                //api.Logger.Debug("Grindstone continuing...");
                //beGrindstone.UpdateRepair(byPlayer);
                return true;
            }*/
            if (beGrindstone.firstEvent) beGrindstone.firstEvent = false;
            return true;
        }
        
        public override bool OnBlockInteractCancel(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
        {
            //api.Logger.Debug("Grindstone interaction cancel");

            //beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);

            //if (beGrindstone != null)
            //{
            //    if (beGrindstone.GetBusy() && beGrindstone.CheckRepairable(byPlayer))
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //if (beGrindstone is null) return false;
            beGrindstone?.ClearData();
            beGrindstone?.SetBusy(byPlayer, false);
            if (beGrindstone != null && beGrindstone.firstEvent) beGrindstone.firstEvent = false;
            beGrindstone?.ToggleSound();
            return true;
            //    }
            //}

            //return true;//base.OnBlockInteractCancel(secondsUsed, world, byPlayer, blockSel, cancelReason);
        }
        
        public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            //api.Logger.Debug("Grindstone interaction stopped");

            beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);

            if (beGrindstone is null) return;
            beGrindstone.ClearData();
            beGrindstone.SetBusy(byPlayer, false);
            beGrindstone.firstEvent = false;
            beGrindstone.ToggleSound();

            //beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);

            //if (beGrindstone != null)
            //{
            //    {
            //        beGrindstone.SetBusy(byPlayer, false); // this should have already been set according to other logic
            //    }
            //}

        }

        // this still needs to be implemented - needs to have a dynamic itemstack list based on the items that can be repaired, which should be fetched from the json attributes etc.
        //public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
        //{
        //    return new WorldInteraction[]
        //    {
        //        new WorldInteraction()
        //        {
        //            ActionLangCode = "blockhelp-grindstone-repair",
        //            MouseButton = EnumMouseButton.Right,
        //            Itemstacks = new ItemStack[] { new ItemStack(world.GetItem(new AssetLocation("pickaxe-iron"))), new ItemStack(world.GetItem(new AssetLocation("chisel-iron"))) }
        //        }
        //    }.Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));

        //    //return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
        //}
    }
}
