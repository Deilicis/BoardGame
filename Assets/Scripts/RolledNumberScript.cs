using UnityEngine;
using UnityEngine.UI;

public class RolledNumberScript : MonoBehaviour
{
    [SerializeField] private DiceRollScript diceRollScript; // assign in Inspector when possible
    [SerializeField] private Text rolledNumberText;

    void Awake()
    {
        // If not assigned in inspector, try common lookups
        if (diceRollScript == null)
        {
            diceRollScript = GetComponent<DiceRollScript>();
        }
        if (diceRollScript == null)
        {
            diceRollScript = GetComponentInParent<DiceRollScript>();
        }
        if (diceRollScript == null)
        {
            diceRollScript = GetComponentInChildren<DiceRollScript>();
        }
        if (diceRollScript == null)
        {
            diceRollScript = FindObjectOfType<DiceRollScript>();
        }

        if (diceRollScript == null)
        {
            Debug.LogWarning("DiceRollScript not found. Assign it in the inspector or place this script on the same GameObject as the dice.");
        }

        if (rolledNumberText == null)
        {
            Debug.LogWarning("rolledNumberText not assigned in inspector.");
        }
    }

    private void Update()
    {
        if (rolledNumberText == null) return;

        if (diceRollScript == null)
        {
            rolledNumberText.text = "?";
            return;
        }

        rolledNumberText.text = diceRollScript.isLanded ? (diceRollScript.diceFaceNum ?? "?") : "?";
    }
}
