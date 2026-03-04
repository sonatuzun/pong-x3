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

    [SerializeField]
    private GameObject _userInfoPopupPrefab;

    private LeaderboardPlayerData _playerData;

    public void SetPlayerData(int rank, LeaderboardPlayerData playerData)
    {
        _rankText.text = $"{rank}.";
        _playerData = playerData;
        _nameText.text = playerData.username;

        // dummy score calculation
        _scoreText.text = $"{1000000 - rank * 1000}";
    }

    public void Update()
    {
        /* 
         * if we set the item height to 1 we can use this code to always fit 
         * always 10 items to the leaderboard but it causes some other proble
         */

        //var rect = GetComponent<RectTransform>();
        //rect.sizeDelta = new Vector2(rect.sizeDelta.x, Screen.height * 0.1f);
    }

    public void OnClicked()
    {
        var userInfoPopup = Instantiate(_userInfoPopupPrefab);
        var popupController = userInfoPopup.GetComponent<UserInfoPopupController>();
        popupController.SetUserData(_playerData);
    }
}
