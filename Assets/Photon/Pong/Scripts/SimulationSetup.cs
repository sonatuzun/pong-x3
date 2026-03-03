using Quantum;
using UnityEngine;

public class SimulationSetup : MonoBehaviour
{

    private void Awake()
    {
        var debugRunner = GetComponent<QuantumRunnerLocalDebug>();

        debugRunner.RuntimeConfig.IsLocalGame = true;
        debugRunner.RuntimeConfig.BotCount = 2;
        debugRunner.RuntimeConfig.LocalPlayerCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
