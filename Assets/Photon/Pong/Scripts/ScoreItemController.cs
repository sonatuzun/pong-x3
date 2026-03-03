using UnityEngine;

using Text = TMPro.TextMeshProUGUI;

public class ScoreItemController : MonoBehaviour
{
    [SerializeField]
    private Text _rankText;

    [SerializeField]
    private Text _nameText;

    [SerializeField]
    private Text _scoreText;

    private LeaderboardPlayerData _playerData;

    public void SetPlayerData(int rank, LeaderboardPlayerData playerData)
    {
        _rankText.text = $"{rank}.";
        _playerData = playerData;

        _nameText.text = playerData.username;

        // dummy score calculation
        _scoreText.text = $"{1000000 - rank * 1000}";
    }
}
