using UnityEngine;
using TMPro;

public class PlayerCountSelector : MonoBehaviour
{
    public int minPlayers = 2;
    public int maxPlayers = 4;
    public int currentCount = 2;
   

        public SetActiveButtonScript setActiveButtonScript; // Assign in inspector
        public int characterSelectPanelIndex; // Set this to the correct index

        public void SaveAndSwitchToCharacterSelect()
        {
            SavePlayerCount();
            setActiveButtonScript.SwitchToPanel(characterSelectPanelIndex);
        }
  
    public TMP_InputField inputField; // Assign in inspector

    void Start()
    {
        currentCount = Mathf.Clamp(currentCount, minPlayers, maxPlayers);
        UpdateInputField();
    }

    public void IncreaseCount()
    {
        if (currentCount < maxPlayers)
        {
            currentCount++;
            UpdateInputField();
        }
    }

    public void DecreaseCount()
    {
        if (currentCount > minPlayers)
        {
            currentCount--;
            UpdateInputField();
        }
    }

    void UpdateInputField()
    {
        if (inputField != null)
        {
            inputField.text = currentCount.ToString();
        }
    }

    // Call this when proceeding to character selection
    public void SavePlayerCount()
    {
        Debug.Log("Selected Player Count: " + currentCount);
        PlayerPrefs.SetInt("PlayerCount", currentCount);
        PlayerPrefs.Save();
    }
}