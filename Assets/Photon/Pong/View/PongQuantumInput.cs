namespace Quantum.Pong
{
    using Photon.Deterministic;
    using UnityEngine;
#if ENABLE_INPUT_SYSTEM && QUANTUM_ENABLE_INPUTSYSTEM
    using UnityEngine.InputSystem;
#endif

    /// <summary>
    /// The <c>AsteroidsQuantumInput</c> class handles the input for the Asteroids game by subscribing to Quantum's input callback system.
    /// It's more complicated that needed because it supports both the new Input System and the legacy Input Manager.
    /// The essence of this class is the <c>OnPollInput</c> method that reads the current input state and sets it in the Quantum input callback.
    /// </summary>
    public class PongQuantumInput : QuantumMonoBehaviour
    {
        public GameObject MobileControlsPrefab;
        public Canvas MobileControlsCanvas;

#if ENABLE_INPUT_SYSTEM && QUANTUM_ENABLE_INPUTSYSTEM
        private GameObject _mobileControls;
        private PlayerInput _playerInput;

        private InputAction _p1_move;
        private InputAction _p1_charge;

        private InputAction _p2_move;
        private InputAction _p2_charge;
#endif

        /// <summary>
        /// Get or create the PlayerInput component. Uses the Quantum default input action asset.
        /// Caches the individual input actions to query inside the Quantum input polling callback.
        /// </summary>
        private void Awake()
        {
#if ENABLE_INPUT_SYSTEM && QUANTUM_ENABLE_INPUTSYSTEM
            if (TryGetComponent<PlayerInput>(out _playerInput) == false)
            {
                _playerInput = gameObject.AddComponent<PlayerInput>();
                _playerInput.actions = QuantumDefaultConfigs.Global.InputActionAsset;
                _playerInput.actions.Enable();
            }

            _p1_move = _playerInput.actions["P1_Move"];
            _p1_charge = _playerInput.actions["P1_Charge"];

            _p2_move = _playerInput.actions["P2_Move"];
            _p2_charge = _playerInput.actions["P2_Charge"];
#endif
        }

        /// <summary>
        /// Subscribes to the Quantum input callback when the script is enabled.
        /// </summary>
        private void OnEnable()
        {
            QuantumCallback.Subscribe(this, (CallbackGameStarted callback) => OnGameStarted(callback));
            QuantumCallback.Subscribe(this, (CallbackGameDestroyed callback) => OnGameDestroyed(callback));
            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => OnPollInput(callback));
        }

        /// <summary>
        /// Polls the current input state and sets it in the Quantum input callback.
        /// </summary>
        /// <param name="callback">The input callback provided by Quantum.</param>
        private void OnPollInput(CallbackPollInput callback)
        {
            Quantum.Input input = new Quantum.Input();

            var p1_move = _p1_move.ReadValue<Vector2>();
            input.P1_Charge = _p1_charge.IsPressed();
            input.P1_Up = p1_move.y > 0.7f;
            input.P1_Down = p1_move.y < -0.7f;

            var p2_move = _p2_move.ReadValue<Vector2>();
            input.P2_Charge = _p2_charge.IsPressed();
            input.P2_Up = p2_move.y > 0.7f;
            input.P2_Down = p2_move.y < -0.7f;

            callback.SetInput(input, DeterministicInputFlags.Repeatable);
        }

        /// <summary>
        /// Create and toggle on mobile controls when the game starts.
        /// </summary>
        private void OnGameStarted(CallbackGameStarted callback)
        {
#if ENABLE_INPUT_SYSTEM && QUANTUM_ENABLE_INPUTSYSTEM && (UNITY_IOS || UNITY_ANDROID)
      if (_mobileControls == null) {
        _mobileControls = Instantiate(MobileControlsPrefab, MobileControlsCanvas.transform);
      }

      _mobileControls?.SetActive(true);
      // Mobile controls are disabled when starting the scene.
      // Toggle the player input to make stick controls to be mapped to actions.
      _playerInput.enabled = false;
      _playerInput.enabled = true;
#endif
        }

        /// <summary>
        /// Toggle mobile controls off.
        /// </summary>
        /// <param name="callback"></param>
        private void OnGameDestroyed(CallbackGameDestroyed callback)
        {
#if ENABLE_INPUT_SYSTEM && QUANTUM_ENABLE_INPUTSYSTEM && (UNITY_IOS || UNITY_ANDROID)
      _mobileControls?.SetActive(false);
#endif
        }
    }
}