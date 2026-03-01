using Photon.Deterministic;
using System;

namespace Quantum.Pong
{

    /// <summary>
    /// The <c>AsteroidsPlayerSystem</c> class handles the addition of new players to the game,
    /// including creating and spawning their ship entities.
    /// </summary>
    public unsafe class PlayerSystem : SystemSignalsOnly, ISignalOnPlayerAdded, ISignalOnPlayerDisconnected
    {
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

            // Create a ship entity from the provided prototype or the default prototype from the RuntimeConfig
            var playerAvatarAssetRef = data.PlayerAvatar.IsValid ? data.PlayerAvatar : f.RuntimeConfig.DefaultPlayerAvatar;
            EntityPrototype paddlePrototype = f.FindAsset(playerAvatarAssetRef);
            EntityRef paddleRef = f.Create(paddlePrototype);

            Transform2D* transform = f.Unsafe.GetPointer<Transform2D>(paddleRef);
            Int32 playerCount = f.Global->PlayerCount;
            bool isOnLeft = playerCount % 2 == 0;
            transform->Position = new FPVector2(isOnLeft ? FP.FromString("-25") : FP.FromString("25"), 0);

            if (f.Unsafe.TryGetPointer<Paddle>(paddleRef, out var paddle))
            {
                paddle->BaseX = transform->Position.X;
            }

            // Set player link component to mark this entity as player controller
            f.Set(paddleRef, new PlayerLink { PlayerRef = player });

            f.Global->PlayerCount++;
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