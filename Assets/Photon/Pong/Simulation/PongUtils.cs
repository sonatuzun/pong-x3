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
    }
}
