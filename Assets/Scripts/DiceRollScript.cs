using UnityEngine;

public class DiceRollScript : MonoBehaviour
{
    Rigidbody rBody;
    Vector3 startPosition;
    [SerializeField] private float maxRandForceVal = 10f;
    [SerializeField] private float startRollingForce = 1000f;
    float forceX, forceY, forceZ;
    private int dicePressCount = 0;
    public string diceFaceNum;
    public bool isLanded = false;
    public bool firstThrow = false;
    public bool hasProcessedResult = false;

    [Header("Click Settings")]
    [SerializeField] private Collider clickCollider; // assign your new SphereCollider here
    [SerializeField] private LayerMask clickLayer;   // layer that includes only the click collider

    void Awake()
    {
        startPosition = transform.position;
        rBody = GetComponent<Rigidbody>();
        rBody.isKinematic = true;

        if (clickCollider == null)
            Debug.LogWarning("Click Collider not assigned!");
    }

    private void RollDice()
    {
        rBody.isKinematic = false;
        forceX = Random.Range(0, maxRandForceVal);
        forceY = Random.Range(0, maxRandForceVal);
        forceZ = Random.Range(0, maxRandForceVal);

        rBody.AddForce(Vector3.up * Random.Range(800, startRollingForce));
        rBody.AddTorque(forceX, forceY, forceZ);
    }

    public void ResetDice()
    {
        transform.position = startPosition;
        firstThrow = false;
        isLanded = false;
        hasProcessedResult = false;
        rBody.linearVelocity = Vector3.zero;
        rBody.angularVelocity = Vector3.zero;
        rBody.isKinematic = true;
        dicePressCount = 0; // Reset press count
        var cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
            cameraController.SwitchToDice();
    }

    void Update()
    {
        // ---- CLICK DETECTION ----
        if (Input.GetMouseButtonDown(0) && clickCollider != null && dicePressCount < 3)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickLayer))
            {
                if (hit.collider == clickCollider)
                {
                    if (!firstThrow)
                        firstThrow = true;

                    RollDice();
                    hasProcessedResult = false;
                    dicePressCount++; // Increment press count
                }
            }
        }

        // ---- MOVE PLAYER AFTER DICE LANDS ----
        if (isLanded && !hasProcessedResult)
        {
            hasProcessedResult = true;

            if (GameTurnManager.instance != null && GameTurnManager.instance.GetCurrentPlayer() != null)
            {
                GameTurnManager.instance
                    .GetCurrentPlayer()
                    .GetComponent<PlayerMovementScript>()
                    .MoveSteps(int.Parse(diceFaceNum));
            }
        }
    }
}
