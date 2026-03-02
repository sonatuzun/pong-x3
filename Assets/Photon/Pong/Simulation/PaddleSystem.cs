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
            public ControlFlags* ControlFlags;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            PongGameConfig config = f.FindAsset(f.RuntimeConfig.GameConfig);

            PongUtils.PaddleInput input = new PongUtils.PaddleInput();
            ControlFlags* flags = filter.ControlFlags;

            if (f.Unsafe.TryGetPointer<Bot>(filter.Entity, out var botInfo))
            {
                input = PongUtils.CreateBotInput(f, filter.Entity);
            }
            else if (f.Unsafe.TryGetPointer<PlayerLink>(filter.Entity, out var playerLink))
            {
                Input* rawInput = f.GetPlayerInput(playerLink->PlayerRef);
                input = PongUtils.ProcessInput(rawInput, flags->AcceptInputForP1, flags->AcceptInputForP2);
            }

            UpdatePaddleMovement(f, ref filter, input, config);

            FP baseX = filter.Paddle->BaseX;
            FP targetX = input.Charge ? baseX + config.PaddleChargeDistance * FPMath.Sign(baseX) : baseX;

            StabilizePaddle(f, ref filter, targetX, config);
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

        private void StabilizePaddle(Frame f, ref Filter filter, FP targetX, PongGameConfig config)
        {
            filter.PhysicsBody->AddAngularImpulse(-filter.Transform->Rotation * 15);

            FP diff = targetX - filter.Transform->Position.X;
            filter.PhysicsBody->AddLinearImpulse(new FPVector2(config.PaddleChargeForce * diff, 0));
        }


        public void SpawnPaddle(Frame f, EntityRef paddle)
        {
            throw new System.NotImplementedException();
        }
    }
}