using UnityEngine;
public class SideDetectScript : MonoBehaviour
{
    DiceRollScript diceRollScript;
    [SerializeField] private float landedVelocitySqrThreshold = 0.01f;

    void Awake()
    {
        diceRollScript = GetComponentInParent<DiceRollScript>();
    }

    private void OnTriggerStay(Collider sideCollider)
    {
        if (diceRollScript == null) return;

        var rb = diceRollScript.GetComponent<Rigidbody>();
        if (rb == null) return;

        if (rb.linearVelocity.sqrMagnitude <= landedVelocitySqrThreshold)
        {
            diceRollScript.isLanded = true;
            diceRollScript.diceFaceNum = gameObject.name;
        }
        else
        {
            diceRollScript.isLanded = false;
        }
    }
}