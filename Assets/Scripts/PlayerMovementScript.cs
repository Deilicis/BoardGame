using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{
    public int currentNode = 0;
    public float moveSpeed = 8f;
    public Vector3 startOffset = Vector3.zero;
    BoardPathScript board;
    bool isMoving = false;
    Animator animator; // Add reference
    public bool hasFinished = false;
    public float timeStarted; // When this player started
    public float timeFinished; // When this player finished
    public float timePlayed = 0f;
    public int totalDiceSum = 0;

    void Start()
    {
        board = FindObjectOfType<BoardPathScript>();
        transform.position = board.nodes[currentNode].position + startOffset;
        animator = GetComponent<Animator>();
        GameTurnManager.instance.RegisterPlayer(this);
        timeStarted = Time.time;
    }
    void Update()
    {
        if (!hasFinished)
            timePlayed += Time.deltaTime;
    }
    public void MoveSteps(int steps)
    {
        if (hasFinished) return; // Prevent finished players from moving

        var cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
            cameraController.SwitchToPlayer(transform);
        if (!isMoving)
            StartCoroutine(Move(steps));
    }

    IEnumerator Move(int steps)
    {
        isMoving = true;
        if (animator != null)
            animator.SetBool("walk", true);

        for (int i = 0; i < steps; i++)
        {
            currentNode++;
            if (currentNode >= board.nodes.Count)
            {
                currentNode = board.nodes.Count - 1;
                break;
            }

            Vector3 nextPos = board.nodes[currentNode].position;

            while (Vector3.Distance(transform.position, nextPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }

        isMoving = false;
        if (animator != null)
            animator.SetBool("walk", false);

        if (currentNode == board.nodes.Count - 1)
        {
            hasFinished = true;
            timeFinished = Time.time;
            GameTurnManager.instance.OnPlayerFinished(this);
            yield break;
        }

        GameTurnManager.instance.NextPlayerTurn();
        DiceRollScript dice = FindObjectOfType<DiceRollScript>();
        dice.ResetDice();
    }
    public void AddDiceValue(int value)
    {
        totalDiceSum += value;
    }

    public bool IsMoving() => isMoving;
}
