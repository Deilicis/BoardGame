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
        GameObject mainCharacter = Instantiate(
            playerPrefabs[characterIndex],
            spawnPoint.transform.position,
            Quaternion.identity
        );
        mainCharacter.GetComponent<NameScript>().SetName(PlayerPrefs.GetString("PlayerName", "Kroplis"));

        otherPlayers = new int[PlayerPrefs.GetInt("PlayerCount")];
        string[] nameArray = ReadLinesFromFile(textFileName);

        for (int i = 0; i < otherPlayers.Length; i++)
        {
            spawnPoint.transform.position += new Vector3(0.8f, 0, 0.08f);
            index = Random.Range(0, playerPrefabs.Length);
            GameObject otherPlayer = Instantiate(
                playerPrefabs[index],
                spawnPoint.transform.position,
                Quaternion.identity
            );
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
