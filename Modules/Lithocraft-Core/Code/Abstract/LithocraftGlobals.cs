using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.Common;

namespace Lithocraft.Code.Abstract
{
    internal class LithocraftGlobals : ModSystem
    {
        public static string[] RepairsValidTools = Array.Empty<string>();

        public override void Start(ICoreAPI api)
        {
            RepairsValidTools = api.Assets.Get<JsonObject>("blocktypes/grindstone.json")["attributes"]["validToolsByType"].AsArray<string>();
            base.Start(api);
        }
    }
}
