using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace Lithocraft.CodeContent
{
    internal class ItemChemicalResidue : Item
    {
        /*
        public override void DoSmelt(IWorldAccessor world, ISlotProvider cookingSlotsProvider, ItemSlot inputSlot, ItemSlot outputSlot)
        {
            if (inputSlot != null && inputSlot.Itemstack.Collectible.LastCodePart() == "aluox") // special case for ruining aluox when attempting alumina conversion; needs more in-depth testing before attempting to implement
            {
                var _out = outputSlot;

                if (base.GetTemperature(world, inputSlot.Itemstack) > 660)
                {
                    _out.Itemstack = null;
                    base.DoSmelt(world, cookingSlotsProvider, inputSlot, _out);
                }
                else
                {
                    base.DoSmelt(world, cookingSlotsProvider, inputSlot, outputSlot);
                }
            }
            else
            {
                base.DoSmelt(world, cookingSlotsProvider, inputSlot, outputSlot);
            }
        }
        */
    }
}
