using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SelectUiManager : MonoBehaviour
{
    [Header("sinóhÇÍ")]
    [SerializeField] private float range;
    [SerializeField] private float flowSpeed;
    private float angle;

    [Header("óhÇÁÇ∑ëŒè€")]
    [SerializeField] private Transform groupA;
    [SerializeField] private Transform leftTriangle;
    [SerializeField] private Transform rightTriangle;
    [SerializeField] private Transform stageNumber;
    [SerializeField] private Transform stageName;
    private Vector3 groupAOrigin;
    private Vector3 leftTriangleOrigin;
    private Vector3 rightTriangleOrigin;
    private Vector3 stageNumberOrigin;
    private Vector3 stageNameOrigin;

    [Header("óhÇÁÇµÇÃÇ∏ÇÍ")]
    [SerializeField] private Vector2 leftTriangleRange;
    private float leftTriangleRandom;
    [SerializeField] private Vector2 rightTriangleRange;
    private float rightTriangleRandom;
    [SerializeField] private Vector2 stageNumberRange;
    private float stageNumberRandom;
    [SerializeField] private Vector2 stageNameRange;
    private float stageNameRandom;

    void Start()
    {
        // sinâ^ìÆÉtÉèÉtÉèèâä˙âª
        angle = 0;

        // Origin
        groupAOrigin = groupA.position;
        leftTriangleOrigin = leftTriangle.position;
        rightTriangleOrigin = rightTriangle.position;
        stageNumberOrigin = stageNumber.position;
        stageNameOrigin = stageName.position;

        // Random
        leftTriangleRandom = Random.Range(leftTriangleRange.x, leftTriangleRange.y);
        rightTriangleRandom = Random.Range(rightTriangleRange.x, rightTriangleRange.y);
        stageNumberRandom = Random.Range(stageNumberRange.x, stageNumberRange.y);
        stageNameRandom = Random.Range(stageNameRange.x, stageNameRange.y);
    }

    void FixedUpdate()
    {
        angle += flowSpeed;

        groupA.position = new Vector3(groupA.position.x, groupAOrigin.y + (MathF.Sin(angle) * range), groupAOrigin.z);
        leftTriangle.position = new Vector3(leftTriangle.position.x, leftTriangleOrigin.y + (MathF.Sin(angle + flowSpeed * leftTriangleRandom) * range), leftTriangleOrigin.z);
        rightTriangle.position = new Vector3(rightTriangle.position.x, rightTriangleOrigin.y + (MathF.Sin(angle + flowSpeed * rightTriangleRandom) * range), rightTriangleOrigin.z);
        stageNumber.position = new Vector3(stageNumber.position.x, stageNumberOrigin.y + (MathF.Sin(angle + flowSpeed * stageNumberRandom) * range), stageNumberOrigin.z);
        stageName.position = new Vector3(stageName.position.x, stageNameOrigin.y + (MathF.Sin(angle + flowSpeed * stageNameRandom) * range), stageNameOrigin.z);
    }
}
