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
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        int playerCount = PlayerPrefs.GetInt("PlayerCount");

        // Main player
        GameObject mainCharacter = Instantiate(
            playerPrefabs[characterIndex],
            spawnPoint.transform.position,
            Quaternion.identity
        );
        var mainMove = mainCharacter.GetComponent<PlayerMovementScript>();
        mainMove.currentNode = 0; // All players start at node 0
        mainMove.startOffset = Vector3.zero; // No offset for main player
        GameTurnManager.instance.RegisterPlayer(mainMove);
        mainCharacter.GetComponent<NameScript>().SetName(PlayerPrefs.GetString("PlayerName", "Kroplis"));

        // AI players
        otherPlayers = new int[playerCount];
        string[] nameArray = ReadLinesFromFile(textFileName);

        for (int i = 0; i < otherPlayers.Length; i++)
        {
            index = Random.Range(0, playerPrefabs.Length);
            GameObject otherPlayer = Instantiate(
                playerPrefabs[index],
                spawnPoint.transform.position,
                Quaternion.identity
            );
            var moveScript = otherPlayer.GetComponent<PlayerMovementScript>();
            moveScript.currentNode = 0; // All players start at node 0
            // Offset each AI player to avoid overlap
            float offsetAmount =5f; // Adjust as needed
            moveScript.startOffset = new Vector3((i + 1) * offsetAmount, 0, (i + 1) * 3f);
            GameTurnManager.instance.RegisterPlayer(moveScript);
            otherPlayer.GetComponent<NameScript>().SetName(
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
