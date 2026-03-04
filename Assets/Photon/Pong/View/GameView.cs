namespace Quantum.Asteroids
{
    using Quantum;
    using UnityEngine;
    using UnityEngine.SceneManagement;
#if QUANTUM_ENABLE_TEXTMESHPRO
    using Text = TMPro.TextMeshProUGUI;
#else
  using Text = UnityEngine.UI.Text;
#endif

    /// <summary>
    /// The AsteroidsGameView class is responsible for updating the game's UI elements
    /// such as the level text and scoreboard.
    /// </summary>
    public unsafe class GameView : QuantumSceneViewComponent
    {
        public GameObject leaderboardPrefab;

        public Text team1ScoreText;
        public Text team2ScoreText;
        public Text gameResultText;

        public UnityEngine.UI.Button restartButton;
        public UnityEngine.UI.Button backToMenuButton;
        public UnityEngine.UI.Button leaderboardButton;
        public UnityEngine.UI.Button smallBackButton;

        /// <summary>
        /// The camera to enable when Quantum is in QUANTUM_XY mode.
        /// </summary>
        public Camera Camera2D;

        /// <summary>
        /// The camera to enable when Quantum is 3D mode (default).
        /// </summary>
        public Camera Camera3D;

        /// <summary>
        /// Toggle the 2D or 3D camera based on QUANTUM_XY.
        /// </summary>
        public override void OnInitialize()
        {
#if QUANTUM_XY
            Camera2D.gameObject.SetActive(true);
            Camera3D.gameObject.SetActive(false);
#else
      Camera2D.gameObject.SetActive(false);
      Camera3D.gameObject.SetActive(true);
#endif
            restartButton.onClick.AddListener(OnRestartButtonClicked);
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
            leaderboardButton.onClick.AddListener(OnLeaderboardButtonClicked);
            smallBackButton.onClick.AddListener(OnSmallBackButtonClicked);

        }

        /// <summary>
        /// Updates the game view, including the level text and scoreboard.
        /// </summary>
        public override void OnUpdateView()
        {
            int team1Score = VerifiedFrame.Global->Team1Score;
            int team2Score = VerifiedFrame.Global->Team2Score;

            team1ScoreText.text = $"{team1Score}";
            team2ScoreText.text = $"{team2Score}";

            bool isGameOver = VerifiedFrame.Global->GameOver;
            bool isLocalGame = PlayerPrefs.GetInt("IsLocalGame") == 1;

            smallBackButton.gameObject.SetActive(isLocalGame);
            restartButton.gameObject.SetActive(isGameOver && isLocalGame);
            backToMenuButton.gameObject.SetActive(isGameOver);
            leaderboardButton.gameObject.SetActive(isGameOver);

            gameResultText.enabled = isGameOver;
            gameResultText.text = (team1Score > team2Score) ? "Team 1 Won" : "Team 2 Won";
        }

        private void OnRestartButtonClicked()
        {
            QuantumRunner.Default?.Shutdown();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnBackToMenuButtonClicked()
        {
            GoBackToMenu();
        }

        private void OnLeaderboardButtonClicked()
        {
            var leaderboard = Instantiate(leaderboardPrefab);
        }

        private void OnSmallBackButtonClicked()
        {
            GoBackToMenu();
        }

        private void GoBackToMenu()
        {
            QuantumRunner.Default?.Shutdown();
            SceneManager.LoadScene(0);
        }
    }
}