using UnityEngine;

using Text = TMPro.TextMeshProUGUI;

public class UserInfoPopupController : MonoBehaviour
{
    [SerializeField]
    private Text _userNameText;

    [SerializeField]
    private Text _favouriteAnimalText;

    public void SetUserData(LeaderboardPlayerData playerData)
    {
        _userNameText.text = playerData.username;
        _favouriteAnimalText.text = playerData.favoriteAnimal;
    }

    public void OnBackButtonClicked()
    {
        var transform = GetComponent<Transform>();
        Destroy(transform.gameObject);
    }
}
