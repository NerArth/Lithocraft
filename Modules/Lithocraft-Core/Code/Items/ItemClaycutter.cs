using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Lithocraft.CodeContent
{
    internal class ItemClaycutter : Item // mostly copied the ItemGem class
    {
        public override TransitionState[] UpdateAndGetTransitionStates(IWorldAccessor world, ItemSlot inslot)
        {
            // probably don't need this but actually kind of useful if it sets the default attribute,
            // as that doesn't happen otherwise, and it really should have a default value because of commands like /giveitem
            if (!inslot.Itemstack.Attributes.HasAttribute("readyToUse"))
            {
                inslot.Itemstack.Attributes.SetBool("readyToUse", false);
            }

            return base.UpdateAndGetTransitionStates(world, inslot);
        }
        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {
            // don't know why but this has to be at the start of the function
            // the ItemGem class I had originally copied had it at the end but it didn't seem to work
            base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo); 

            if (inSlot.Itemstack.Attributes != null)
            {
                string ready = inSlot.Itemstack.Attributes.GetBool("readyToUse", false).ToString().ToLowerInvariant();

                dsc.AppendLine( Lang.Get("lithocraft:claycutter-ready-" + ready) ); // the namespace HAS to be included for the game to find the correct translation key
            }
        }
    }
}
