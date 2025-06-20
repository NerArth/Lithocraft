using Lithocraft;
using Lithocraft.Items;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace Lithocraft.Chem
{
    public class LithocraftChemModSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            //base.Start(api);
        }
        public override void StartServerSide(ICoreServerAPI api)
        {
            Mod.Logger.Event(Mod.Info.Name + " " + Mod.Info.Version + " is starting (server)...");
            //Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("lithocraft:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            Mod.Logger.Event(Mod.Info.Name + " " + Mod.Info.Version + " is starting (client)...");
            Mod.Logger.StoryEvent("Creating interactions... ");
            //Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("lithocraft:hello"));
        }
    }
}
