using Photon.Deterministic;

namespace Quantum.Pong
{
  public unsafe class CollisionsSystem : SystemSignalsOnly, ISignalOnCollisionEnter2D
  {
    public void OnCollisionEnter2D(Frame f, CollisionInfo2D info)
    {
      if (f.Unsafe.TryGetPointer<Ball>(info.Entity, out var ball))
      {
        if (f.Has<Paddle>(info.Other))
        {
          HandleBallHitPaddle(f, info, ball);
        }
      }
    }

    private void HandleBallHitPaddle(Frame f, CollisionInfo2D info, Ball* ball)
    {
       ball->Velocity.X *= -1;
    }
  }
}