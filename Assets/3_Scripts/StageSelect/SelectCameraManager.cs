using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCameraManager : MonoBehaviour
{
    // ˆÚ“®Œn
    [SerializeField] private float chasePower;
    private Vector3 targetPosition;
    private Vector3 nextPosition;

    void Update()
    {
        nextPosition = transform.position;

        CameraMove();

        transform.position = nextPosition;
    }

    void CameraMove()
    {
        nextPosition.x += (targetPosition.x - nextPosition.x) * (chasePower * Time.deltaTime);
        nextPosition.y += (targetPosition.y - nextPosition.y) * (chasePower * Time.deltaTime);
    }

    // Setter
    public void SetPosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        transform.position = new(targetPosition.x, targetPosition.y, transform.position.z);
    }
    public void SetTargetPosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
    }

    // Getter
    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }
}
