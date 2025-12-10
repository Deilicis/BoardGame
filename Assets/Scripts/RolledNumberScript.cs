using UnityEngine;
using UnityEngine.UI;

public class RolledNumberScript : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign the DiceRollScript if it's on a different GameObject. Otherwise the script will try to find one automatically.")]
    [SerializeField] private DiceRollScript diceRollScript;

    [SerializeField]
    public Text rolledNumberText;

    void Awake()
    {
        // Allow manual assignment in Inspector; otherwise try several fallbacks (only once)
        if (diceRollScript == null)
        {
            diceRollScript = GetComponent<DiceRollScript>()
                             ?? GetComponentInParent<DiceRollScript>()
                             ?? GetComponentInChildren<DiceRollScript>()
                             ?? FindObjectOfType<DiceRollScript>();
        }

        if (diceRollScript == null)
        {
            Debug.LogWarning($"RolledNumberScript on '{gameObject.name}' cannot find a DiceRollScript. " +
                "Either add DiceRollScript to the same GameObject, assign it in the Inspector, " +
                "or ensure a parent/child contains the component.");
        }

        if (rolledNumberText == null)
        {
            Debug.LogWarning($"RolledNumberScript on '{gameObject.name}' has no Text assigned to 'rolledNumberText'.");
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

        rolledNumberText.text = diceRollScript.isLanded ? diceRollScript.diceFaceNum : "?";
    }
}