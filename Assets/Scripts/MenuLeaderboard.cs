using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;

public class MenuLeaderboard : MonoBehaviour
{
    public Transform listParent; // Assign the layout group in inspector
    public GameObject entryPrefab; // Assign the entry prefab

    void Start()
    {
        LoadAndDisplayLeaderboard();
    }

    public void LoadAndDisplayLeaderboard()
    {
        string path = Application.persistentDataPath + "/leaderboard.json";
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveLoadScript.LeaderboardData>(json);

        foreach (Transform child in listParent)
            Destroy(child.gameObject);

        int place = 1;
        foreach (var entry in data.entries.OrderBy(e => e.hasFinished ? 0 : 1).ThenBy(e => e.hasFinished ? e.timePlayed : -1))
        {
            var go = Instantiate(entryPrefab, listParent);
            go.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = entry.playerName;
            go.transform.Find("DiceSum").GetComponent<TextMeshProUGUI>().text = entry.diceSum.ToString();
            go.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = entry.timePlayed.ToString("0.0") + "s";
            go.transform.Find("Place").GetComponent<TextMeshProUGUI>().text = entry.hasFinished ? place + "." : "-";
            if (entry.hasFinished) place++;
        }
    }
}