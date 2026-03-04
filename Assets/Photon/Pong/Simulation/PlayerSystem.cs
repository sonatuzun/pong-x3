using Photon.Deterministic;
using Photon.Deterministic.Protocol;
using Quantum.Prototypes;
using System;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace Quantum.Pong
{

    /// <summary>
    /// The <c>AsteroidsPlayerSystem</c> class handles the addition of new players to the game,
    /// including creating and spawning their ship entities.
    /// </summary>
    public unsafe class PlayerSystem : SystemSignalsOnly, ISignalOnPlayerAdded, ISignalOnPlayerDisconnected
    {
        public AssetRef<EntityPrototype> paddlePrototype;

        /// <summary>
        /// Called when a new player is added to the game. This method creates a paddle entity for the player,
        /// sets up the player link component, and triggers the ship spawn signal.
        /// </summary>
        /// <param name="f">The game frame.</param>
        /// <param name="player">The reference to the player being added.</param>
        /// <param name="firstTime">Indicates if this is the first time the player is added.</param>
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            RuntimePlayer data = f.GetPlayerData(player);

            AssetRef<EntityPrototype> playerPrototype = data.PlayerAvatar.IsValid ? data.PlayerAvatar : f.RuntimeConfig.DefaultPlayerInfo;
            AssetRef<EntityPrototype> paddlePrototype = f.RuntimeConfig.DefaultPaddle;
            EntityRef playerRef = f.Create(playerPrototype);

            {
                ControlFlags flags;
                flags.AcceptInputForP1 = true;
                flags.AcceptInputForP2 = f.RuntimeConfig.LocalPlayerCount <= 1;

                var paddle = SpawnPaddle(f, paddlePrototype);
                AddPlayer(f, paddle, player, flags);
            }

            if (f.RuntimeConfig.BotCount > 0)
            {
                BotInfo botInfo;
                botInfo.BotIndex = 0;
                var botPaddle = SpawnPaddle(f, paddlePrototype);
                AddBot(f, botPaddle, botInfo);
            }

            if (f.RuntimeConfig.LocalPlayerCount > 1)
            {
                ControlFlags flags;
                flags.AcceptInputForP1 = false;
                flags.AcceptInputForP2 = true;

                var paddle = SpawnPaddle(f, paddlePrototype);
                AddPlayer(f, paddle, player, flags);
            }

            if (f.RuntimeConfig.BotCount > 1)
            {
                BotInfo botInfo;
                botInfo.BotIndex = 1;
                var botPaddle = SpawnPaddle(f, paddlePrototype);
                AddBot(f, botPaddle, botInfo);
            }
        }

        public EntityRef SpawnPaddle(Frame f, AssetRef<EntityPrototype> paddleAsset)
        {
            // Create a paddle entity from the provided prototype or the default prototype from the RuntimeConfig
            EntityPrototype paddlePrototype = f.FindAsset(paddleAsset);
            EntityRef paddleRef = f.Create(paddlePrototype);
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);

            Transform2D* transform = f.Unsafe.GetPointer<Transform2D>(paddleRef);
            Int32 playerCount = f.Global->PaddleCount;
            bool isOnLeft = playerCount % 2 == 0;
            transform->Position = new FPVector2(isOnLeft ? -config.PaddleDistanceToCenter : config.PaddleDistanceToCenter, 0);

            if (f.Unsafe.TryGetPointer<Paddle>(paddleRef, out var paddle))
            {
                paddle->BaseX = transform->Position.X;
            }



            f.Global->PaddleCount++;

            return paddleRef;
        }

        public void AddPlayer(Frame f, EntityRef paddleRef, PlayerRef player, ControlFlags flags)
        {
            f.Set(paddleRef, new PlayerLink { PlayerRef = player });
            f.Set(paddleRef, flags);
        }

        public void AddBot(Frame f, EntityRef paddleRef, BotInfo botInfo)
        {
            f.Set(paddleRef, botInfo);
        }

        /// <summary>
        /// Called when a new player is disconnected. This callback handles the ship destruction.
        /// </summary>
        /// <param name="f">The game frame.</param>
        /// <param name="player">The reference to the player being desconnected.</param>
        public void OnPlayerDisconnected(Frame f, PlayerRef player)
        {
            foreach (var pair in f.GetComponentIterator<PlayerLink>())
            {
                if (pair.Component.PlayerRef == player)
                {
                    f.Destroy(pair.Entity);
                }
            }
        }
    }
}