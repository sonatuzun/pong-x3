using Photon.Deterministic;
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
            AssetRef<EntityPrototype> paddle = f.RuntimeConfig.DefaultPaddle;
            EntityRef playerRef = f.Create(playerPrototype);

            if (f.Unsafe.TryGetPointer<PlayerInfo>(playerRef, out var playerInfo))
            {
                if (playerInfo->IsSingleKeyboardMultiplayer)
                {
                    ControlFlags flags1;
                    flags1.BotControlled = false;
                    flags1.AcceptInputForP1 = true;
                    flags1.AcceptInputForP2 = false;

                    SpawnPaddle(f, paddle, flags1, player);

                    ControlFlags flags2;
                    flags2.BotControlled = true;
                    flags2.AcceptInputForP1 = false;
                    flags2.AcceptInputForP2 = true;

                    SpawnPaddle(f, paddle, flags2, player);
                }
                else
                {
                    ControlFlags flags;
                    flags.BotControlled = false;
                    flags.AcceptInputForP1 = true;
                    flags.AcceptInputForP2 = true;

                    SpawnPaddle(f, paddle, flags, player);
                }
            }
        }

        public void SpawnPaddle(Frame f, AssetRef<EntityPrototype> paddleAsset, ControlFlags flags, PlayerRef? player = null)
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

            // Set control flags for the paddle 
            f.Set(paddleRef, flags);

            // Set player link component to mark this entity as player controller
            if (player.HasValue)
            {
                f.Set(paddleRef, new PlayerLink { PlayerRef = player.Value });
            }

            f.Global->PaddleCount++;
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