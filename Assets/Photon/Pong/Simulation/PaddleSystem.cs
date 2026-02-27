namespace Quantum.Pong
{
    using Photon.Deterministic;

    /// <summary>
    /// The <c>AsteroidsShipSystem</c> class manages the behavior of player-controlled ships,
    /// including movement, firing, and handling destruction and respawn.
    /// </summary>
    public unsafe class PaddleSystem : SystemMainThreadFilter<PaddleSystem.Filter>, ISignalSpawnPaddle
    {
        /// <summary>
        /// The <c>Filter</c> struct represents the components required for the system's operations,
        /// including an entity reference, transform, physics body, ship component, and player link component.
        /// </summary>
        public struct Filter
        {
            /// <summary>
            /// The reference to the entity being processed.
            /// </summary>
            public EntityRef Entity;

            /// <summary>
            /// Pointer to the entity's transform component.
            /// </summary>
            public Transform2D* Transform;

            /// <summary>
            /// Pointer to the entity's ship component.
            /// </summary>
            public Paddle* Paddle;

            /// <summary>
            /// Pointer to the entity's player link component.
            /// </summary>
            public PlayerLink* PlayerLink;
        }

        /// <summary>
        /// Updates the ship movement and firing, as well as respawning if needed.
        /// </summary>
        /// <param name="f">The game frame.</param>
        /// <param name="filter">The filter containing the entity and its components.</param>
        public override void Update(Frame f, ref Filter filter)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
            Input* input = f.GetPlayerInput(filter.PlayerLink->PlayerRef);

            UpdatePaddleMovement(f, ref filter, input, config);

        }

        /// <summary>
        /// Updates the movement state of the ship, handling thrust and rotation based on player input.
        /// </summary>
        /// <param name="f">The game frame.</param>
        /// <param name="filter">The filter containing the entity and its components.</param>
        /// <param name="input">The input state of the player.</param>
        /// <param name="config">The game configuration settings.</param>
        private void UpdatePaddleMovement(Frame f, ref Filter filter, Input* input, PongGameConfig config)
        {
            //FP turnSpeed = config.ShipTurnSpeed;
            if (input->Up)
            {
                filter.Transform->Position += FPVector2.Up;
            }

            if (input->Down)
            {
                filter.Transform->Position += FPVector2.Down;
            }
        }

        // Copied from MapSystem RIP
        /// <summary>
        /// Updates the state of the system by checking if entities are out of bounds and wrapping them if necessary.
        /// </summary>
        /// <param name="f">The game frame.</param>
        /// <param name="filter">The filter containing the entity and its transform.</param>
        public void LimitPaddleMovement(Frame f, ref Filter filter)
        {
            //AsteroidsGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);

            //if (config.IsOutOfBounds(filter.Transform->Position, out FPVector2 newPosition))
            //{
            //    filter.Transform->Teleport(f, newPosition);
            //}
        }

        public void SpawnPaddle(Frame f, EntityRef paddle)
        {
            throw new System.NotImplementedException();
        }
    }
}