using Quantum;
using UnityEngine;

public class SimulationSetup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

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
