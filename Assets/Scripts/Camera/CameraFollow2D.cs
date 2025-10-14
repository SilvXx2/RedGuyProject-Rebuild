using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target a seguir")]
    public Transform target;

    [Header("Ajustes de cámara")]
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 1f, -10f);

    [Header("Límites opcionales")]
    public bool useLimits = false;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        if (useLimits)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minPosition.x, maxPosition.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minPosition.y, maxPosition.y);
        }

        transform.position = smoothedPosition;
    }
}
