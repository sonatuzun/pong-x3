namespace Quantum.Pong
{
    using Photon.Deterministic;
    using UnityEngine;

    public class PongGameConfig : AssetObject
    {
        [Header("Game Configuration")]
        public FP ScoreLimit = 3;

        [Header("Paddle Configuration")]
        [Tooltip("Base speed for the paddle")]
        public FP PaddleBaseSpeed;
        // Following settings do not affect the paddle yet 
        // They are here to use for calculations
        [Tooltip("Base size for the paddle.")]
        public FP PaddleBaseSize = 10;
        public FP PaddleDistanceToCenter = 30;
        public FP PaddleChargeDistance = 8;
        public FP PaddleChargeForce = 10;


        [Header("Ball configuration")]
        [Tooltip("Prototype reference to spawn ball")]
        public AssetRef<EntityPrototype> BallPrototype;
        [Tooltip("Base speed for the ball")]
        public FP BallBaseSpeed = 15;
        [Tooltip("Maximum speed for the ball")]
        public FP BallMaxSpeed = 100;
        [Tooltip("Speed increment for each paddle hit.")]
        public FP BallSpeedIncrement = 5;
        [Tooltip("Minimum horizontal ball speed. Prevents the ball from being stuck.")]
        public FP BallMinHorizontalSpeed = 5;

        [Header("Ball Paddle Interaction")]
        [Tooltip("The rate at vertical paddle movement influences the ball.")]
        public FP VelocityTransferRate = FP._0_50;

        [Header("Map configuration")]
        [Tooltip("Total size of the map. This is used to calculate when an entity is outside de gameplay area and then wrap it to the other side")]
        public FPVector2 GameMapSize = new FPVector2(25, 25);

        /// <summary>
        /// Called when the asset is loaded. Initializes the map extents based on the game map size.
        /// </summary>
        /// <param name="resourceManager">The resource manager used to load assets.</param>
        /// <param name="allocator">The memory allocator used during loading.</param>
        public override void Loaded(IResourceManager resourceManager, Native.Allocator allocator)
        {
            base.Loaded(resourceManager, allocator);
        }
    }
}