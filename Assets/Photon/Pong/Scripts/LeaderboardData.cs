using UnityEngine;

[CreateAssetMenu(fileName = "LeaderboardData", menuName = "Pong/LeaderboardData", order = 1)]
public class LeaderboardData : ScriptableObject
{
    public LeaderboardPlayerData[] playerDatas;
}