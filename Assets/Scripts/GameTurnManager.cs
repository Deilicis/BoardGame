using System.Collections.Generic;
using UnityEngine;

public class GameTurnManager : MonoBehaviour
{
    public static GameTurnManager instance;

    private List<PlayerMovementScript> allPlayers = new List<PlayerMovementScript>();
    private List<PlayerMovementScript> finishedPlayers = new List<PlayerMovementScript>();
    private int currentPlayerIndex = 0;
    public List<PlayerMovementScript> GetAllPlayers()
    {
        return allPlayers;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        allPlayers.Clear();
        finishedPlayers.Clear();
        currentPlayerIndex = 0;
    }

    public void RegisterPlayer(PlayerMovementScript player)
    {
        if (!allPlayers.Contains(player))
            allPlayers.Add(player);
    }

    public void OnPlayerFinished(PlayerMovementScript player)
    {
        if (!finishedPlayers.Contains(player))
            finishedPlayers.Add(player);

        Debug.Log($"Player finished: {player.gameObject.name}. Total finished: {finishedPlayers.Count}/{allPlayers.Count}");

        var cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
            cameraController.SwitchToDice();

        // Reset the dice for the next player
        var dice = FindObjectOfType<DiceRollScript>();
        if (dice != null)
            dice.ResetDice();

        // Advance to the next player if the game is not over
        if (finishedPlayers.Count < allPlayers.Count)
            NextPlayerTurn();

        if (finishedPlayers.Count == allPlayers.Count)
        {
            Debug.Log("All players finished! Showing victory screen.");
            GameManager.instance.ShowVictoryScreen(finishedPlayers[0]);
        }
    }

    public PlayerMovementScript GetCurrentPlayer()
    {
        if (allPlayers.Count == 0) return null;
        return allPlayers[currentPlayerIndex];
    }

    public void NextPlayerTurn()
    {
        if (allPlayers.Count == 0) return;

        int startIndex = currentPlayerIndex;
        do
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % allPlayers.Count;
            // If all players are finished, break to avoid infinite loop
            if (finishedPlayers.Count == allPlayers.Count)
                break;
        }
        while (finishedPlayers.Contains(allPlayers[currentPlayerIndex]) && currentPlayerIndex != startIndex);
    }
}
