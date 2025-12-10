using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform diceTarget;       // Dice bowl center
    public Transform currentPlayer;    // Player currently moving
    public Vector3 offset;             // Camera offset relative to target
    public float smoothSpeed = 5f;

    private enum CameraState { Dice, Player }
    private CameraState state = CameraState.Dice;

    void LateUpdate()
    {
        Transform target = state == CameraState.Dice ? diceTarget : currentPlayer;

        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        transform.LookAt(target);
    }

    public void SwitchToPlayer(Transform player)
    {
        currentPlayer = player;
        state = CameraState.Player;
    }

    public void SwitchToDice()
    {
        state = CameraState.Dice;
    }
}
