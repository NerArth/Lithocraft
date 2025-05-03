using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Common;
using Vintagestory.GameContent;
using static System.Net.Mime.MediaTypeNames;
using static Vintagestory.Server.Timer;

namespace Lithocraft.CodeContent
{
    /// <summary>
    /// <para>
    /// Class for handheld item cannon.
    /// </para>
    /// <para>
    /// Depending on json code variant, will fire different types of ammo, which are validated in GetNextAmmo(). Sample variants:
    /// </para>
    /// <code>
    /// handcannon-stone-tinbronze
    /// handcannon-rocket-steel
    /// </code>
    /// </summary>
    internal class ItemHandCannon : ItemBow
    {

        #region classvars
        bool CoolingOff;
        float CooldownTime = 2.5f; // would like to set tiers based on the metal used but this will do for now
        float CooldownRemaining;
        float[] CooldownMultiplier = {1,4}; // each [] is matched to the {type} in the order of the json's data
        float ChargingTime = 1.00f;
        //bool ConfirmedFire;
        string FiringSoundAssetLocation = "";
        float LauncherAccModifier = 0.98f;
        float FlatDamageModifier = 1f;
        bool RocketLoaded;
        string LoadedRocketType; //should be reset to null when not loaded
        #endregion

        #region getters
        /// <summary>
        /// Returns the type variant in the code:
        /// <code>
        /// handcannon-{type}-{metal}
        /// </code>
        /// </summary>
        /// <returns></returns>
        private string RetrieveLauncherType()
        {
            return this.FirstCodePart(1);
        }
        #endregion
        /*
        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
            //api.Event.RegisterCallback(ResetCooldown, CooldownTime);
        }
        */
        /// <summary>
        /// Returns a validated ItemSlot from byEntity, unless in creative.
        /// </summary>
        /// <param name="byEntity"></param>
        /// <returns></returns>
        protected new virtual ItemSlot GetNextArrow (EntityAgent byEntity) // much of this is verbatim the original GetNextArrow() code, then modified 
        {
            ItemSlot slot = null;

            byEntity.WalkInventory((invSlot) =>
            {
                if (invSlot is ItemSlotCreative) return true;

                switch (RetrieveLauncherType())
                {
                    case "stone":
                        if (invSlot.Itemstack != null && invSlot.Itemstack.Collectible.WildCardMatch("stone-*"))
                        {
                            slot = invSlot;
                            return false;
                        }
                        break;
                    case "rocket":
                        if (invSlot.Itemstack != null && invSlot.Itemstack.Collectible.WildCardMatch("cannonrocket-*"))
                        {
                            slot = invSlot;
                            return false;
                        }
                        break;
                }
                return true;
            });

            return slot;
        }
        /// <summary>
        /// Callback method. Resets CoolingOff to false if the item was on cooldown.
        /// </summary>
        /// <param name="dt"></param>
        private void ResetCooldown(float dt)
        {
            if (CoolingOff) CoolingOff = false;
            //api.Event.UnregisterCallback(ObjectCacheUtil.TryGet<long>(api, ResetCooldown));
            return;
        }

        // custom implementation of ItemBow
        public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
        {
            base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
            LangUtility _langutil = new LangUtility();
            // presumably this stops the interaction if any default preventions should take place
            if (handling == EnumHandHandling.PreventDefault) return;

            // check if we can actually fire
            if (CoolingOff)
            {
                //ProvideErrorFeedback("handcannoncooldown", "Can't fire again yet"); // needs to be localised!
                _langutil.ProvideErrorFeedback("handcannon-cooldown", "lithocraft:feedback-handcannon-cooldown",api);
                return;
            }

            // check for ammo
            ItemSlot invSlot = GetNextArrow(byEntity);
            if (invSlot == null) return;

            // need to move these render variant things into TryLoad
            /*
            if (byEntity.World is IClientWorldAccessor) // need to check what this is for // seems this uses the alternate shapes in ItemBow's json data to set the "animation" state of the bow
            {
                slot.Itemstack.TempAttributes.SetInt("renderVariant", 1);
            }

            slot.Itemstack.Attributes.SetInt("renderVariant", 1); // again, not sure what this is for
            */
            if (!RocketLoaded && !CoolingOff && RetrieveLauncherType() == "rocket")
            {
                string _rocketToLoad = invSlot.Itemstack.Collectible.Variant["type"];//invSlot.Itemstack.Collectible.FirstCodePart(1);
                TryLoadRocket(byEntity, slot);
            }

            IPlayer byPlayer = null;
            if (byEntity is EntityPlayer)
            {
                byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);

                // this should prevent from firing if targetting a block and not in creative, acting as a safety
                //var _player = byEntity as EntityPlayer;
                //if (_player.World.PlayerByUid(_player.PlayerUID).WorldData.CurrentGameMode == EnumGameMode.Creative && blockSel != null) return;
                if (byPlayer.WorldData.CurrentGameMode == EnumGameMode.Creative && blockSel != null) return;
            }
            byEntity.World.PlaySoundAt(new AssetLocation("sounds/bow-draw"), byEntity, byPlayer, false, 8);

