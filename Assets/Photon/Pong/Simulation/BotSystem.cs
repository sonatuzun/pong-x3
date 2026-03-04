namespace Quantum.Pong
{
    using Photon.Deterministic;
    using static Quantum.Pong.PongUtils;

    /// <summary>
    /// The <c>AsteroidsProjectileSystem</c> class manages the lifecycle of projectiles,
    /// including updating their time-to-live (TTL) and handling projectile shooting signals.
    /// </summary>
    public unsafe class BotSystem : SystemMainThreadFilter<BotSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public BotInfo* BotInfo;
            public Paddle* Paddle;
        }

        public static EntityRef? FindAlly(Frame f, EntityRef entity)
        {
            Transform2D transform = f.Get<Transform2D>(entity);

            foreach (EntityComponentPair<Paddle> other in f.GetComponentIterator<Paddle>())
            {
                if (other.Entity == entity)
                    continue;

                Transform2D otherTransform = f.Get<Transform2D>(other.Entity);
                bool isAlly = (otherTransform.Position.X * transform.Position.X) > 0; // on the same side

                if (!isAlly)
                    continue;

                return other.Entity;
            }

            return null;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            Transform2D* transform = filter.Transform;
            FPVector2 paddlePos = transform->Position;
            BotInfo* botInfo = filter.BotInfo;
            Paddle* paddle = filter.Paddle;

            EntityRef? ally = FindAlly(f, filter.Entity);
            FPVector2 vectorToAlly = FPVector2.Zero;
            bool isTooCloseToAlly = false;
            bool isDefender = false;

            // reset paddle input
            paddle->Input = new PaddleInput();

            if (ally.HasValue)
            {
                Transform2D allyTransform = f.Get<Transform2D>(ally.Value);
                vectorToAlly = allyTransform.Position - paddlePos;
                isTooCloseToAlly = FPMath.Max(vectorToAlly.Y) < 15;

                var allyPaddle = f.Get<Paddle>(ally.Value);

                if (!f.Has<BotInfo>(ally.Value))
                {
                    isDefender = true;
                }
                else if (allyPaddle.BallHitCount < paddle->BallHitCount)
                {
                    isDefender = true;
                }
                else if (allyPaddle.BallHitCount == paddle->BallHitCount
                    && f.Unsafe.TryGetPointer<BotInfo>(ally.Value, out var allyBotInfo)
                    && allyBotInfo->BotIndex > botInfo->BotIndex)
                {
                    isDefender = true;
                }
            }

            if (isTooCloseToAlly)
            {
                paddle->Input.Charge = isDefender;
            }

            // target on the first ball even if multiple balls are active
            foreach (var pair in f.GetComponentIterator<Ball>())
            {
                Transform2D ballTransform = f.Get<Transform2D>(pair.Entity);

                FP actionTreshold = 5;
                FPVector2 ballPos = ballTransform.Position;

                if (isDefender)
                {
                    ballPos.Y *= -1;
                }

                if (ballPos.Y > paddlePos.Y + actionTreshold)
                {
                    paddle->Input.Up = true;
                }
                else if (ballPos.Y < paddlePos.Y - actionTreshold)
                {
                    paddle->Input.Down = true;
                }
                else if (!isDefender)
                {
                    paddle->Input.Charge = true;
                }
            }
        }
    }
}