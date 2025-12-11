using UnityEngine;

public class FaceCameraScript : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        Vector3 dir = transform.position - Camera.main.transform.position;
        dir.y = 0;  // Keep upright
        transform.forward = dir;
    }
}

