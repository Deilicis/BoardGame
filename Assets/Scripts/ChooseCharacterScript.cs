using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ChooseCharacterScript : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject inputField;
    public SceneChanger sceneChanger;
    public TMP_Text playerText;
    private int playerCount;
    private int currentPlayer = 0;
    private int characterIndex = 0;
    private List<PlayerSelection> selections = new List<PlayerSelection>();

    void OnEnable()
    {
        playerCount = PlayerPrefs.GetInt("PlayerCount");
        Debug.Log("ChooseCharacterScript: playerCount = " + playerCount);
        characterIndex = 0;
        foreach (GameObject character in characters)
            character.SetActive(false);
        characters[characterIndex].SetActive(true);
        currentPlayer = 0;
        selections.Clear();
        UpdateUI();
    }

    public void NextCharacter()
    {
        characters[characterIndex].SetActive(false);
        characterIndex = (characterIndex + 1) % characters.Length;
        characters[characterIndex].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[characterIndex].SetActive(false);
        characterIndex = (characterIndex - 1 + characters.Length) % characters.Length;
        characters[characterIndex].SetActive(true);
    }

    public void ConfirmSelection()
    {
        string name = inputField.GetComponent<TMPro.TMP_InputField>().text;
        if (name.Length > 3)
        {
            selections.Add(new PlayerSelection { characterIndex = characterIndex, playerName = name });

            // Optionally disable the selected character for next players
            characters[characterIndex].SetActive(false);

            currentPlayer++;
            if (currentPlayer < playerCount)
            {
                // Prepare for next player
                characterIndex = 0;
                while (characterIndex < characters.Length && !characters[characterIndex].activeSelf)
                    characterIndex++;
                if (characterIndex == characters.Length) characterIndex = 0;
                if (characters[characterIndex] != null)
                    characters[characterIndex].SetActive(true);

                inputField.GetComponent<TMPro.TMP_InputField>().text = "";
                UpdateUI();
            }
            else
            {
                SaveSelections();
                StartCoroutine(sceneChanger.Delay("play", 0, "")); // You can pass more info if needed
            }
        }
        else
        {
            inputField.GetComponent<TMPro.TMP_InputField>().Select();
        }
    }

    void UpdateUI()
    {
        playerText.text = $"Player {currentPlayer + 1} of {playerCount}";
    }


    void SaveSelections()
    {
        // Save all selections to PlayerPrefs or a static class for use in the game scene
        for (int i = 0; i < selections.Count; i++)
        {
            PlayerPrefs.SetInt($"Player_{i}_Character", selections[i].characterIndex);
            PlayerPrefs.SetString($"Player_{i}_Name", selections[i].playerName);
        }
        PlayerPrefs.SetInt("HumanPlayerCount", selections.Count);
    }
}

public class PlayerSelection
{
    public int characterIndex;
    public string playerName;
}