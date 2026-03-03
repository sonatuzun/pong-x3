using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField]
    private GameObject _scoreItemPrefab;

    [SerializeField]
    private LeaderboardData _leaderboardData;

    [SerializeField]
    private RectTransform _entryHolder;

    [SerializeField]
    private UnityEngine.UI.Button _backButton;

    void Start()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);

        var leaderboardData = LetsPretendWeAreGettingLeaderboardDataOnline();
        FillLeaderboard(leaderboardData);
    }

    private LeaderboardData LetsPretendWeAreGettingLeaderboardDataOnline()
    {
        return _leaderboardData;
    }

    private void FillLeaderboard(LeaderboardData leaderboardData)
    {
        LeaderboardPlayerData[] playerDatas = leaderboardData.playerDatas;

        for (int i = 0; i < playerDatas.Length; i++)
        {
            var scoreItem = Instantiate(_scoreItemPrefab);
            scoreItem.transform.SetParent(_entryHolder);

            var rank = i + 1;
            var playerData = playerDatas[i];
            var itemController = scoreItem.GetComponent<ScoreItemController>();
            itemController.SetPlayerData(rank, playerData);
        }
    }

    private void OnBackButtonPressed()
    {
        var transform = GetComponent<Transform>();
        Destroy(transform.gameObject);
    }
}
