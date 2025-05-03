using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace Lithocraft.CodeContent
{
    public class EntityProjectileExploding : EntityProjectile
    {
        #region inheritedvars
        protected bool beforeCollided;
        protected bool stuck;
        protected long msLaunch;
        protected long msCollide;
        protected Vec3d motionBeforeCollide = new Vec3d();
        protected CollisionTester collTester = new CollisionTester();
        protected Cuboidf collisionTestBox;
        protected EntityPartitioning ep;

        public new float Weight = 5.0f;
        public new float Damage;
        #endregion

        #region classvars
        protected float ExplosionRadius = 1.0f; // damage radius should 3/4 of the of the full radius; this should be retrieved from the item attributes, to make it easy to add different potency variants
        //protected float DamageRadius;
        #endregion

        // requires custom implementation for damage handling
        protected virtual void impactOnEntity(Entity impactedEntity)
        {
            if (!Alive) return;

            EntityPos pos = SidedPos;

            IServerPlayer FiringPlayer = null;
            if (FiredBy is EntityPlayer) { FiringPlayer = (FiringPlayer as EntityPlayer).Player as IServerPlayer; }

            bool targetIsPlayer = impactedEntity is EntityPlayer;
            bool targetIsCreature = impactedEntity is EntityAgent;
            bool canDamage = true;

            ICoreServerAPI sapi = World.Api as ICoreServerAPI;
            if (FiringPlayer != null)
            {
                if (targetIsPlayer && (!sapi.Server.Config.AllowPvP || !FiringPlayer.HasPrivilege("attackplayers"))) canDamage = false;
                if (targetIsCreature && !FiringPlayer.HasPrivilege("attackcreatures")) canDamage = false;
            }

            msCollide = World.ElapsedMilliseconds;

            pos.Motion.Set(0, 0, 0);

            if (canDamage && World.Side == EnumAppSide.Server)
            {
                World.PlaySoundAt(new AssetLocation("sounds/arrow-impact"), this, null, false, 24);

                float dmg = Damage;
                if (FiredBy != null) dmg *= FiredBy.Stats.GetBlended("rangedWeaponsDamage");

                bool didDamage = impactedEntity.ReceiveDamage(new DamageSource()
                {
                    Source = FiringPlayer != null ? EnumDamageSource.Player : EnumDamageSource.Entity,
                    SourceEntity = this,
                    CauseEntity = FiredBy,
                    Type = EnumDamageType.BluntAttack
                }, dmg);

                float kbresist = impactedEntity.Properties.KnockbackResistance;
                impactedEntity.SidedPos.Motion.Add(kbresist * pos.Motion.X * Weight, kbresist * pos.Motion.Y * Weight, kbresist * pos.Motion.Z * Weight);

                int leftDurability = 1;
                if (DamageStackOnImpact)
                {
                    ProjectileStack.Collectible.DamageItem(impactedEntity.World, impactedEntity, new DummySlot(ProjectileStack));
                    leftDurability = ProjectileStack == null ? 1 : ProjectileStack.Collectible.GetRemainingDurability(ProjectileStack);
                }

                if (World.Rand.NextDouble() < DropOnImpactChance && leftDurability > 0)
                {

                }
                else
                {
                    Die();
                }

                if (FiredBy is EntityPlayer && didDamage)
                {
                    World.PlaySoundFor(new AssetLocation("sounds/player/projectilehit"), (FiredBy as EntityPlayer).Player, false, 24);
                }
            }
        }

        //((IServerWorldAccessor)Api.World).CreateExplosion(Pos, BlastType, BlastRadius, InjureRadius);

        public override bool CanCollect(Entity byEntity)
        {
            return false;
        }

        protected bool IsClaimedLand()
        {
            int rad = (int)Math.Ceiling(ExplosionRadius);
            Cuboidi exploArea = new Cuboidi(Pos.AsBlockPos.AddCopy(-rad, -rad, -rad), Pos.AsBlockPos.AddCopy(rad, rad, rad));
            List<LandClaim> claims = (Api as ICoreServerAPI).WorldManager.SaveGame.LandClaims;
            for (int i = 0; i < claims.Count; i++)
            {
                if (claims[i].Intersects(exploArea)) return true;
            }
            return false;
        }
    }
}
