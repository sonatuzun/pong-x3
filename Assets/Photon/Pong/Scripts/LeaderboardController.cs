using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    public UnityEngine.UI.Button backButton;

    // get data from a file
    // get entries object 
    // populate popup
    // be able to close the popup

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        var transform = GetComponent<Transform>();
        Destroy(transform.gameObject);
    }
}
