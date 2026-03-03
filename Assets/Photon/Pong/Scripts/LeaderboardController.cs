using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreItemPrefab;

    [SerializeField]
    private LeaderboardData leaderboardData;

    [SerializeField]
    private Transform entryHolder;

    [SerializeField]
    private UnityEngine.UI.Button backButton;

    // get data from a file
    // get entries object 
    // populate popup
    // be able to close the popup

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backButton.onClick.AddListener(OnBackButtonPressed);

        var leaderboardData = LetsPretendWeAreGettingLeaderboardDataOnline();
        FillLeaderboard(leaderboardData);
    }

    private LeaderboardData LetsPretendWeAreGettingLeaderboardDataOnline()
    {
        return leaderboardData;
    }

    private void FillLeaderboard(LeaderboardData leaderboardData)
    {
        LeaderboardPlayerData[] playerDatas = leaderboardData.playerDatas;

        for (int i = 0; i < playerDatas.Length; i++)
        {
            var scoreItem = Instantiate(scoreItemPrefab);
            scoreItem.transform.SetParent(entryHolder);
        }
    }

    private void OnBackButtonPressed()
    {
        var transform = GetComponent<Transform>();
        Destroy(transform.gameObject);
    }
}
