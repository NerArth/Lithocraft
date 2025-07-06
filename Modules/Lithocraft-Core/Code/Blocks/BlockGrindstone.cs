using System;
using System.Collections.Generic;
using System.Linq;
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
        //internal bool StopFlag;
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (blockSel != null && !world.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use)) return false;

            api.Logger.Debug("Grindstone interaction start");

            beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);

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
            //base.OnBlockInteractStep(secondsUsed, world, byPlayer, blockSel);
            api.Logger.Debug("Grindstone busy step by " + byPlayer.PlayerName);

            beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);

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
            api.Logger.Debug("Grindstone interaction cancel");

            beGrindstone = (BlockEntityGrindstone?)world?.BlockAccessor.GetBlockEntity(blockSel?.Position);

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
            api.Logger.Debug("Grindstone interaction stopped");

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

        /*
        public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
        {
            var beg = world.BlockAccessor.GetBlockEntity(selection.Position) as BlockEntityGroundStorage;
            if (beg?.StorageProps != null)
            {
                int bulkquantity = beg.StorageProps.BulkTransferQuantity;

                if (beg.StorageProps.Layout == EnumGroundStorageLayout.Stacking && !beg.Inventory.Empty)
                {
                    var canIgniteStacks = BlockBehaviorCanIgnite.CanIgniteStacks(api, true).ToArray();

                    var collObj = beg.Inventory[0].Itemstack.Collectible;

                    return new WorldInteraction[]
                    {
                        new WorldInteraction()
                        {
                            ActionLangCode = "blockhelp-firepit-ignite",
                            MouseButton = EnumMouseButton.Right,
                            HotKeyCode = "shift",
                            Itemstacks = canIgniteStacks,
                            GetMatchingStacks = (wi, bs, es) => {
                                var begs = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityGroundStorage;
                                if (begs?.IsBurning == false && begs?.CanIgnite == true)
                                {
                                    return wi.Itemstacks;
                                }
                                return null;
                            }
                        },
                        new WorldInteraction()
                        {
                            ActionLangCode = "blockhelp-groundstorage-addone",
                            MouseButton = EnumMouseButton.Right,
                            HotKeyCode = "shift",
                            Itemstacks = new ItemStack[] { new ItemStack(collObj, 1) }
                        },
                        new WorldInteraction()
                        {
                            ActionLangCode = "blockhelp-groundstorage-removeone",
                            MouseButton = EnumMouseButton.Right,
                            HotKeyCode = null
                        },

                        new WorldInteraction()
                        {
                            ActionLangCode = "blockhelp-groundstorage-addbulk",
                            MouseButton = EnumMouseButton.Right,
                            HotKeyCodes = new string[] {"ctrl", "shift" },
                            Itemstacks = new ItemStack[] { new ItemStack(collObj, bulkquantity) }
                        },
                        new WorldInteraction()
                        {
                            ActionLangCode = "blockhelp-groundstorage-removebulk",
                            HotKeyCode = "ctrl",
                            MouseButton = EnumMouseButton.Right
                        }

                    }.Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
                }

                if (beg.StorageProps.Layout == EnumGroundStorageLayout.SingleCenter)
                {
                    return new WorldInteraction[]
                    {
                        new WorldInteraction()
                        {
                            ActionLangCode = "blockhelp-behavior-rightclickpickup",
                            MouseButton = EnumMouseButton.Right
                        },

                    }.Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
                }

                if (beg.StorageProps.Layout == EnumGroundStorageLayout.Halves || beg.StorageProps.Layout == EnumGroundStorageLayout.Quadrants)
                {
                    return new WorldInteraction[]
                    {
                        new WorldInteraction()
                        {
                            ActionLangCode = "blockhelp-groundstorage-add",
                            MouseButton = EnumMouseButton.Right,
                            HotKeyCode = "shift",
                            Itemstacks = beg.StorageProps.Layout == EnumGroundStorageLayout.Halves ? groundStorablesHalves : groundStorablesQuadrants
                        },
                        new WorldInteraction()
                        {
                            ActionLangCode = "blockhelp-groundstorage-remove",
                            MouseButton = EnumMouseButton.Right,
                            HotKeyCode = null
                        }

                    }.Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
                }

            }

            return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
        }*/
    }
}
