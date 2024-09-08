using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCameraManager : MonoBehaviour
{
    // �J�����̔����̉���
    private float cameraHalfSizeX;
    // ���̈ړ��� = �J�����̉���
    private float moveValue;

    // �ړ��n
    [SerializeField] private float chasePower;
    private float targetX;
    private Vector3 nextPosition;

    [Header("�v���C���[")]
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
