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
            StabilizePaddle(f, ref filter, input);
        }

        /// <summary>
        /// Handle input.
        /// Move the paddle.
        /// Limit it's movement in top and bottom.
        /// </summary>
        private void UpdatePaddleMovement(Frame f, ref Filter filter, Input* input, PongGameConfig config)
        {
            FP paddleExtent = config.PaddleBaseSize * FP._0_50;
            FP paddleSpeed = config.PaddleBaseSpeed;
            FP mapExtentY = config.GameMapSize.Y * FP._0_50;
            FP movementLimit = mapExtentY - paddleExtent;

            PhysicsBody2D* body = filter.PhysicsBody;
            Transform2D* transform = filter.Transform;

            if (input->Up && transform->Position.Y < movementLimit)
            {
                body->Velocity = FPVector2.Up * paddleSpeed;
            }
            else if (input->Down && transform->Position.Y > -movementLimit)
            {
                body->Velocity = FPVector2.Down * paddleSpeed;
            }
            else
            {
                body->Velocity = FPVector2.Zero;
            }
        }

        private void StabilizePaddle(Frame f, ref Filter filter, Input* input)
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