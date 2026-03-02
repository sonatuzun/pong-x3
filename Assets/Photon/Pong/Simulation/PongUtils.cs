namespace Quantum.Pong
{
    using Photon.Deterministic;
    using UnityEngine;

    public static unsafe class PongUtils
    {
        public struct PaddleInput
        {
            public bool Up;
            public bool Down;
            public bool Charge;
        }

        public static PaddleInput ProcessInput(Quantum.Input* rawInput, bool includeP1, bool includeP2)
        {
            PaddleInput res = new PaddleInput();

            res.Up = (rawInput->P1_Up && includeP1) || (rawInput->P2_Up && includeP2);
            res.Down = (rawInput->P1_Down && includeP1) || (rawInput->P2_Down && includeP2);
            res.Charge = (rawInput->P1_Charge && includeP1) || (rawInput->P2_Charge && includeP2);

            return res;
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

        public static PaddleInput CreateBotInput(Frame f, EntityRef entity)
        {
            PaddleInput res = new PaddleInput();
            Transform2D transform = f.Get<Transform2D>(entity);
            FPVector2 paddlePos = transform.Position;

            EntityRef? ally = FindAlly(f, entity);
            FPVector2 vectorToAlly = FPVector2.Zero;
            bool isTooCloseToAlly = false;
            bool isDefender = false;

            if (ally.HasValue)
            {
                Transform2D allyTransform = f.Get<Transform2D>(ally.Value);
                vectorToAlly = allyTransform.Position - paddlePos;

                isTooCloseToAlly = FPMath.Max(vectorToAlly.Y) < 15;
                isDefender = (vectorToAlly.X * transform.Position.X) < 0;
            }

            if (isTooCloseToAlly)
            {
                res.Charge = isDefender;
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
                    res.Up = true;
                }
                else if (ballPos.Y < paddlePos.Y - actionTreshold)
                {
                    res.Down = true;
                }
                else if (!isDefender)
                {
                    res.Charge = true;
                }
            }

            return res;
        }
    }
}
