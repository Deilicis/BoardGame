using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{
    public int currentNode = 0;
    public float moveSpeed = 8f;

    BoardPathScript board;
    bool isMoving = false;
    Animator animator; // Add reference

    void Start()
    {
        board = FindObjectOfType<BoardPathScript>();
        transform.position = board.nodes[currentNode].position;
        animator = GetComponent<Animator>(); // Get Animator
    }

    public void MoveSteps(int steps)
    {
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
            animator.SetBool("walk", true); // Start walk animation

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
            animator.SetBool("walk", false); // Stop walk animation

        GameTurnManager.instance.NextPlayerTurn();
        DiceRollScript dice = FindObjectOfType<DiceRollScript>();
        dice.ResetDice();
    }

    public bool IsMoving() => isMoving;
}
