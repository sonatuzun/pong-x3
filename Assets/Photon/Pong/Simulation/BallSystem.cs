namespace Quantum.Pong
{
    using Photon.Deterministic;

    /// <summary>
    /// The <c>AsteroidsProjectileSystem</c> class manages the lifecycle of projectiles,
    /// including updating their time-to-live (TTL) and handling projectile shooting signals.
    /// </summary>
    public unsafe class BallSystem : SystemMainThreadFilter<BallSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public PhysicsBody2D* PhysicsBody;
            public Ball* Ball;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            //filter.Transform->Position += filter.Ball->Velocity;
            //HandleBorderCollisions(f, ref filter);
        }

        public void HandleBorderCollisions(Frame f, ref Filter filter)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
            FP extends = config.GameMapSize.Y / 2;
            
            if (FPMath.Abs(filter.Transform->Position.Y) > extends)
            {
                filter.Ball->Velocity.Y *= -1;
            }
        }
    }
}