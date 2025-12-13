using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class LeaderboardScript : MonoBehaviour
{
    public GameObject[] leaderboardPanels; // Assign both panels in the inspector
    public Transform[] listParents;        // Assign the layout group for each panel
    public GameObject entryPrefab;         // name, time, sum, position

    public void RefreshLeaderboard()
    {
        if (GameTurnManager.instance == null)
        {
            Debug.LogWarning("Leaderboard: GameTurnManager not ready.");
            return;
        }

        var players = GameTurnManager.instance.GetAllPlayers();
        if (players == null || players.Count == 0)
        {
            Debug.LogWarning("Leaderboard: No players registered yet.");
            return;
        }

        // Order: finished first (by finish time), then unfinished by current progress
        var ordered = players
            .OrderBy(p => p.hasFinished ? 0 : 1)
            .ThenBy(p => p.hasFinished ? p.timePlayed : -p.currentNode)
            .ToList();

        for (int i = 0; i < listParents.Length; i++)
        {
            // Clear old entries
            foreach (Transform child in listParents[i])
                Destroy(child.gameObject);

            int place = 1;
            foreach (var p in ordered)
            {
                var entry = Instantiate(entryPrefab, listParents[i]);

                entry.transform.Find("Name").GetComponent<TextMeshProUGUI>().text =
                    p.GetComponent<NameScript>().playerName;

                entry.transform.Find("DiceSum").GetComponent<TextMeshProUGUI>().text =
                    p.totalDiceSum.ToString();

                entry.transform.Find("Time").GetComponent<TextMeshProUGUI>().text =
                    p.timePlayed.ToString("0.0") + "s";

                entry.transform.Find("Place").GetComponent<TextMeshProUGUI>().text =
                    p.hasFinished ? place + "." : "-";

                if (p.hasFinished) place++;
            }
        }
    }

    public void ShowLeaderboard()
    {
        foreach (var panel in leaderboardPanels){
            RefreshLeaderboard();
        }

        RefreshLeaderboard();
    }
}