using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Lithocraft.Blocks;
using Lithocraft.Util;
using ProtoBuf;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using Vintagestory.Server;
using Vintagestory.ServerMods.NoObf;

namespace Lithocraft.BlockEntities
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    //internal class BlockEntityGrindstone : BlockEntityDisplay // reference BEs: quern, fruit press, anvil
    internal class BlockEntityGrindstone : BlockEntity // reference BEs: quern, fruit press, anvil
    {
        #region classvariables
        // variables
        //   functional
        private ICoreClientAPI? capi;
        public BlockGrindstone? ownBlock;
        private LangUtility _langutil = new LangUtility();
        internal long listenerID;
        public bool StopFlag;

        public int[] TierDuraModifier = { 2, 4, 5 }; // limits 2x 4x 5x
        //public int[] TierRepairModifier = { 2, 6, 20 }; // repair frequency500ms 166ms 50ms; no longer applicable
        public int[] TierRepairModifier = { 1, 2, 3 }; // originally 1, 3, 10
        public bool StateBusy;
        public bool CanRepair;
        public IPlayer BusyPlayer;

        public bool firstEvent;
        BlockEntityAnimationUtil animUtil
        {
            get { return GetBehavior<BEBehaviorAnimatable>()?.animUtil; }
        }
        Vec3f rendererRot = new Vec3f();

        ILoadedSound AmbientSound;

        public void ToggleSound()
        {
            if (AmbientSound != null && !AmbientSound.IsPlaying && firstEvent)
            {
                AmbientSound?.Start();
                MarkDirty();
            }
            else if (AmbientSound != null && AmbientSound.IsPlaying && !GetBusy())
            {
                AmbientSound?.Stop();
                MarkDirty();
            }
            else if (!GetBusy())
            {
                AmbientSound?.Stop();
                MarkDirty();
            }
        }

        //   other
        public enum Tier
        {
            Stone,
            Ceramic,
            Diamond
        }

        public Tier CurrentTier { get; private set; }

        /*private enum ValidToolsEnum
        {
            axe,
            pickaxe,
            prospectingpick,
            shovel,
            falx,
            hoe,
            knife,
            scythe,
            shears,
            hammer,
            chisel,
            saw,
            spear,
            wrench,
            cleaver
        };*/
        /*protected string[] ValidTools { get; } =
        {
            "axe",
            "pickaxe",
            "prospectingpick",
            "shovel",
            "blade",
            "hoe",
            "knife",
            "scythe",
            "shears",
            "chisel",
            "saw",
            "spear",
            "cleaver"
        };*/

        protected string[] ValidTools => Block.Attributes["validTools"].AsObject<string[]>();

        // handled item-stack
        ItemStack heldStack;
        public ItemSlot itemSlot;

        //string handledToolType; // ended up not needing
        int prevDurability;
        int maxDurability;
        int timesRepaired;
        int repairTolerance;
        int repairAmount;
        #endregion

        #region particles
        static SimpleParticleProperties GrindingParticles;

        static BlockEntityGrindstone()
        {
            GrindingParticles = new SimpleParticleProperties()
            {
                MinVelocity = new Vec3f(-0.04f, 0f, -0.04f),
                AddVelocity = new Vec3f(0.08f, 0.05f, 0.08f),
                addLifeLength = 0.25f,
                LifeLength = 0.5f,
                MinQuantity = 0.75f,
                GravityEffect = 0.5f,
                SelfPropelled = true,
                MinSize = 0.05f,
                MaxSize = 0.10f
            };
        }
        #endregion

        /*public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            base.FromTreeAttributes(tree, worldAccessForResolve);
        }*/

        /// <summary>
        /// <para>Returns the CurrentTier, assuming it has been initialised.</para>
        /// </summary>
        /// <returns></returns>
        public Tier GetTier()
        {
            return CurrentTier;
        }

        /// <summary>
        /// <para>Sets the BE tier; only meant for internal use but deliberately left public.</para>
        /// </summary>
        /// <param name="tier"></param>
        public void SetTier(Tier tier)
        {
            CurrentTier = tier;
        }

        /// <summary>
        /// <para>Returns the current StateBusy (true|false).</para>
        /// </summary>
        /// <returns></returns>
        public bool GetBusy()
        {
            return StateBusy;
        }

        /// <summary>
        /// <para>Sets BusyPlayer and StateBusy in the BE.</para>
        /// </summary>
        /// <param name="player"></param>
        /// <param name="busy"></param>
        /// <returns></returns>
        public bool SetBusy(IPlayer player, bool busy)
        {
            BusyPlayer = player;
            StateBusy = busy;

            /*if (StateBusy && CanRepair)
            {
                AmbientSound?.Start();
            }*/
            /*else
            {
                AmbientSound?.Stop();
            }*/

            if (heldStack != null && itemSlot != null)
            {
                CheckDurability();
            }

            // should probably let the client know the states have been set
            if (Api.Side == EnumAppSide.Server)
            {
                MarkDirty();
            }

            return StateBusy;
        }

        public long LoadTickListener()
        {
            listenerID = RegisterGameTickListener(Every500ms, 500);
            return listenerID;
        }

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            // These have been a pain to understand how to work with conceptually.
            //RegisterGameTickListener(EveryTIERms, 1000 / TierTimeModifier[(int)CurrentTier]);
            //RegisterGameTickListener(Every500ms, 500);
            /* tbh should probably only register this right before it's needed and then unregister it again,
             * as otherwise every grindstone will always have a registered tick listener for no reason?
             */

            this.ownBlock = Block as BlockGrindstone;
            capi = api as ICoreClientAPI;

            //RegisterGameTickListener(Every100ms, 100);

            // init the tier as retrieved from the block data
            switch (this.ownBlock?.Variant["type"])
            {
                case "stone":
                    SetTier(Tier.Stone); break;
                case "ceramic":
                    SetTier(Tier.Ceramic); break;
                case "diamond":
                    SetTier(Tier.Diamond); break;
            }

            if (AmbientSound == null && api.Side == EnumAppSide.Client)
            {
                AmbientSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams()
                {
                    Location = new AssetLocation("lithocraft:sounds/effect/grindstonesharpen1.ogg"),
                    ShouldLoop = true,
                    Position = Pos.ToVec3f().Add(0.5f, 0.4f, 0.5f),
                    DisposeOnFinish = false,
                    Volume = 0.66f,
                    Range = 12,
                    Pitch = 1
                });
            }
        }

        public override void OnBlockRemoved()
        {
            base.OnBlockRemoved();

            UnregisterGameTickListener(listenerID);
        }

        public override void OnBlockUnloaded()
        {
            base.OnBlockUnloaded();

            AmbientSound?.Dispose(); // I think this is required since the ambient sound is set to not auto-dispose; we probably want to dispose of it to prevent memory issues?
        }

        #region timingmethods
        private void Every100ms(float dt) // for adjusting sound and creating particles
        {
            if (Api.Side == EnumAppSide.Client)
            {
                /*
                // something is making the position stuff return null so just leave this for now
                if (StateBusy)
                {
                    GrindingParticles.Color = 0xffeeaa;
                    GrindingParticles.Color &= 0xffffff;
                    GrindingParticles.Color |= (200 << 24);
                    GrindingParticles.MinQuantity = 1;
                    GrindingParticles.AddQuantity = 5;
                    //GrindingParticles.MinPos.Set(Pos.X - 1 / 32f, Pos.Y + 11 / 16f, Pos.Z - 1 / 32f);
                    GrindingParticles.Pos.Set(Pos.X, Pos.Y + 11 / 16f, Pos.Z);
                    GrindingParticles.MinVelocity.Set(-0.1f, 0, -0.1f);
                    GrindingParticles.AddVelocity.Set(0.2f, 0.2f, 0.2f);


                    if (GrindingParticles.Pos is null) return; else Api.World.SpawnParticles(GrindingParticles);
                }
                */
                /*
                if (ambientSound != null && automated && mpc.TrueSpeed != prevSpeed)
                {
                    prevSpeed = mpc.TrueSpeed;
                    ambientSound.SetPitch((0.5f + prevSpeed) * 0.9f);
                    ambientSound.SetVolume(Math.Min(1f, prevSpeed * 3f));
                }
                else prevSpeed = float.NaN;
                */

                return;
            }
        }

        private void Every500ms(float dt) // for doing the repairs
        {
            //if (CheckRepairable(BusyPlayer) is false) {ClearData();}

            //if (GetBusy())
            
            if (BusyPlayer != null && CanRepair)
            {
                Api.Logger.Debug("Grindstone is running repair update: " + dt);
                if (!CheckRepairable(BusyPlayer)) { SetBusy(BusyPlayer, false); ClearData(); ToggleSound();  return; }
                SetBusy(BusyPlayer, true);
                UpdateRepair(BusyPlayer);
                MarkDirty();
            }
        }
        #endregion

        public bool CheckRepairable(IPlayer byPlayer) // check that the repair can happen
        {
            if (Api?.World is null) { Api.Logger.Debug("LC Grindstone: World is null?"); return false; }
            var player = byPlayer;
            if (player is null) return false;
            bool isValidTool = false;
            CheckHeldData(player); if (heldStack is null || heldStack.Item is null) return false; 
            CheckDurability();
            // this is important because the item checks need to happen and they can only happen if we know who the player is
            //if (!StateBusy) { Api.Logger.Debug("StateBusy is: " + StateBusy); return false; }
            //if (!StateBusy) SetBusy(player,false);

            //else
            //{
            //Api.Logger.Debug("Grindstone is busy! Being used by: " + player.PlayerName);
            if (player?.InventoryManager != null)
            {

                //Api.Logger.Debug("Repair: checking item stack and slot...");
                // check the stack and the item slot
                //ItemStack heldStack = player.InventoryManager.GetHotbarItemstack(player.InventoryManager.ActiveHotbarSlotNumber); if (heldStack is null) return false;
                //ItemSlot itemSlot = player.InventoryManager.ActiveHotbarSlot; if (itemSlot is null) return false;

                //Api.Logger.Debug("Repair: checking item TYPE valid...");
                // make sure the item is a valid item before we do anything else
                foreach  (string tool in ValidTools)
                {
                    if (heldStack.Item.WildCardMatch(tool+"-*"))
                    {
                        isValidTool = true;
                        CanRepair = true;
                        break;
                        /*handledToolType = tool;*/
                    } 
                }
                // all of the error feedback things here need to change so they become localised and read from the en.json lang file
                if (!isValidTool)
                {
                    _langutil.ProvideErrorFeedback("lithocraft:grindstone-toolnotvalid", "lithocraft:feedback-grindstone-toolnotvalid", capi);
                    return false;
                }
                    
                // if repaired too much just stop (and should make a message)
                if (timesRepaired >= repairTolerance)
                {
                    //Api.Logger.Debug("Repair: Tool already fixed too many times.");
                    _langutil.ProvideErrorFeedback("lithocraft:grindstone-tooltoofargone", "lithocraft:feedback-grindstone-tooltoofargone", capi);
                    //SetBusy(BusyPlayer, false);
                    CanRepair = false;
                    return false;
                }
                else if (prevDurability == maxDurability) 
                {
                    _langutil.ProvideErrorFeedback("lithocraft:grindstone-toolalreadymax", "lithocraft:feedback-grindstone-toolalreadymax", capi);
                    CanRepair = false;
                    return false;
                }
            }
            //}
            return true;
        }

        /// <summary>
        /// <para>Checks if an item has ever been repaired and sets things appropriately if it's a valid type.</para>
        ///
        /// <para>Returns an ItemStack-dependent array of prevDurability, maxDurability, repairTolerance, repairAmount.</para>
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        private int[] CheckDurability() 
        {
            if (Api?.World is null) return null; if (heldStack is null) return null; if (itemSlot is null) return null;
            // init array
            int[] array = new int[4];

            // define the variables
            float permil = 0.009f; // this is the base repair amount in permil form (0.9%)
            prevDurability = heldStack.Collectible.GetRemainingDurability(heldStack);
            maxDurability = heldStack.Collectible.GetMaxDurability(heldStack);
            repairTolerance = TierDuraModifier[(int)GetTier()] * maxDurability;
            //repairAmount = 1 * (TierRepairModifier[(int)GetTier()] / 2); // this could potentially be a percentage instead
            // rounded up ‰ of tool max dura times the repair modifier
            repairAmount = (int)(Math.Ceiling(permil * maxDurability) * TierRepairModifier[(int)GetTier()]);

            // set the item stack to have the "timesRepaired" attribute if it didn't exist
            int? timesRepairedRetrieved = heldStack?.Attributes?.GetInt("timesRepaired");

            if (timesRepairedRetrieved is null || timesRepairedRetrieved == 0)
            {
                //Api.Logger.Debug("Repair: item not previously repaired...");
                heldStack.Attributes.SetInt("timesRepaired", 0);
                timesRepaired = 0;
            }
            else
            {
                timesRepaired = timesRepairedRetrieved.Value;
                //Api.Logger.Debug("Repair: item previously repaired! reports: " + timesRepaired);
            }
            array[0] = prevDurability;
            array[1] = maxDurability;
            array[2] = repairTolerance;
            array[3] = repairAmount;
            // return the array of handled variables
            return array;
        }

        public bool UpdateRepair(IPlayer byPlayer)
        {
            if (Api?.World is null) { Api.Logger.Debug("World is null?"); return false; }
            var player = byPlayer;

            // then if the tool is actually repairable after all checks...
            if (prevDurability < maxDurability && timesRepaired < maxDurability * repairTolerance)
            {
                if (prevDurability + repairAmount > maxDurability) // if the durability would go over max, just set it to max and finish
                {
                    //Api.Logger.Debug("Repair: Tool durability would go over max. Capping!");
                    //heldStack.Item.Durability = maxDurability;
                    heldStack.Attributes.SetInt("durability", maxDurability);
                    //heldStack.Attributes.SetInt("timesRepaired", timesRepaired + repairAmount - (maxDurability - prevDurability));
                    heldStack.Attributes.SetInt("timesRepaired", timesRepaired + (maxDurability - prevDurability));
                    //SetBusy(BusyPlayer,false); // we don't do this anymore
                    itemSlot.MarkDirty(); //resync item
                    ClearData();
                    return false;
                }
                else // otherwise, just add durability
                {
                    //Api.Logger.Debug("Repair: Tool durability increased succesfully.");
                    heldStack.Attributes.SetInt("durability", prevDurability + repairAmount);
                    heldStack.Attributes.SetInt("timesRepaired", timesRepaired + repairAmount);
                    //world.SpawnCubeParticles(byPlayer.SidedPos.XYZ.Add(byPlayer.SelectionBox.Y2 / 2f), heldStack, 0.25f, 30, 1f, byPlayer); //copied/modified from collectible object
                    itemSlot.MarkDirty(); //resync item
                    //CheckDurability();
                    //ClearData();
                    //return true;
                }
            }
            else
            {
                //Api.Logger.Debug("Repair: something went wrong and no repair action took place!");
                //SetBusy(BusyPlayer, false); // we don't do this anymore 
                ClearData();
                return false;
            }

            return false;
        }

        internal void CheckHeldData(IPlayer byPlayer)
        {
            // just see what the player is holding and keep the data in the BE
            if (Api?.World is null) return;

            var player = byPlayer; if (player is null) return;

            if (player.InventoryManager.GetHotbarItemstack(player.InventoryManager.ActiveHotbarSlotNumber) != null)
            {
                heldStack = player.InventoryManager.GetHotbarItemstack(player.InventoryManager.ActiveHotbarSlotNumber);
            }
            else heldStack = null;

            if (player.InventoryManager.ActiveHotbarSlot != null)
            {
                itemSlot = player.InventoryManager.ActiveHotbarSlot;
            }
            else itemSlot = null;

            return;
        }

        internal void ClearData()
        {
            // clear any previous held tool data
            heldStack = null;
            itemSlot = null;

            //handledToolType = null;
            prevDurability = 0;
            maxDurability = 0;
            timesRepaired = 0;
            repairTolerance = 0;
            repairAmount = 0;

            CanRepair = false;
            
            UnregisterGameTickListener(listenerID);
        }

        public void RequestInteractionStop()
        {
            StopFlag = true;
            ClearData();
            StopFlag = false;
            MarkDirty();
            //BlockGrindstone _block = Block as BlockGrindstone;
            //_block.StopFlag = true;
        }

        /* no point having this anymore honestly, can be achieved through regular block description
        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            base.GetBlockInfo(forPlayer, dsc);

            string tier = GetTier().ToString();

            dsc.AppendLine(Lang.Get("Grindstone tier: ", tier));
        }
        */

        /*
        /// <summary>
        /// Triggers an in-game error message over the hotbar displaying feedbackmsg with an internal errorcode.
        /// </summary>
        /// <param name="errorcode"></param>
        /// <param name="feedbackmsg"></param>
        private void ProvideErrorFeedback(string errorcode, string feedbackmsg)
        {
            // for providing client-side feedback
            if (Api.Side == EnumAppSide.Client)
            {
                (Api as ICoreClientAPI).TriggerIngameError(this, errorcode, Lang.Get(feedbackmsg));
            }
        }*/
    }
}