            handling = EnumHandHandling.PreventDefault;
        }

        // custom implementation of ItemBow
        public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
        {
            var _launchertype = 0;
            if (RetrieveLauncherType() == "rocket")
            {
                _launchertype = 1;
            }

            if (byEntity.Attributes.GetInt("aimingCancel") == 1) return;
            byEntity.Attributes.SetInt("aiming", 0);
            //byEntity.AnimManager.StopAnimation(aimAnimation);

            /* moved into loadrocket
            if (byEntity.World is IClientWorldAccessor)
            {
                slot.Itemstack.TempAttributes.RemoveAttribute("renderVariant");
            }

            slot.Itemstack.Attributes.SetInt("renderVariant", 0);
            (byEntity as EntityPlayer)?.Player?.InventoryManager.BroadcastHotbarSlot();
            */

            // probably wise to keep this so there is a hold-down to actually fire
            if (secondsUsed < ChargingTime) return;

            // validate ammo
            ItemSlot ammoSlot = GetNextArrow(byEntity);
            if (ammoSlot == null) return;

            float damage = 0;

            // Bow damage
            if (slot.Itemstack.Collectible.Attributes != null)
            {
                damage += slot.Itemstack.Collectible.Attributes["damage"].AsFloat(0);

            }

            // Arrow damage
            if (ammoSlot.Itemstack.Collectible.Attributes != null)
            {
                damage += ammoSlot.Itemstack.Collectible.Attributes["damage"].AsFloat(0);
            }

            IPlayer byPlayer = null;
            if (byEntity is EntityPlayer) byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
            byEntity.World.PlaySoundAt(new AssetLocation("sounds/bow-release"), byEntity, byPlayer, false, 8);

            if (_launchertype == 0) // stone launcing
            {
                TryFireStone(secondsUsed, ammoSlot, byEntity, blockSel, entitySel);
                LaunchAftermath(byEntity, slot, _launchertype);
            }
            else if (_launchertype == 1 && RocketLoaded) // rocket launching
            {
                TryFireRocket(secondsUsed, ammoSlot, byEntity, blockSel, entitySel);
                LaunchAftermath(byEntity, slot, _launchertype);
            }

            // after TryFire*(), if the projectile was launched, deal with the aftermath
            
            /*if (ConfirmedFire)
            {
                slot.Itemstack.Collectible.DamageItem(byEntity.World, byEntity, slot); // why doesn't the item require marking dirty after this?

                CoolingOff = true;
                CooldownRemaining = CooldownTime * CooldownMultiplier[_launchertype];
                byEntity.World.RegisterCallback(ResetCooldown, (int)CooldownRemaining * 1000); // hopefully this works
            }*/

            

            
            //byEntity.AnimManager.StartAnimation("bowhit");
        }

        protected void LaunchAftermath(EntityAgent byEntity, ItemSlot slot, int launcherType)
        {
            slot.Itemstack.Collectible.DamageItem(byEntity.World, byEntity, slot); // why doesn't the item require marking dirty after this?

            CoolingOff = true;
            CooldownRemaining = CooldownTime * CooldownMultiplier[launcherType];
            byEntity.World.RegisterCallback(ResetCooldown, (int)CooldownRemaining * 1000); // hopefully this works
        }


        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {
            base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);

