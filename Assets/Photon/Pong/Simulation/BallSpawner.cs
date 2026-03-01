using Photon.Deterministic;

namespace Quantum.Pong
{

    public unsafe class BallSpawner : SystemSignalsOnly,
      ISignalOnComponentRemoved<Ball>
    {
        public override void OnInit(Frame f)
        {
            SpawnBall(f);
        }

        public void OnRemoved(Frame f, EntityRef entity, Ball* component)
        {
            if (f.ComponentCount<Ball>(false) <= 1)
            {
                PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
                SpawnBall(f);
            }
        }

        public void SpawnBall(Frame f)
        {
            for (int i = 0; i < 1; i++)
            {
                PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
                EntityRef ballRef = f.Create(config.BallPrototype);
                Transform2D* ballTransform = f.Unsafe.GetPointer<Transform2D>(ballRef);
                PhysicsBody2D* body = f.Unsafe.GetPointer<PhysicsBody2D>(ballRef);
                Ball* ball = f.Unsafe.GetPointer<Ball>(ballRef);


                ballTransform->Position = FPVector2.Zero;

                FPVector2 direction = (f.RNG->Next() < FP._0_50) ? FPVector2.Left : FPVector2.Right;
                FP angle = (f.RNG->Next() - FP._0_50) * FP._1;
                FPVector2 velocity = FPVector2.Rotate(direction * config.BallBaseSpeed, angle);

                body->Velocity = velocity;
            }
        }
    }
}