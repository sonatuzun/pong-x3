using Photon.Deterministic;

namespace Quantum.Pong
{
    public unsafe class CollisionsSystem : SystemSignalsOnly, ISignalOnCollisionEnter2D, ISignalOnTriggerEnter2D
    {
        public void OnCollisionEnter2D(Frame f, CollisionInfo2D info)
        {
            if (f.Unsafe.TryGetPointer<Ball>(info.Entity, out var ball))
            {
                if (f.Has<Paddle>(info.Other))
                {
                    HandleBallHitPaddle(f, ball);
                }
            }
        }

        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (f.Unsafe.TryGetPointer<Paddle>(info.Entity, out var paddle))
            {
                if (f.Unsafe.TryGetPointer<Ball>(info.Other, out var ball))
                {
                    HandleBallHitPaddle(f, ball);
                }
            }
        }

        private void HandleBallHitPaddle(Frame f, Ball* ball)
        {
            ball->Velocity.X *= -1;
        }
    }
}