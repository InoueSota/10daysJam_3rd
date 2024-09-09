using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Transform stageNameParent;
    private Text[] stageNameTexts;

    [Header("�X�e�[�W�I���Ԋu�̎���")]
    [SerializeField] private float selectIntervalTime;
    private float selectIntervalTimer;

    [Header("�Q�[�g")]
    [SerializeField] private float stageGateSpace;
    [SerializeField] private Transform stageGateParent;
    private StageGateManager[] stageGateManagers;

    [Header("�J����")]
    [SerializeField] private SelectCameraManager selectCameraManager;

    [Header("�t���[��")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Color[] themeColor;
    [SerializeField] private float colorChasePower;
    private Color targetColor;

    [Header("�e�[�}")]
    [SerializeField] private Text themeText;
    [SerializeField] private string[] themeTitle;

    void Start()
    {
        inputManager = GetComponent<InputManager>();

        // �X�e�[�W�J�ڐ�Ɋւ�����̏�����
        stageName = new string[stageMax];
        stageGateManagers = new StageGateManager[stageMax];
        stageNameTexts = new Text[stageMax];
        for (int i = 1; i < stageMax + 1; i++)
        {
            // �Q�[�g�̎q�I�u�W�F�N�g�擾
            Transform stageGateTransform = stageGateParent.GetChild(i - 1).transform;
            stageGateTransform.position = new((i - 1) * stageGateSpace, stageGateTransform.position.y, stageGateTransform.position.z);
            stageGateManagers[i - 1] = stageGateTransform.GetComponent<StageGateManager>();

            // �e�L�X�g�̎q�I�u�W�F�N�g
            stageNameTexts[i - 1] = stageNameParent.GetChild(i - 1).GetComponent<Text>();
            stageNameTexts[i - 1].transform.position = new(stageGateTransform.position.x, stageGateTransform.position.y + 0.75f, stageGateTransform.position.z);

            // �X�e�[�W���擾
            stageName[i - 1] = "Stage" + i.ToString();
            stageNameTexts[i - 1].text = stageName[i - 1];
        }

        // GlobalVariables����ϐ����擾����
        stageNumber = GlobalVariables.selectStageNumber;

        // �Y����StageGate����eobj���C������
        selectCameraManager.SetPosition(stageGateManagers[stageNumber].transform.position);
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
            selectCameraManager.SetTargetPosition(stageGateManagers[stageNumber].transform.position);
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
        if (isTriggerJump)
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
