namespace Quantum.Pong
{
    using Photon.Deterministic;

    public unsafe class PaddleSystem : SystemMainThreadFilter<PaddleSystem.Filter>, ISignalSpawnPaddle
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public PhysicsBody2D* PhysicsBody;
            public Paddle* Paddle;
            public PlayerLink* PlayerLink;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);
            Input* input = f.GetPlayerInput(filter.PlayerLink->PlayerRef);

            UpdatePaddleMovement(f, ref filter, input, config);
            //LimitPaddleMovement(f, ref filter);

            StablizePaddle(f, ref filter, input);
        }

        private void UpdatePaddleMovement(Frame f, ref Filter filter, Input* input, PongGameConfig config)
        {
            // TODO: add a paddle movement limit

            if (input->Up)
            {
                filter.PhysicsBody->Velocity = FPVector2.Up * config.PaddleBaseSpeed;
            }
            else if (input->Down)
            {
                filter.PhysicsBody->Velocity = FPVector2.Down * config.PaddleBaseSpeed;
            }
            else
            {
                filter.PhysicsBody->Velocity = FPVector2.Zero;
            }
        }

        private void StablizePaddle(Frame f, ref Filter filter, Input* input)
        {
            filter.PhysicsBody->AddAngularImpulse(-filter.Transform->Rotation * 15);

            FP baseX = filter.Paddle->BaseX;
            FP target = input->Fire ? baseX + 8 * FPMath.Sign(baseX) : baseX;
            FP diff = target - filter.Transform->Position.X;
            filter.PhysicsBody->AddLinearImpulse(new FPVector2(FP.FromString("1.5") * FPMath.Sign(diff) * diff * diff, 0));
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