using System;
using System.Collections.Generic;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace ChestPlagin
{
    [ApiVersion(2, 1)]
    public class LobbyModeAlwaysOnPlugin : TerrariaPlugin
    {
        public override string Name => "LobbyModeAlwaysOn";
        public override string Author => "Caxarok.ru@2026";
        public override string Description => "Игроки полностью скрыты друг от друга, мир работает нормально.";
        public override Version Version => new Version(3, 2);

        private readonly HashSet<PacketTypes> blockedPackets = new HashSet<PacketTypes>
        {
            PacketTypes.PlayerUpdate,
            PacketTypes.PlayerInfo,
            PacketTypes.PlayerSpawn,
            PacketTypes.PlayerSpawnSelf,
            PacketTypes.FinishedConnectingToServer,
            PacketTypes.PlayerActive,
            PacketTypes.PlayerHp,
            PacketTypes.PlayerMana,
            PacketTypes.PlayerAnimation,
            PacketTypes.PlayerTeam,
            PacketTypes.PlayerBuff,
            PacketTypes.PlayerAddBuff,
            PacketTypes.PlayerSlot,
            PacketTypes.TogglePvp,
            PacketTypes.Teleport,
            PacketTypes.PlayerTeleportPortal,
            PacketTypes.PlayerHurtV2,
            PacketTypes.PlayerDeathV2,
            PacketTypes.SpectatePlayer,
            PacketTypes.ClientSyncedInventory,
            PacketTypes.SyncLoadout,
            PacketTypes.PlayerPlatformInfo
        };

        public LobbyModeAlwaysOnPlugin(Main game) : base(game) { }

        public override void Initialize()
        {
            ServerApi.Hooks.NetSendData.Register(this, OnSendData);
        }

        private void OnSendData(SendDataEventArgs args)
        {
            // блокируем пакеты между игроками
            if (args.remoteClient != -1 && args.number != args.remoteClient)
            {
                if (blockedPackets.Contains(args.MsgId))
                {
                    args.Handled = true;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetSendData.Deregister(this, OnSendData);
            }
            base.Dispose(disposing);
        }
    }
}