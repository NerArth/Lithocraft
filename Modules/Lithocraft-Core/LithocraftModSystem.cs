using Lithocraft.BlockEntities;
using Lithocraft.Blocks;
using Lithocraft.Items;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
/*
[
    assembly: ModInfo(
        "lithocraft",
        Authors = new string[] { "NerArth" },
        Description = "From a stone you may draw its blood after all...",
        Version = "0.3.0"
    )
]
*/

namespace Lithocraft
{
    public class LithocraftModSystem : ModSystem
    {
        public override void StartPre(ICoreAPI api)
        {

        }

        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            LogPassthrough.LithocraftModReference = Mod;
            string _shortid = Mod.Info.ModID;
            base.Start(api); // self-memo: read the base.Start tooltip description

            // remember to set commented classes to compile again when wanting to work on them

            //Mod.Logger.Notification("Hello from template mod: " + api.Side);
            Mod.Logger.Event(Mod.Info.Name + " " + Mod.Info.Version + " is registering code classes... ");
            Mod.Logger.StoryEvent("Drawing blood from stones... ");
            //api.RegisterBlockBehaviorClass(Mod.Info.ModID + ".behaviordebug", typeof(BlockBehaviorHoriAttachDebug));

            //api.RegisterEntity(_shortid + ".projectileexploding", typeof(EntityProjectileExploding));

            // register item classes
            api.RegisterItemClass(_shortid + ".claycutter",typeof(ItemClaycutter));
            api.RegisterItemClass(_shortid + ".whetstone", typeof(ItemWhetstone));
            //api.RegisterItemClass(_shortid + ".chemicalresidue", typeof(ItemChemicalResidue));

            //api.RegisterItemClass(_shortid + ".handcannon", typeof(ItemHandCannon));
            //api.RegisterItemClass(_shortid + ".cannonrocket", typeof(ItemCannonRocket));

            // register block/BE paired classes
            api.RegisterBlockClass(_shortid + ".grindstone", typeof(BlockGrindstone));
            api.RegisterBlockEntityClass(_shortid + ".BEgrindstone", typeof(BlockEntityGrindstone));

            //api.RegisterBlockEntityClass(_shortid + ".BEtoolmold", typeof(BlockEntityToolMoldCustom));

            //api.RegisterBlockClass(_shortid + ".vat", typeof(BlockVat));


        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            Mod.Logger.Event(Mod.Info.Name + " " + Mod.Info.Version + " is starting (server)...");
            //Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("lithocraft:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            Mod.Logger.Event(Mod.Info.Name + " " + Mod.Info.Version + " is starting (client)...");
            Mod.Logger.StoryEvent("Coalescing elemental matter... ");
            //Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("lithocraft:hello"));

            //foreach (GuiDialogHandbook dialog in api.Gui.LoadedGuis)
            //{
            //    //dialog.on

            //    //if (page.PageCode == )
            //}
        }
    }

    internal static class LogPassthrough
    {
        public static Mod? LithocraftModReference { get; internal set; }
    }
    public class BlockEntityOveny : BlockEntityOven
    {

    }
}
