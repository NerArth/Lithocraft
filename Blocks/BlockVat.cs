using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace Lithocraft.CodeContent
{
    public class BlockVat : BlockBarrel
    {
        // hopefully this works; should just override the shape and nothing else, keeping any existing vanilla function intact
        static protected new string shapesBasePath => "shapes/block/stone/vat/";

        public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        {
            base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
        }
    }
}
