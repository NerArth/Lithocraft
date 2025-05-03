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
    internal class BlockEntityToolMoldCustom : BlockEntityToolMold
    {
        private ToolMoldRenderer renderer;
        private int requiredUnits = 100;
        private float fillHeight = 1f;

        internal void UpdateRenderer() // new keyword not required because base class method is inaccessible *anyway*, so it's impossible to call or refer to the original base class method when only using inheritance
        {
            if (renderer != null)
            {
                renderer.Level = (float)fillLevel * fillHeight / (float)requiredUnits;
                if (metalContent?.Collectible != null)
                {
                    renderer.TextureName = new AssetLocation("lithocraft:block/metal/ingot/" + metalContent.Collectible.LastCodePart() + ".png");
                }
                else
                {
                    renderer.TextureName = null;
                }
            }
        }
    }
}
