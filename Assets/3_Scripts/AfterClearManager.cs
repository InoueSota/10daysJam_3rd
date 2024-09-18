using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AfterClearManager : MonoBehaviour
{
    // �t���O
    private bool isActive;
    private bool isActiveNext;
    [SerializeField] private bool isLastStage;

    // ���R���|�[�l���g�擾
    private InputManager inputManager;

    // ����
    private bool isTriggerDecide;
    private bool isTriggerUp;
    private bool isTriggerDown;

    // ���R���|�[�l���g�擾
    private S_Transition transition;

    [Header("�ω����x")]
    [SerializeField] private float chasePower;
    [Header("�R���e���c�̈ړ���")]
    [SerializeField] private float contentsDiff;

    [Header("�Ǘ��Ώ�")]
    [SerializeField] private GameObject clearObj;
    private Image clearImage;
    private Color clearImageColor;
    [SerializeField] private Image backImage;
    [SerializeField] private Color backTargetColor;
    [SerializeField] private RectTransform headerRect;
    private Vector3 headerTarget;
    [SerializeField] private RectTransform stageNumberRect;
    [SerializeField] private Text stageNumber;
    private Vector3 stageNumberTarget;
    [SerializeField] private Text nextStage;
    [SerializeField] private RectTransform nextStageRect;
    private Vector3 nextStageTarget;
    [SerializeField] private Image nextStageImage;
    [SerializeField] private RectTransform contentsRect;
    private Vector3 contentsTarget;
    [SerializeField] private RectTransform retryRect;
    private Vector3 retryOrigin;
    private Vector3 retryTarget;
    [SerializeField] private RectTransform nextRect;
    private Vector3 nextOrigin;
    private Vector3 nextTarget;
    [SerializeField] private RectTransform selectRect;
    private Vector3 selectOrigin;
    private Vector3 selectTarget;

    // �I����
    private enum Choices
    {
        RETRY,
        NEXTSTAGE,
        STAGESELECT
    }
    private Choices choices = Choices.NEXTSTAGE;

    public void Initialize()
    {
        isActive = true;
        isActiveNext = false;

        // �R���|�[�l���g�擾
        inputManager = GetComponent<InputManager>();
        if (GameObject.FindWithTag("trans"))
        {
            transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
        }

        clearObj.SetActive(true);
        if (!isLastStage)
        {
            // �Ǘ��Ώ�
            clearImage = clearObj.GetComponent<Image>();
            clearImageColor = clearImage.GetComponent<Image>().color;
            headerTarget = headerRect.localPosition;
            stageNumberTarget = stageNumberRect.localPosition;
            stageNumber.text = GlobalVariables.nextStageName;
            nextStage.text = GlobalVariables.stageTitle[GlobalVariables.selectStageNumber + 1];
            nextStage.color = new(1f, 1f, 1f, 0f);
            nextStageTarget = nextStageRect.localPosition;
            nextStageImage.sprite = GlobalVariables.stageSprite[GlobalVariables.selectStageNumber + 1];
            nextStageImage.color = new(1f, 1f, 1f, 0f);
            nextStageImage.GetComponent<StageImageManager>().Initialize(GlobalVariables.isClear[GlobalVariables.selectStageNumber + 1], GlobalVariables.stageDifficulty[GlobalVariables.selectStageNumber + 1], GlobalVariables.stageChapterNumber[GlobalVariables.selectStageNumber + 1]);
            contentsTarget = contentsRect.localPosition;
            retryOrigin = retryRect.localPosition;
            nextOrigin = nextRect.localPosition;
            selectOrigin = selectRect.localPosition;
            retryTarget = retryOrigin;
            nextTarget = nextOrigin;
            selectTarget = selectOrigin;

            // �I�����̏����l�� NEXTSTAGE �ɂ���
            choices = Choices.NEXTSTAGE;
        }
    }

    void Update()
    {
        if (isActive)
        {
            GetInput();

            if (isActiveNext)
            {
                switch (choices)
                {
                    case Choices.RETRY:

                        // �I��ύX
                        if (isTriggerUp)
                        {
                            ToSelectInitialize();
                            choices = Choices.STAGESELECT;
                        }
                        else if (isTriggerDown)
                        {
                            ToNextInitialize();
                            choices = Choices.NEXTSTAGE;
                        }

                        // �J��
                        if (isTriggerDecide && !transition.isTransNow)
                        {
                            transition.SetTransition(SceneManager.GetActiveScene().name);
                        }

                        break;
                    case Choices.NEXTSTAGE:

                        // �I��ύX
                        if (isTriggerUp)
                        {
                            ToRetryInitialize();
                            choices = Choices.RETRY;
                        }
                        else if (isTriggerDown)
                        {
                            ToSelectInitialize();
                            choices = Choices.STAGESELECT;
                        }

                        // �J��
                        if (isTriggerDecide && !transition.isTransNow)
                        {
                            GlobalVariables.selectStageNumber++;
                            transition.SetTransition(GlobalVariables.nextStageName);
                        }

                        break;
                    case Choices.STAGESELECT:

                        // �I��ύX
                        if (isTriggerUp)
                        {
                            ToNextInitialize();
                            choices = Choices.NEXTSTAGE;
                        }
                        else if (isTriggerDown)
                        {
                            ToRetryInitialize();
                            choices = Choices.RETRY;
                        }

                        // �J��
                        if (isTriggerDecide && !transition.isTransNow)
                        {
                            transition.SetTransition("SelectScene");
                        }

                        break;
                }

                // �f���^�^�C���Ή�
                float deltaChasePower = chasePower * Time.deltaTime;

                // �ω�
                clearImage.color += (clearImageColor - clearImage.color) * deltaChasePower;
                backImage.color += (backTargetColor - backImage.color) * deltaChasePower;
                headerRect.localPosition += (headerTarget - headerRect.localPosition) * deltaChasePower;
                stageNumberRect.localPosition += (stageNumberTarget - stageNumberRect.localPosition) * deltaChasePower;
                nextStage.color += (Color.white - nextStage.color) * deltaChasePower;
                nextStageRect.localPosition += (nextStageTarget - nextStageRect.localPosition) * deltaChasePower;
                nextStageImage.color += (Color.white - nextStageImage.color) * deltaChasePower;
                contentsRect.localPosition += (contentsTarget - contentsRect.localPosition) * deltaChasePower;
                retryRect.localPosition += (retryTarget - retryRect.localPosition) * deltaChasePower;
                nextRect.localPosition += (nextTarget - nextRect.localPosition) * deltaChasePower;
                selectRect.localPosition += (selectTarget - selectRect.localPosition) * deltaChasePower;
            }

            if (!isLastStage && !isActiveNext && isTriggerDecide)
            {
                clearImageColor.a = 0f;
                headerTarget.y = 320f;
                stageNumberTarget.x = -480f;
                nextStageTarget.y = 0f;
                contentsTarget.x = 400f;
                nextTarget.x = nextOrigin.x - contentsDiff;

                isActiveNext = true;
            }
            else if (isLastStage && isTriggerDecide && !transition.isTransNow)
            {
                // �J��
                transition.SetTransition("SelectScene");
            }
        }
    }
    void ToRetryInitialize()
    {
        retryTarget.x = retryOrigin.x - contentsDiff;
        nextTarget.x = nextOrigin.x;
        selectTarget.x = selectOrigin.x;
    }
    void ToNextInitialize()
    {
        retryTarget.x = retryOrigin.x;
        nextTarget.x = nextOrigin.x - contentsDiff;
        selectTarget.x = selectOrigin.x;
    }
    void ToSelectInitialize()
    {
        retryTarget.x = retryOrigin.x;
        selectTarget.x = selectOrigin.x - contentsDiff;
        nextTarget.x = nextOrigin.x;
    }

    void GetInput()
    {
        isTriggerDecide = false;
        isTriggerUp = false;
        isTriggerDown = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerDecide = true;
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.VERTICAL))
        {
            if (inputManager.ReturnInputValue(InputManager.INPUTPATTERN.VERTICAL) > 0f)
            {
                isTriggerUp = true;
            }
            else
            {
                isTriggerDown = true;
            }
        }
    }
}
