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

namespace Lithocraft.CodeContent
{
    internal class BlockHoriAttachBehaviorDebug : BlockBehaviorHorizontalAttachable
    {
        bool handleDrops = true;
        string dropBlockFace = "north";
        string dropBlock = null;
        Dictionary<string, Cuboidi> attachmentAreas;

        public BlockHoriAttachBehaviorDebug(Block block) : base(block)
        {
        }

        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
        {
            handling = EnumHandling.PreventDefault;

            // Prefer selected block face
            if (blockSel.Face.IsHorizontal)
            {
                if (TryAttachTo(world, byPlayer, blockSel, itemstack, ref failureCode)) return true;
            }

            // Otherwise attach to any possible face
            BlockFacing[] faces = BlockFacing.HORIZONTALS;
            blockSel = blockSel.Clone();
            for (int i = 0; i < faces.Length; i++)
            {
                blockSel.Face = faces[i];
                if (TryAttachTo(world, byPlayer, blockSel, itemstack, ref failureCode)) return true;
            }

            failureCode = "requirehorizontalattachable";

            return false;
        }

        bool TryAttachTo(IWorldAccessor world, IPlayer player, BlockSelection blockSel, ItemStack itemstack, ref string failureCode)
        {
            BlockFacing oppositeFace = blockSel.Face.Opposite;

            BlockPos attachingBlockPos = blockSel.Position.AddCopy(oppositeFace);
            Block attachingBlock = world.BlockAccessor.GetBlock(attachingBlockPos);
            Block orientedBlock = world.BlockAccessor.GetBlock(block.CodeWithParts(oppositeFace.Code));

            Cuboidi attachmentArea = null;
            attachmentAreas?.TryGetValue(oppositeFace.Code, out attachmentArea);

            if (attachingBlock.CanAttachBlockAt(world.BlockAccessor, block, attachingBlockPos, blockSel.Face, attachmentArea) && orientedBlock.CanPlaceBlock(world, player, blockSel, ref failureCode))
            {
                orientedBlock.DoPlaceBlock(world, player, blockSel, itemstack);
                return true;
            }

            return false;
        }

    }
}
