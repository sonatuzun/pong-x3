using Quantum;
using UnityEngine;

public class PongGameSetup : MonoBehaviour
{

    private void Awake()
    {
        var debugRunner = GetComponent<QuantumRunnerLocalDebug>();

        bool isLocalGame = PlayerPrefs.GetInt("IsLocalGame") == 1;
        debugRunner.RuntimeConfig.IsLocalGame = isLocalGame;

        debugRunner.RuntimeConfig.BotCount = PlayerPrefs.GetInt("BotCount");
        debugRunner.RuntimeConfig.LocalPlayerCount = PlayerPrefs.GetInt("LocalPlayerCount"); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
