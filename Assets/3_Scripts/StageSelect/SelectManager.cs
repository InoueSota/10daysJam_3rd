using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    // ����
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;
    private bool isPushLeft;
    private bool isPushRight;

    // ���R���|�[�l���g�擾
    S_Transition transition;

    [Header("�X�e�[�W�ő吔")]
    [SerializeField] private int stageMax;
    private int stageNumber;
    private string[] stageName;

    [Header("�X�e�[�W���e�L�X�g")]
    [SerializeField] private GameObject stageNumberPrefab;
    [SerializeField] private Transform stageNumberParent;

    [Header("�X�e�[�W���e�L�X�g")]
    [SerializeField] private GameObject stageNamePrefab;
    [SerializeField] private Transform stageNameParent;
    [SerializeField] private string[] stageNames;

    [Header("�X�e�[�W�C���[�W�摜")]
    [SerializeField] private GameObject stageImagePrefab;
    [SerializeField] private Transform stageImageParent;
    [SerializeField] private Sprite[] stageImageSprite;

    [Header("�X�e�[�W�I���Ԋu�̎���")]
    [SerializeField] private float selectIntervalTime;
    private float selectIntervalTimer;

    [Header("�Q�[�g")]
    [SerializeField] private float stageGateSpace;
    [SerializeField] private Transform stageGateParent;
    private StageGateManager[] stageGateManagers;

    [Header("�J����")]
    [SerializeField] private SelectCameraManager selectCameraManager;

    [Header("�㕔�t���[��")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Color[] themeColor;
    [SerializeField] private float colorChasePower;
    private Color targetColor;

    [Header("�e�[�}")]
    [SerializeField] private Text themeText;
    [SerializeField] private string[] themeTitle;

    [Header("UI")]
    [SerializeField] private Image leftTriangle;
    [SerializeField] private Image rightTriangle;
    [SerializeField] private Image backGround;
    [SerializeField] private Color[] backGroundColor;
    private Color backGroundTargetColor;

    void Start()
    {
        inputManager = GetComponent<InputManager>();

        // �X�e�[�W�J�ڐ�Ɋւ�����̏�����
        stageName = new string[stageMax];
        stageGateManagers = new StageGateManager[stageMax];
        for (int i = 1; i < stageMax + 1; i++)
        {
            // �Q�[�g�̎q�I�u�W�F�N�g�擾
            Transform stageGateTransform = stageGateParent.GetChild(i - 1).transform;
            stageGateTransform.position = new((i - 1) * stageGateSpace, stageGateTransform.position.y, stageGateTransform.position.z);
            stageGateManagers[i - 1] = stageGateTransform.GetComponent<StageGateManager>();

            // �e�L�X�g�̎q�I�u�W�F�N�g
            GameObject stageNumberText = Instantiate(stageNumberPrefab, new(stageGateTransform.position.x, stageGateTransform.position.y + 1f, stageGateTransform.position.z), Quaternion.identity);
            stageNumberText.transform.SetParent(stageNumberParent);
            GameObject stageNameText = Instantiate(stageNamePrefab, new(stageGateTransform.position.x, stageGateTransform.position.y, stageGateTransform.position.z), Quaternion.identity);
            stageNameText.transform.SetParent(stageNameParent);
            stageNameText.GetComponent<Text>().text = stageNames[i - 1];

            // �C���[�W�摜�̎q�I�u�W�F�N�g
            GameObject stageImage = Instantiate(stageImagePrefab, new(stageGateTransform.position.x, stageGateTransform.position.y - 3.5f, stageGateTransform.position.z), Quaternion.identity);
            stageImage.transform.SetParent(stageImageParent);
            stageImage.GetComponent<Image>().sprite = stageImageSprite[i - 1];

            // �X�e�[�W���擾
            stageName[i - 1] = "Stage" + i.ToString();
            stageNumberText.GetComponent<Text>().text = stageName[i - 1];
        }

        // GlobalVariables����ϐ����擾����
        stageNumber = GlobalVariables.selectStageNumber;

        // �Y����StageGate����eobj���C������
        selectCameraManager.SetPosition(stageGateManagers[stageNumber].transform.position.x);
        themeText.text = themeTitle[(int)stageGateManagers[stageNumber].GetChapter()];
        frameImage.color = themeColor[(int)stageGateManagers[stageNumber].GetChapter()];
        targetColor = themeColor[(int)stageGateManagers[stageNumber].GetChapter()];

        transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
    }

    void Update()
    {
        GetInput();

        ChangeSelectStage();
        ChangeScene();
        ChangeChapter();
        UiManager();
    }

    void ChangeSelectStage()
    {
        selectIntervalTimer -= Time.deltaTime;

        if (selectIntervalTimer <= 0f && (isPushLeft || isPushRight))
        {
            // �X�e�[�W�ԍ������Z����
            if (isPushLeft)
            {
                // ���łɍŏ��ԍ���I�����Ă�����A�ő�ԍ��ɂ���
                if (stageNumber == 0)
                {
                    stageNumber = stageMax - 1;
                }
                else
                {
                    stageNumber--;
                }
            }
            // �X�e�[�W�ԍ������Z����
            else if (isPushRight)
            {
                // ���łɍő�ԍ���I�����Ă�����A�ŏ��ԍ��ɂ���
                if (stageNumber == stageMax - 1)
                {
                    stageNumber = 0;
                }
                else
                {
                    stageNumber++;
                }
            }
            selectCameraManager.SetTargetPosition(stageGateManagers[stageNumber].transform.position.x);
            selectIntervalTimer = selectIntervalTime;
        }
    }
    void ChangeChapter()
    {
        themeText.text = themeTitle[(int)stageGateManagers[stageNumber].GetChapter()];
        targetColor = themeColor[(int)stageGateManagers[stageNumber].GetChapter()];
        frameImage.color += (targetColor - frameImage.color) * (colorChasePower * Time.deltaTime);
    }
    void ChangeScene()
    {
        if (isTriggerJump && !transition.isTransNow)
        {
            GlobalVariables.selectStageNumber = stageNumber;
            transition.SetTransition(stageName[stageNumber]);
        }
        if (isTriggerCancel)
        {
            GlobalVariables.selectStageNumber = stageNumber;
            transition.SetTransition("TitleScene");
        }
    }
    void UiManager()
    {
        leftTriangle.color = frameImage.color;
        rightTriangle.color = frameImage.color;
        backGroundTargetColor = backGroundColor[(int)stageGateManagers[stageNumber].GetChapter()];
        backGround.color += (backGroundTargetColor - backGround.color) * (colorChasePower * Time.deltaTime); ;

        if (!isPushLeft)
        {
            leftTriangle.color = new(frameImage.color.r, frameImage.color.g, frameImage.color.b, 0.5f);
        }
        if (!isPushRight)
        {
            rightTriangle.color = new(frameImage.color.r, frameImage.color.g, frameImage.color.b, 0.5f);
        }
    }

    // Getter
    void GetInput()
    {
        isTriggerJump = false;
        isTriggerCancel = false;
        isPushLeft = false;
        isPushRight = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
        if (inputManager.IsPush(InputManager.INPUTPATTERN.HORIZONTAL))
        {
            if (inputManager.ReturnInputValue(InputManager.INPUTPATTERN.HORIZONTAL) < 0f)
            {
                isPushLeft = true;
            }
            else
            {
                isPushRight = true;
            }
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.CANCEL))
        {
            isTriggerCancel = true;
        }
    }

}