            if (inSlot.Itemstack.Collectible.Attributes == null) return;

            //float dmg = inSlot.Itemstack.Collectible.Attributes?["damage"].AsFloat(0) ?? 0;
            //if (dmg != 0) dsc.AppendLine(Lang.Get("bow-piercingdamage", dmg));

            //float accuracyBonus = inSlot.Itemstack.Collectible?.Attributes["statModifier"]["rangedWeaponsAcc"].AsFloat(0) ?? 0;
            //if (accuracyBonus != 0) dsc.AppendLine(Lang.Get("bow-accuracybonus", accuracyBonus > 0 ? "+" : "", (int)(100 * accuracyBonus)));
        }

        /// <summary>
        /// Try to fire a stone from the launcher. Requires a pass-through of all OnHeldInteractStop arguments, since this method is just a custom implementation of ItemStone's throwing.
        /// </summary>
        /// <param name="secondsUsed"></param>
        /// <param name="slot"></param>
        /// <param name="byEntity"></param>
        /// <param name="blockSel"></param>
        /// <param name="entitySel"></param>
        public void TryFireStone(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
        {
            bool preventDefault = false;

            foreach (CollectibleBehavior behavior in CollectibleBehaviors)
            {
                EnumHandling handled = EnumHandling.PassThrough;

                behavior.OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel, ref handled);
                if (handled != EnumHandling.PassThrough) preventDefault = true;

                if (handled == EnumHandling.PreventSubsequent) return;
            }

            if (preventDefault) return;

            if (byEntity.Attributes.GetInt("aimingCancel") == 1) return;

            byEntity.Attributes.SetInt("aiming", 0);
            //byEntity.StopAnimation("aim");

            //if (secondsUsed < ChargingTime) return;

            float damage = 4; // in the case of stone-throwing, this should be material dependent but let's leave it static for now

            ItemStack stack = slot.TakeOut(1);
            slot.MarkDirty();

            IPlayer byPlayer = null;
            if (byEntity is EntityPlayer) byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
            byEntity.World.PlaySoundAt(new AssetLocation("sounds/player/throw"), byEntity, byPlayer, false, 8);

            EntityProperties type = byEntity.World.GetEntityType(new AssetLocation("thrownstone-" + Variant["rock"]));
            Entity entityammo = byEntity.World.ClassRegistry.CreateEntity(type);
            ((EntityThrownStone)entityammo).FiredBy = byEntity;
            ((EntityThrownStone)entityammo).Damage = damage;
            ((EntityThrownStone)entityammo).ProjectileStack = stack;


            float acc = (1 - byEntity.Attributes.GetFloat("aimingAccuracy", 0));
            double rndpitch = byEntity.WatchedAttributes.GetDouble("aimingRandPitch", 1) * acc * LauncherAccModifier;
            double rndyaw = byEntity.WatchedAttributes.GetDouble("aimingRandYaw", 1) * acc * LauncherAccModifier;

            Vec3d pos = byEntity.ServerPos.XYZ.Add(0, byEntity.LocalEyePos.Y, 0);
            Vec3d aheadPos = pos.AheadCopy(1, byEntity.ServerPos.Pitch + rndpitch, byEntity.ServerPos.Yaw + rndyaw);
            Vec3d velocity = (aheadPos - pos) * 0.5;

            //entity.ServerPos.SetPosWithDimension(
            entityammo.ServerPos.SetPos(
                byEntity.ServerPos.BehindCopy(0.21).XYZ.Add(0, byEntity.LocalEyePos.Y, 0)
            );

            entityammo.ServerPos.Motion.Set(velocity);

            entityammo.Pos.SetFrom(entityammo.ServerPos);
            entityammo.World = byEntity.World;

            byEntity.World.SpawnEntity(entityammo);
            //byEntity.StartAnimation("throw");
        }
        /// <summary>
        /// Try to fire a rocket from the launcher. Requires a pass-through of all OnHeldInteractStop arguments, since this method is just a custom implementation of ItemBow's firing.
        /// </summary>
        /// <param name="secondsUsed"></param>
        /// <param name="slot"></param>
        /// <param name="byEntity"></param>
        /// <param name="blockSel"></param>
        /// <param name="entitySel"></param>
        public void TryFireRocket(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
        {
            ItemStack stack = slot.TakeOut(1);
            slot.MarkDirty();

            //float breakChance = 0.5f;
            //if (stack.ItemAttributes != null) breakChance = stack.ItemAttributes["breakChanceOnImpact"].AsFloat(0.5f);

            EntityProperties type = byEntity.World.GetEntityType(new AssetLocation("arrow-" + stack.Collectible.Variant["material"]));
            var entityammo = byEntity.World.ClassRegistry.CreateEntity(type) as EntityProjectileExploding;
            entityammo.FiredBy = byEntity;
            entityammo.Damage = FlatDamageModifier;
            entityammo.ProjectileStack = stack;
            /*
            if (RetrieveLauncherType() == "rocket") // rocket-specific logic
            {
                entityammo.DropOnImpactChance = 0;
            }
            else
            {
                entityammo.DropOnImpactChance = 1 - breakChance;
            }
            */

            float acc = Math.Max(0.001f, 1 - byEntity.Attributes.GetFloat("aimingAccuracy", 0));
            double rndpitch = byEntity.WatchedAttributes.GetDouble("aimingRandPitch", 1) * acc * LauncherAccModifier; // want character accuracy to be taken into account but not as much as a bow does
            double rndyaw = byEntity.WatchedAttributes.GetDouble("aimingRandYaw", 1) * acc * LauncherAccModifier;

            Vec3d pos = byEntity.ServerPos.XYZ.Add(0, byEntity.LocalEyePos.Y - 0.2, 0);
            Vec3d aheadPos = pos.AheadCopy(1, byEntity.SidedPos.Pitch + rndpitch, byEntity.SidedPos.Yaw + rndyaw);
            // not sure how to interpret the velocity vector; is it the difference between the look-direction vector and the firing position?
            // placeholder multiplier until tested
            Vec3d velocity = (aheadPos - pos) * 0.5; //byEntity.Stats.GetBlended("bowDrawingStrength");

            // setpos with dim is probably 1.20 specific, so leaving it here
            //entityarrow.ServerPos.SetPosWithDimension(byEntity.SidedPos.BehindCopy(0.21).XYZ.Add(0, byEntity.LocalEyePos.Y, 0));
            entityammo.ServerPos.SetPos(byEntity.SidedPos.BehindCopy(0.21).XYZ.Add(0, byEntity.LocalEyePos.Y - 0.2, 0));
            entityammo.ServerPos.Motion.Set(velocity);
            entityammo.Pos.SetFrom(entityammo.ServerPos);
            entityammo.World = byEntity.World;
            entityammo.SetRotation();

            byEntity.World.SpawnEntity(entityammo);
        }

        public void TryLoadRocket(EntityAgent byEntity, ItemSlot slot)
        {
            if (RocketLoaded && LoadedRocketType != null) return;

            if (byEntity.World is IClientWorldAccessor)
            {
                slot.Itemstack.TempAttributes.RemoveAttribute("renderVariant");
            }

            // the problem with doing this is that we can't add more cases through json
            // also can't use an enum because the end-result is the same
            // the logical solution would be something using attributes from cannonrocket json data attributes, probably?
            switch (LoadedRocketType)
            {
                case "basic":
                    break;
                
            }
                

            slot.Itemstack.Attributes.SetInt("renderVariant", 0);
            (byEntity as EntityPlayer)?.Player?.InventoryManager.BroadcastHotbarSlot();

            // useless old rubbish, delete later
            /*ItemSlot itemSlot = byEntity.ActiveHandItemSlot;
            ItemStack itemStack = itemSlot.Itemstack;

            CompositeShape _loadedRocketShape = itemStack.Item.Shape.Clone();

            _loadedRocketShape.

            itemStack.Item.Shape = _loadedRocketShape;*/
        }

        /*
        private void ProvideErrorFeedback(string errorcode, string feedbackmsg) // should probably make this part of an abstract class
        {
            // for providing client-side feedback
            if (api.Side == EnumAppSide.Client)
            {
                (api as ICoreClientAPI).TriggerIngameError(this, errorcode, Lang.Get(feedbackmsg));
            }
        }
        */
    }
}
