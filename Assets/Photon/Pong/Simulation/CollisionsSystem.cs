using Photon.Deterministic;

namespace Quantum.Pong
{
    public unsafe class CollisionsSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D, ISignalOnCollision2D
    {
        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (f.Unsafe.TryGetPointer<Paddle>(info.Entity, out var paddle))
            {
                if (f.Unsafe.TryGetPointer<Ball>(info.Other, out var ball))
                {
                    HandleBallHitPaddle(f, ball);
                }
            }

            if (f.Unsafe.TryGetPointer<GoalLine>(info.Entity, out var goalLine))
            {
                if (f.Unsafe.TryGetPointer<Ball>(info.Other, out var ball))
                {
                    if (goalLine->isOnLeft)
                    {
                        f.Global->Team2Score++;
                    }
                    else
                    {
                        f.Global->Team1Score++;
                    }

                    f.Destroy(info.Other);
                }
            }
        }

        public void OnCollision2D(Frame f, CollisionInfo2D info)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);

            if (f.Has<Ball>(info.Entity) && f.Unsafe.TryGetPointer<PhysicsBody2D>(info.Entity, out var physicsBody))
            {
                var vel = physicsBody->Velocity;
                vel = vel.Normalized * FPMath.Max(config.BallBaseSpeed, vel.Magnitude * FP._1_10);
                vel = physicsBody->Velocity;

                // vel.X should be greater than vel.Y or the ball can stuck
                vel.X = FPMath.Sign(vel.X) * FPMath.Max(FPMath.Abs(vel.X), FPMath.Abs(vel.Y));
                physicsBody->Velocity = vel;

            }
        }

        private void HandleBallHitPaddle(Frame f, Ball* ball)
        {
            //ball->Velocity.X *= -1;
        }
    }
}