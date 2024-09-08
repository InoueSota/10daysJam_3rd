using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCameraManager : MonoBehaviour
{
    // カメラの半分の横幅
    private float cameraHalfSizeX;
    // 一回の移動量 = カメラの横幅
    private float moveValue;

    // 移動系
    [SerializeField] private float chasePower;
    private float targetX;
    private Vector3 nextPosition;

    [Header("プレイヤー")]
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        cameraHalfSizeX = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
        moveValue = cameraHalfSizeX * 2f;
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

        if (diffX >= cameraHalfSizeX)
        {
            targetX += moveValue;
        }
        else if (diffX <= -cameraHalfSizeX)
        {
            targetX -= moveValue;
        }
    }
    void CameraMove()
    {
        CheckCameraMove();

        nextPosition.x += (targetX - nextPosition.x) * (chasePower * Time.deltaTime);
    }
}
