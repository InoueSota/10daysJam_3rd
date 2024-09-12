using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private SelectUiManager selectUiManager;
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;
    private bool isPushLeft;
    private bool isPushRight;
    private bool isPushUp;
    private bool isPushDown;

    // ���R���|�[�l���g�擾
    private S_Transition transition;
    private ClearData clearData;

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

    [Header("�e�[�}")]
    [SerializeField] private Text themeText;
    [SerializeField] private Text chapterText;
    [SerializeField] private string[] themeTitle;
    private string[] chapterTitle;

    void Start()
    {
        selectUiManager = GetComponent<SelectUiManager>();
        inputManager = GetComponent<InputManager>();
        clearData = new ClearData();
        clearData = clearData.LoadClearData(clearData);

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
        selectUiManager.Initialize((int)stageGateManagers[stageNumber].GetChapter());

        // �`���v�^�[�����񏉊���
        chapterTitle = new string[themeTitle.Length];
        for (int i = 1; i < themeTitle.Length + 1; i++)
        {
            chapterTitle[i - 1] = "�`���v�^�[" + i.ToString();
        }

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

        if (selectIntervalTimer <= 0f && (isPushLeft || isPushRight || isPushUp || isPushDown) && !transition.isTransNow)
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
                selectUiManager.StartTriangleRotate(true);
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
                selectUiManager.StartTriangleRotate(false);
            }
            else if (isPushUp || isPushDown)
            {
                // ���݂̃X�e�[�W�����Q�Ƃ��A�`���v�^�[���擾����
                // �u��n�v��I��
                if (stageNumber >= GlobalVariables.toCemeteryNumber)
                {
                    if (isPushUp)
                    {
                        stageNumber = stageMax - 1;
                    }
                    else if (isPushDown)
                    {
                        stageNumber = GlobalVariables.toDesertNumber;
                    }
                }
                // �u�����v��I��
                else if (stageNumber >= GlobalVariables.toDesertNumber)
                {
                    if (isPushUp)
                    {
                        stageNumber = GlobalVariables.toCemeteryNumber;
                    }
                    else if (isPushDown)
                    {
                        stageNumber = GlobalVariables.toCaveNumber;
                    }
                }
                // �u���A�v��I��
                else if (stageNumber >= GlobalVariables.toCaveNumber)
                {
                    if (isPushUp)
                    {
                        stageNumber = GlobalVariables.toDesertNumber;
                    }
                    else if (isPushDown)
                    {
                        stageNumber = 0;
                    }
                }
                // �u�����v��I��
                else
                {
                    if (isPushUp)
                    {
                        stageNumber = GlobalVariables.toCaveNumber;
                    }
                    else if (isPushDown)
                    {
                        stageNumber = 0;
                    }
                }
            }
            selectCameraManager.SetTargetPosition(stageGateManagers[stageNumber].transform.position.x);
            selectIntervalTimer = selectIntervalTime;
        }
    }
    void ChangeChapter()
    {
        themeText.text = themeTitle[(int)stageGateManagers[stageNumber].GetChapter()];
        chapterText.text = chapterTitle[(int)stageGateManagers[stageNumber].GetChapter()];
    }
    void ChangeScene()
    {
        if (isTriggerJump && !transition.isTransNow)
        {
            selectUiManager.StartCircle();
            GlobalVariables.isClear = false;
            GlobalVariables.selectStageNumber = stageNumber;
            transition.SetTransition(stageName[stageNumber]);
        }
        if (isTriggerCancel && !transition.isTransNow)
        {
            GlobalVariables.isClear = false;
            GlobalVariables.selectStageNumber = stageNumber;
            transition.SetTransition("TitleScene");
        }
    }
    void UiManager()
    {
        selectUiManager.ChangeByChapter((int)stageGateManagers[stageNumber].GetChapter());
        selectUiManager.SetTriangleColor(isPushLeft, isPushRight);
    }

    // Getter
    void GetInput()
    {
        isTriggerJump = false;
        isTriggerCancel = false;
        isPushLeft = false;
        isPushRight = false;
        isPushUp = false;
        isPushDown = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.CANCEL))
        {
            isTriggerCancel = true;
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
        if (inputManager.IsPush(InputManager.INPUTPATTERN.VERTICAL))
        {
            if (inputManager.ReturnInputValue(InputManager.INPUTPATTERN.VERTICAL) > 0f)
            {
                isPushUp = true;
            }
            else
            {
                isPushDown = true;
            }
        }
    }
}
