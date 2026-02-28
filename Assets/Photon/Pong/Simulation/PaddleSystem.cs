namespace Quantum.Pong
{
    using Photon.Deterministic;

    public unsafe class PaddleSystem : SystemMainThreadFilter<PaddleSystem.Filter>, ISignalSpawnPaddle
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public Paddle* Paddle;
            public PlayerLink* PlayerLink;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
            Input* input = f.GetPlayerInput(filter.PlayerLink->PlayerRef);

            UpdatePaddleMovement(f, ref filter, input, config);
            LimitPaddleMovement(f, ref filter);
        }

        private void UpdatePaddleMovement(Frame f, ref Filter filter, Input* input, PongGameConfig config)
        {
            //FP turnSpeed = config.ShipTurnSpeed;
            if (input->Up)
            {
                filter.Transform->Position += FPVector2.Up;
            }

            if (input->Down)
            {
                filter.Transform->Position += FPVector2.Down;
            }
        }

        public void LimitPaddleMovement(Frame f, ref Filter filter)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
            FP extend = config.GameMapSize.Y / FP.FromString("2");
            FP y = filter.Transform->Position.Y;

            if (y > extend)
            {
                filter.Transform->Position.Y = extend;
            }
            else if (y < -extend)
            {
                filter.Transform->Position.Y = -extend;
            }
        }

        public void SpawnPaddle(Frame f, EntityRef paddle)
        {
            throw new System.NotImplementedException();
        }
    }
}