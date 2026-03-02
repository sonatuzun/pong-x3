namespace Quantum.Asteroids
{
    using Quantum;
    using UnityEngine;
#if QUANTUM_ENABLE_TEXTMESHPRO
    using Text = TMPro.TextMeshProUGUI;
#else
  using Text = UnityEngine.UI.Text;
#endif

    /// <summary>
    /// The AsteroidsGameView class is responsible for updating the game's UI elements
    /// such as the level text and scoreboard.
    /// </summary>
    public unsafe class AsteroidsGameView : QuantumSceneViewComponent
    {
        public Text team1ScoreText;
        public Text team2ScoreText;
        public Text gameResultText;

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

            gameResultText.enabled = VerifiedFrame.Global->GameOver;
            gameResultText.text = (team1Score > team2Score) ? "Team 1 Won" : "Team 2 Won";

            /*
            if (ScoreBoard != null)
            {
                ScoreBoard.text = "<b>Score</b>\n";
                var shipsFilter = VerifiedFrame.Filter<PlayerLink, Paddle>();
                while (shipsFilter.Next(out var entity, out var playerLink, out var shipFields))
                {
                    var playerName = VerifiedFrame.GetPlayerData(playerLink.PlayerRef).PlayerNickname;
                    //ScoreBoard.text += $"{playerName}: {shipFields.Score}  \n";
                }
            }
            */
        }
    }
}