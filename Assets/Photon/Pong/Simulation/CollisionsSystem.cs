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
            if (f.Unsafe.TryGetPointer<Ball>(info.Entity, out var ball) 
                && f.Unsafe.TryGetPointer<PhysicsBody2D>(info.Entity, out var body)
                && f.Unsafe.TryGetPointer<Transform2D>(info.Entity, out var transform))
            {
                HandleBallVelocityAfterCollision(f, ball, body, transform, info);
            }
        }

        private void HandleBallVelocityAfterCollision(Frame f, Ball* ball, PhysicsBody2D* ballBody, 
            Transform2D* ballTransform, ExitInfo2D info)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);

            var vel = ballBody->Velocity;

            // Bounced from paddle
            if (f.Unsafe.TryGetPointer<Paddle>(info.Other, out var paddle)
                && f.Unsafe.TryGetPointer<PhysicsBody2D>(info.Other, out var paddleBody))
            {
                // This check prevents side collisions with the paddle to be counted as paddle bounces
                // The ball must be going towards the center (horizontally) for it to be counted as bounce
                bool validBounce = ballBody->Velocity.X * ballTransform->Position.X < 0;

                if (validBounce)
                {                
                    ball->PaddleBounceCount++;

                    // inherit some velocity from paddle for better control of the ball
                    ballBody->Velocity.Y = FPMath.Lerp(ballBody->Velocity.Y, paddleBody->Velocity.Y, FP._0_33); //config.VerticalVelocityTransferRate;

                    // results in a minimum launch angle
                    ballBody->Velocity.X = FPMath.Sign(vel.X) * FPMath.Max(FPMath.Abs(vel.X), FPMath.Abs(vel.Y) * FP._0_50);
                }
            }

            // minimum ball speed increases as the ball bounces
            // it may also get faster due to physics
            // but it can never exceed the BallMaxSpeed from game config
            var minSpeed = FPMath.Min(config.BallBaseSpeed + ball->PaddleBounceCount * config.BallSpeedIncrement, config.BallMaxSpeed);
            vel = vel.Normalized * FPMath.Clamp(vel.Magnitude, minSpeed, config.BallMaxSpeed);

            ballBody->Velocity = vel;
        }

        private void HandleBallHitPaddle(Frame f, Ball* ball)
        {
            //ball->Velocity.X *= -1;
        }
    }
}