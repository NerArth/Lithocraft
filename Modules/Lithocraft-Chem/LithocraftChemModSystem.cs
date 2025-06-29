using Lithocraft;
using Lithocraft.Items;
using Lithocraft.Chem.Items;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using System;
using System.Collections.Generic;
using Vintagestory.Common;
using System.Runtime.CompilerServices;

namespace Lithocraft.Chem
{
    public class LithocraftChemModSystem : ModSystem
    {
        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);

        }
        public override void Start(ICoreAPI api)
        {
            string _shortid = Mod.Info.ModID;
            base.Start(api);

            Mod.Logger.Event(Mod.Info.Name + " " + Mod.Info.Version + " is registering code classes... ");
            Mod.Logger.StoryEvent("Throwing free radicals...");

            // register item classes
            api.RegisterItemClass(_shortid + ".chemicalresidue", typeof(ItemChemicalResidue));

            //List<CollectibleObject> lithocraftChemItems = new();


            //foreach (var asset in api.ModLoader.Mods)
            //{
            //    lithocraftChemItems.Add(api);
            //}

            //for (int i = 0; i < lithocraftChemItems.Count; i++)
            //{
            //    CollectibleObject item = lithocraftChemItems[i];
            //    if (item == api.World.Collectibles.Find(c => c.Code == item.Code))
            //    {
            //        Mod.Logger.Event("Registered item: " + item.Code);
            //    }
            //}

            //List<CollectibleObject> lithocraftChemItems = api.World.Collectibles.FindAll(api.Assets.GetMany("itemtypes/resource/chemical*", "lithocraft", false));
            //api.Assets.GetMany("itemtypes/resource/chemical*", "lithocraft", false
            //List<CollectibleObject>? lci = api.World.Collectibles.FindAll(c => c.CodeWithPath(lithocraftChemItems[0].FilePath) == "lithocraft");



            //for (int ci = 0; ci < api.World.Collectibles.Count; ci++)
            //{
            //    CollectibleObject? item = api.World.Collectibles[ci];
            //    if (item.Class is "lithocraftchem.chemicalresidue")
            //    {
            //        Mod.Logger.Event("Registered item: " + item.Code);
            //    }
            //}
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
