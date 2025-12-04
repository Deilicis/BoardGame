using System.Collections;
using UnityEngine;

public class SetActiveButtonScript : MonoBehaviour
{
    [Tooltip("Assign the panels (e.g. MainMenu, ButtonsPanel, Settings) in the order you want them indexed.")]
    public GameObject[] panels;

    [Tooltip("If true, the GameObject this script is on will be deactivated when switching panels.")]
    public bool deactivateSelfOnSwitch = false;

    // Switch to a specific panel by index after a delay (seconds)
    public void SwitchToPanelAfterDelay(int index, float delay)
    {
        StartCoroutine(SwitchToPanelCoroutine(index, delay));
    }

    private IEnumerator SwitchToPanelCoroutine(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        SwitchToPanel(index);
    }

    // Immediately switch to the given panel index, disabling all others
    public void SwitchToPanel(int index)
    {
        if (panels == null || panels.Length == 0)
        {
            Debug.LogWarning("SetActiveButtonScript: no panels assigned.");
            return;
        }
        if (index < 0 || index >= panels.Length)
        {
            Debug.LogWarning($"SetActiveButtonScript: index {index} is out of range (0..{panels.Length - 1}).");
            return;
        }

        for (int i = 0; i < panels.Length; i++)
        {
            var p = panels[i];
            if (p != null)
                p.SetActive(i == index);
        }

        if (deactivateSelfOnSwitch)
            gameObject.SetActive(false);
    }

    // Convenience: toggle between two panels (useful for back/forward buttons)
    public void ToggleBetweenPanels(int indexA, int indexB)
    {
        if (panels == null || panels.Length == 0) return;
        if (indexA < 0 || indexA >= panels.Length || indexB < 0 || indexB >= panels.Length) return;
        var a = panels[indexA];
        var b = panels[indexB];
        if (a == null || b == null) return;

        bool aActive = a.activeSelf;
        a.SetActive(!aActive);
        b.SetActive(aActive);
    }

    public void ToggleBetweenPanelsAfterDelay(int indexA, int indexB, float delay)
    {
        StartCoroutine(ToggleBetweenPanelsCoroutine(indexA, indexB, delay));
    }

    private IEnumerator ToggleBetweenPanelsCoroutine(int indexA, int indexB, float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleBetweenPanels(indexA, indexB);
    }
}
