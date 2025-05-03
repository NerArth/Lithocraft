using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace Lithocraft.CodeContent
{
    internal class BEVat : BlockEntityBarrel
    {
        private GuiDialogBarrel invDialog;
        protected new void toggleInventoryDialogClient(IPlayer byPlayer)
        {
            if (invDialog == null)
            {
                ICoreClientAPI capi = Api as ICoreClientAPI;
                invDialog = new GuiDialogBarrel(Lang.Get("Barrel"), Inventory, Pos, Api as ICoreClientAPI);
                invDialog.OnClosed += delegate
                {
                    invDialog = null;
                    capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, 1001);
                    capi.Network.SendPacketClient(Inventory.Close(byPlayer));
                };
                invDialog.OpenSound = AssetLocation.Create("game:sounds/block/barrelopen", base.Block.Code.Domain);
                invDialog.CloseSound = AssetLocation.Create("game:sounds/block/barrelclose", base.Block.Code.Domain);
                invDialog.TryOpen();
                capi.Network.SendPacketClient(Inventory.Open(byPlayer));
                capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, 1000);
            }
            else
            {
                invDialog.TryClose();
            }
        }
    }
}
