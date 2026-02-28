using Photon.Deterministic;

namespace Quantum.Pong
{

    /// <summary>
    /// The <c>AsteroidsWaveSpawnerSystem</c> class is responsible for spawning waves of asteroids
    /// and managing the asteroid spawning logic.
    /// </summary>
    public unsafe class BallSpawner : SystemSignalsOnly,
      ISignalOnComponentRemoved<Ball>
    {
        /// <summary>
        /// Initializes the wave spawner system by setting the initial wave count and spawning the first wave of asteroids.
        /// </summary>
        /// <param name="f">The game frame.</param>
        public override void OnInit(Frame f)
        {
            SpawnBall(f);
        }

        /// <summary>
        /// Handles the removal of an asteroid component. If there are no asteroids remaining,
        /// spawns a new wave of asteroids.
        /// </summary>
        /// <param name="f">The game frame.</param>
        /// <param name="entity">The entity from which the asteroid component was removed.</param>
        /// <param name="component">The removed asteroid component.</param>
        public void OnRemoved(Frame f, EntityRef entity, Ball* component)
        {
            if (f.ComponentCount<Ball>(false) <= 1)
            {
                PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
                SpawnBall(f);
            }
        }

        /// <summary>
        /// Spawns an asteroid entity at a random position on the edge of the game area or near a specified parent entity.
        /// </summary>
        /// <param name="f">The game frame.</param>
        /// <param name="childPrototype">The prototype of the asteroid entity to spawn.</param>
        /// <param name="parent">The parent entity near which to spawn the asteroid (or EntityRef.None for random edge spawn).</param>
        public void SpawnBall(Frame f)
        {
            for (int i = 0; i < 1; i++)
            {
                PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
                EntityRef ballRef = f.Create(config.BallPrototype);
                Transform2D* ballTransform = f.Unsafe.GetPointer<Transform2D>(ballRef);
                PhysicsBody2D* body = f.Unsafe.GetPointer<PhysicsBody2D>(ballRef);
                Ball* ball = f.Unsafe.GetPointer<Ball>(ballRef);


                ballTransform->Position = PUtils.GetRandomEdgePointOnCircle(f, 0);
                //ball->Velocity = PUtils.GetRandomEdgePointOnCircle(f, config.BallBaseSpeed);
                body->Velocity = PUtils.GetRandomEdgePointOnCircle(f, config.BallBaseSpeed);
            }
        }
    }
}