using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("ˆÚ“®")]
    [SerializeField] private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
        transform.position = startPosition;

        StartAnimation();
    }
    void StartAnimation()
    {
        transform.DOMove(targetPosition, 1f)
            .SetEase(Ease.OutCirc)
            .SetDelay(2f);
    }

    void Update()
    {

    }
}
