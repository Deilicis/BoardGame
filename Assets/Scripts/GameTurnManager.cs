using System.Collections.Generic;
using UnityEngine;

public class GameTurnManager : MonoBehaviour
{
    public static GameTurnManager instance;

    public List<PlayerMovementScript> players = new List<PlayerMovementScript>();
    private int currentPlayerIndex = 0;

    void Awake()
    {
        instance = this;
    }

    public void RegisterPlayer(PlayerMovementScript player)
    {
        players.Add(player);
        Debug.Log("REGISTERED: " + player.name);
    }

    public PlayerMovementScript GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void NextPlayerTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }
}
