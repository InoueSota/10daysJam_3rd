using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectUiManager : MonoBehaviour
{
    [Header("上部フレーム")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Color[] themeColor;
    [SerializeField] private float colorChasePower;
    private Color targetColor;

    [Header("三角")]
    [SerializeField] private RectTransform leftTriangle;
    private Image leftTriangleImage;
    [SerializeField] private RectTransform rightTriangle;
    private Image rightTriangleImage;

    [Header("背景")]
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Color[] backGroundColor;
    private Color backGroundTargetColor;

    [Header("浮動管理対象")]
    [SerializeField] private Transform groupA;
    [SerializeField] private Transform stageNumber;
    [SerializeField] private Transform stageName;
    [SerializeField] private Animator startCircleAnimator;

    // 元座標
    private Vector3 groupAOrigin;
    private Vector3 leftTriangleOrigin;
    private Vector3 rightTriangleOrigin;
    private Vector3 stageNumberOrigin;
    private Vector3 stageNameOrigin;

    [Header("sin揺れ")]
    [SerializeField] private float range;
    [SerializeField] private float flowSpeed;
    private float angle;

    [Header("揺らしのずれ")]
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
        // 各オブジェクトの自コンポーネント取得
        leftTriangleImage = leftTriangle.GetComponent<Image>();
        rightTriangleImage = rightTriangle.GetComponent<Image>();

        // Origin
        groupAOrigin = groupA.position;
        leftTriangleOrigin = leftTriangle.position;
        rightTriangleOrigin = rightTriangle.position;
        stageNumberOrigin = stageNumber.position;
        stageNameOrigin = stageName.position;

        // sin運動フワフワ初期化
        angle = 0;

        // Random
        leftTriangleRandom = Random.Range(leftTriangleRange.x, leftTriangleRange.y);
        rightTriangleRandom = Random.Range(rightTriangleRange.x, rightTriangleRange.y);
        stageNumberRandom = Random.Range(stageNumberRange.x, stageNumberRange.y);
        stageNameRandom = Random.Range(stageNameRange.x, stageNameRange.y);
    }
    public void Initialize(int _chapterNumber)
    {
        frameImage.color = themeColor[_chapterNumber];
        targetColor = themeColor[_chapterNumber];
    }

    void FixedUpdate()
    {
        angle += flowSpeed;

        groupA.position = new Vector3(groupA.position.x, groupAOrigin.y + (Mathf.Sin(angle) * range), groupAOrigin.z);
        leftTriangle.position = new Vector3(leftTriangle.position.x, leftTriangleOrigin.y + (Mathf.Sin(angle + flowSpeed * leftTriangleRandom) * range), leftTriangleOrigin.z);
        rightTriangle.position = new Vector3(rightTriangle.position.x, rightTriangleOrigin.y + (Mathf.Sin(angle + flowSpeed * rightTriangleRandom) * range), rightTriangleOrigin.z);
        stageNumber.position = new Vector3(stageNumber.position.x, stageNumberOrigin.y + (Mathf.Sin(angle + flowSpeed * stageNumberRandom) * range), stageNumberOrigin.z);
        stageName.position = new Vector3(stageName.position.x, stageNameOrigin.y + (Mathf.Sin(angle + flowSpeed * stageNameRandom) * range), stageNameOrigin.z);
    }

    // Setter
    public void ChangeByChapter(int _chapterNumber)
    {
        backGroundTargetColor = backGroundColor[_chapterNumber];
        backGroundImage.color += (backGroundTargetColor - backGroundImage.color) * (colorChasePower * Time.deltaTime);
        targetColor = themeColor[_chapterNumber];
        frameImage.color += (targetColor - frameImage.color) * (colorChasePower * Time.deltaTime);
    }
    public void SetTriangleColor(bool _isPushLeft, bool _isPushRight)
    {
        leftTriangleImage.color = frameImage.color;
        rightTriangleImage.color = frameImage.color;

        if (!_isPushLeft)
        {
            leftTriangleImage.color = new(frameImage.color.r, frameImage.color.g, frameImage.color.b, 0.5f);
        }
        if (!_isPushRight)
        {
            rightTriangleImage.color = new(frameImage.color.r, frameImage.color.g, frameImage.color.b, 0.5f);
        }
    }
    public void StartTriangleRotate(bool _isLeft)
    {
        if (_isLeft)
        {
            leftTriangle.DORotate(Vector3.forward * 360f, 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo);
        }
        else
        {
            rightTriangle.DORotate(Vector3.forward * -360f, 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo);
        }
    }
    public void StartCircle()
    {
        startCircleAnimator.SetTrigger("Start");
    }
}
