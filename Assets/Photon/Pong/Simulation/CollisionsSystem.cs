using Photon.Deterministic;
using UnityEngine.LowLevelPhysics2D;

namespace Quantum.Pong
{
    public unsafe class CollisionsSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D, ISignalOnTriggerExit2D, ISignalOnCollisionExit2D
    {
        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (f.Unsafe.TryGetPointer<GoalLine>(info.Entity, out var goalLine)
                && f.Unsafe.TryGetPointer<Ball>(info.Other, out var ball))
            {
                if (goalLine->isOnLeft)
                {
                    f.Global->Team2Score++;
                }
                else
                {
                    f.Global->Team1Score++;
                }

                PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
                if (f.Global->Team1Score >= config.ScoreLimit 
                    || f.Global->Team2Score >= config.ScoreLimit)
                {
                    f.Global->GameOver = true;
                }
            }
        }

        public void OnTriggerExit2D(Frame f, ExitInfo2D info)
        {
            if (f.Unsafe.TryGetPointer<GoalLine>(info.Entity, out var goalLine)
                && f.Unsafe.TryGetPointer<Ball>(info.Other, out var ball))
            {
                f.Destroy(info.Other);

                if (!f.Global->GameOver)
                {
                    f.Signals.SpawnNewBall();
                }
            }
        }

        public void OnCollisionExit2D(Frame f, ExitInfo2D info)
        {
            if (f.Unsafe.TryGetPointer<Ball>(info.Entity, out var ball) 
                && f.Unsafe.TryGetPointer<PhysicsBody2D>(info.Entity, out var body)
                && f.Unsafe.TryGetPointer<Transform2D>(info.Entity, out var transform))
            {
                HandleGameStateAfterBounce(f, ball, body, transform, info);
            }
        }

        private void HandleGameStateAfterBounce(Frame f, Ball* ball, PhysicsBody2D* ballBody, 
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
                bool validBounce = (ballBody->Velocity.X * ballTransform->Position.X) < 0;

                if (validBounce)
                {                
                    ball->PaddleBounceCount++;
                    paddle->BallHitCount++;

                    // inherit some vertical velocity from paddle
                    //ballBody->Velocity.Y = FPMath.Lerp(ballBody->Velocity.Y, paddleBody->Velocity.Y, config.VelocityTransferRate);
                }
            }
            // Bounced from wall
            else
            {
                // Lose some vertical velocity to make ball easier to hit
                vel.Y *= FP.FromString("0.9");
            }

            // minimum ball speed increases as the ball bounces but does not exceed the maximimum ball speed
            // it may also get faster due to physics
            // but it can never exceed the BallMaxSpeed from game config
            var minSpeed = FPMath.Min(config.BallBaseSpeed + ball->PaddleBounceCount * config.BallSpeedIncrement, config.BallMaxSpeed);
            vel = vel.Normalized * FPMath.Clamp(vel.Magnitude, minSpeed, config.BallMaxSpeed);

            // a minimum vel.X prevents the ball from getting stuck
            vel.X = FPMath.Sign(vel.X) * FPMath.Max(FPMath.Abs(vel.X), config.BallMinHorizontalSpeed);

            ballBody->Velocity = vel;
        }
    }
}