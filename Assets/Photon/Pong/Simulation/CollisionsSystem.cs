using Photon.Deterministic;
using UnityEngine.LowLevelPhysics2D;

namespace Quantum.Pong
{
    public unsafe class CollisionsSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D, ISignalOnCollisionExit2D
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

        public void OnCollisionExit2D(Frame f, ExitInfo2D info)
        {
            if (f.Unsafe.TryGetPointer<Ball>(info.Entity, out var ball) && f.Unsafe.TryGetPointer<PhysicsBody2D>(info.Entity, out var physicsBody))
            {
                HandleBallCollision(f, ball, physicsBody);
            }
        }

        private void HandleBallCollision(Frame f, Ball* ball, PhysicsBody2D* physicsBody)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);

            var vel = physicsBody->Velocity;
            int ballSpeedIncrement = 5;
            var minSpeed = config.BallBaseSpeed + ball->BounceCount * ballSpeedIncrement;

            vel = vel.Normalized * FPMath.Max(minSpeed, vel.Magnitude);

            // vel.X should be greater than vel.Y or the ball can stuck
            vel.X = FPMath.Sign(vel.X) * FPMath.Max(FPMath.Abs(vel.X), FPMath.Abs(vel.Y));

            physicsBody->Velocity = vel;
            ball->BounceCount++;
        }

        private void HandleBallHitPaddle(Frame f, Ball* ball)
        {
            //ball->Velocity.X *= -1;
        }
    }
}