using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace Lithocraft.CodeContent
{
    internal class ItemCannonRocket : ItemArrow
    {
        /*
        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {
            base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);

            if (inSlot.Itemstack.Collectible.Attributes == null) return;

            float dmg = inSlot.Itemstack.Collectible.Attributes["damage"].AsFloat(0);
            if (dmg != 0) dsc.AppendLine(Lang.Get("arrow-piercingdamage", (dmg > 0 ? "+" : "") + dmg));

            float breakChanceOnImpact = inSlot.Itemstack.Collectible.Attributes["breakChanceOnImpact"].AsFloat(0.5f);
            dsc.AppendLine(Lang.Get("breakchanceonimpact", (int)(breakChanceOnImpact * 100)));

        }
        */
    }
}
