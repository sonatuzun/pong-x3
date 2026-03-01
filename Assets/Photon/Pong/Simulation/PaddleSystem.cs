namespace Quantum.Pong
{
    using Photon.Deterministic;
    using System.Buffers.Text;

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
            Input* rawInput = f.GetPlayerInput(filter.PlayerLink->PlayerRef);

            PongUtils.PaddleInput input = PongUtils.ProcessInput(rawInput, true, true);


            UpdatePaddleMovement(f, ref filter, input, config);

            FP baseX = filter.Paddle->BaseX;
            FP targetX = input.Charge ? baseX + 8 * FPMath.Sign(baseX) : baseX;

            StabilizePaddle(f, ref filter, targetX);
        }

        /// <summary>
        /// Handle input.
        /// Move the paddle.
        /// Limit it's movement in top and bottom.
        /// </summary>
        private void UpdatePaddleMovement(Frame f, ref Filter filter, PongUtils.PaddleInput input, PongGameConfig config)
        {
            FP paddleExtent = config.PaddleBaseSize * FP._0_50;
            FP paddleSpeed = config.PaddleBaseSpeed;
            FP mapExtentY = config.GameMapSize.Y * FP._0_50;
            FP movementLimit = mapExtentY - paddleExtent;

            PhysicsBody2D* body = filter.PhysicsBody;
            Transform2D* transform = filter.Transform;

            if (input.Up && transform->Position.Y < movementLimit)
            {
                body->Velocity = FPVector2.Up * paddleSpeed;
            }
            else if (input.Down && transform->Position.Y > -movementLimit)
            {
                body->Velocity = FPVector2.Down * paddleSpeed;
            }
            else
            {
                body->Velocity = FPVector2.Zero;
            }
        }

        private void StabilizePaddle(Frame f, ref Filter filter, FP targetX)
        {
            filter.PhysicsBody->AddAngularImpulse(-filter.Transform->Rotation * 15);

            FP diff = targetX - filter.Transform->Position.X;
            filter.PhysicsBody->AddLinearImpulse(new FPVector2(FP.FromString("1.5") * FPMath.Sign(diff) * diff * diff, 0));
        }


        public void SpawnPaddle(Frame f, EntityRef paddle)
        {
            throw new System.NotImplementedException();
        }
    }
}