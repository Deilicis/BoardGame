using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public SaveLoadScript saveLoadScript;
    public FadeScript fadeScript;

    public void CloseGame()
    {

        StartCoroutine(Delay("quit", -1, ""));
    }

    public IEnumerator Delay(string command, int characterIndex, string characterName)
    {
        if (string.Equals(command, "quit", System.StringComparison.OrdinalIgnoreCase))
        {
            if (fadeScript != null)
                yield return fadeScript.FadeOut(0.1f);
            else
                Debug.LogWarning("FadeScript is not assigned in SceneChanger!");

            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
        else if (string.Equals(command, "play", System.StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeOut(0.01f);
            saveLoadScript.SaveGame(characterIndex, characterName);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (string.Equals(command, "menu", System.StringComparison.OrdinalIgnoreCase))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public void GoToMenu()
    {
               StartCoroutine(Delay("menu", -1, ""));
    }
}
