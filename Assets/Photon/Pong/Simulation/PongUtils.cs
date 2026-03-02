namespace Quantum.Pong
{
    using Photon.Deterministic;

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

        public static PaddleInput CreateBotInput(Frame f, FPVector2 paddlePos)
        {
            PaddleInput res = new PaddleInput();

            foreach( EntityComponentPair<Ball> ball in f.GetComponentIterator<Ball>())
            {
                EntityRef ballRef = ball.Entity;
                
                if (f.Unsafe.TryGetPointer<Transform2D>(ballRef, out var ballTransform))
                {
                    FP actionTreshold = 5;
                    FPVector2 ballPos = ballTransform->Position;

                    if (ballPos.Y > paddlePos.Y + actionTreshold)
                    {
                        res.Up = true;
                    }
                    else if (ballPos.Y < paddlePos.Y - actionTreshold)
                    {
                        res.Down = true;
                    }
                    else
                    {
                        res.Charge = true;
                    }

                }
            }

            return res;
        }
    }
}
