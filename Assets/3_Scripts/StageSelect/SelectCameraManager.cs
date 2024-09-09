using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCameraManager : MonoBehaviour
{
    // カメラの半分の幅
    private Vector2 cameraHalfSize;
    // 一回の移動量 = カメラの幅
    private Vector3 moveValue;

    // 移動系
    [SerializeField] private float chasePower;
    private Vector3 targetPosition;
    private Vector3 nextPosition;

    // 深さ
    private int depth;

    [Header("プレイヤー")]
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        cameraHalfSize = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height, 0f));
        moveValue.x = cameraHalfSize.x * 2f;
        moveValue.y = 11f;
    }

    void Update()
    {
        nextPosition = transform.position;

        CameraMove();

        transform.position = nextPosition;
    }

    void CheckCameraMove()
    {
        float diffX = playerTransform.position.x - transform.position.x;

        if (diffX >= cameraHalfSize.x)
        {
            targetPosition.x += moveValue.x;
        }
        else if (diffX <= -cameraHalfSize.x)
        {
            targetPosition.x -= moveValue.x;
        }

        float diffY = playerTransform.position.y - transform.position.y;

        if (diffY >= cameraHalfSize.y)
        {
            targetPosition.y += moveValue.y;
            depth--;
        }
        else if (diffY <= -cameraHalfSize.y)
        {
            targetPosition.y -= moveValue.y;
            depth++;
        }
    }
    void CameraMove()
    {
        CheckCameraMove();

        nextPosition.x += (targetPosition.x - nextPosition.x) * (chasePower * Time.deltaTime);
        nextPosition.y += (targetPosition.y - nextPosition.y) * (chasePower * Time.deltaTime);
    }

    // Setter
    public void SetTargetPosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        transform.position = new(targetPosition.x, targetPosition.y, transform.position.z);
    }
    public void SetDepth(int _depth)
    {
        depth = _depth;
    }

    // Getter
    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }
    public int GetDepth()
    {
        return depth;
    }
}
