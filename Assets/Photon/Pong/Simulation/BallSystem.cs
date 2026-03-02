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
            public PhysicsCollider2D PhysicsCollider;
            public PhysicsBody2D* PhysicsBody;
            public Ball* Ball;
        }

        private FPVector2 _baseBodyExtents;

        // for velocity calculations of ball after collisions 
        // check out CollisionsSystem.cs

        public override void OnInit(Frame f)
        {
            
        }

        public override void Update(Frame f, ref Filter filter)
        {
            
        }
    }
}