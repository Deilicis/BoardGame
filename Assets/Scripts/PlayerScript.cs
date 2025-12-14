using UnityEngine;
using System.IO;

public class PlayerScript : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    int characterIndex;
    public GameObject spawnPoint;
    int[] otherPlayers;
    int index;
    private const string textFileName = "PlayerNames";

    void Start()
    {
        int totalPlayers = PlayerPrefs.GetInt("PlayerCount", 2); // Total players (human + AI)
        int humanPlayers = PlayerPrefs.GetInt("HumanPlayerCount", 1); // How many human players
        string[] nameArray = ReadLinesFromFile(textFileName);

        // Instantiate human players
        for (int i = 0; i < humanPlayers; i++)
        {
            int charIdx = PlayerPrefs.GetInt($"Player_{i}_Character", 0);
            string name = PlayerPrefs.GetString($"Player_{i}_Name", $"Player {i + 1}");

            GameObject playerObj = Instantiate(
                playerPrefabs[charIdx],
                spawnPoint.transform.position,
                Quaternion.identity
            );
            var moveScript = playerObj.GetComponent<PlayerMovementScript>();
            moveScript.currentNode = 0;
            moveScript.startOffset = new Vector3(i * 5f, 0, i * 3f); // Offset to avoid overlap
            GameTurnManager.instance.RegisterPlayer(moveScript);
            playerObj.GetComponent<NameScript>().SetName(name);
        }

        // Instantiate AI players
        int aiCount = Mathf.Max(0, totalPlayers - humanPlayers);
        for (int i = 0; i < aiCount; i++)
        {
            int charIdx = Random.Range(0, playerPrefabs.Length);
            GameObject aiObj = Instantiate(
                playerPrefabs[charIdx],
                spawnPoint.transform.position,
                Quaternion.identity
            );
            var moveScript = aiObj.GetComponent<PlayerMovementScript>();
            moveScript.currentNode = 0;
            moveScript.startOffset = new Vector3((i + humanPlayers) * 5f, 0, (i + humanPlayers) * 3f);
            GameTurnManager.instance.RegisterPlayer(moveScript);
            aiObj.GetComponent<NameScript>().SetName(
                nameArray[Random.Range(0, nameArray.Length)]
            );
        }
    }


    string[] ReadLinesFromFile(string filename)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filename);
        if (textAsset != null)
        {
            return textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            Debug.LogError("File not found: " + filename);
            return new string[0];
        }
    }
}
