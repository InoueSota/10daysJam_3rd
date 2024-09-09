using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    // 入力
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;
    private bool isPushLeft;
    private bool isPushRight;

    // 他コンポーネント取得
    S_Transition transition;

    [Header("ステージ最大数")]
    [SerializeField] private int stageMax;
    private int stageNumber;
    private string[] stageName;

    [Header("ステージ名テキスト")]
    [SerializeField] private Transform stageNameParent;
    private Text[] stageNameTexts;

    [Header("ステージ選択間隔の時間")]
    [SerializeField] private float selectIntervalTime;
    private float selectIntervalTimer;

    [Header("ゲート")]
    [SerializeField] private float stageGateSpace;
    [SerializeField] private Transform stageGateParent;
    private StageGateManager[] stageGateManagers;

    [Header("カメラ")]
    [SerializeField] private SelectCameraManager selectCameraManager;

    [Header("フレーム")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Color[] themeColor;
    [SerializeField] private float colorChasePower;
    private Color targetColor;

    [Header("テーマ")]
    [SerializeField] private Text themeText;
    [SerializeField] private string[] themeTitle;

    void Start()
    {
        inputManager = GetComponent<InputManager>();

        // ステージ遷移先に関する情報の初期化
        stageName = new string[stageMax];
        stageGateManagers = new StageGateManager[stageMax];
        stageNameTexts = new Text[stageMax];
        for (int i = 1; i < stageMax + 1; i++)
        {
            // ゲートの子オブジェクト取得
            Transform stageGateTransform = stageGateParent.GetChild(i - 1).transform;
            stageGateTransform.position = new((i - 1) * stageGateSpace, stageGateTransform.position.y, stageGateTransform.position.z);
            stageGateManagers[i - 1] = stageGateTransform.GetComponent<StageGateManager>();

            // テキストの子オブジェクト
            stageNameTexts[i - 1] = stageNameParent.GetChild(i - 1).GetComponent<Text>();
            stageNameTexts[i - 1].transform.position = new(stageGateTransform.position.x, stageGateTransform.position.y + 0.75f, stageGateTransform.position.z);

            // ステージ名取得
            stageName[i - 1] = "Stage" + i.ToString();
            stageNameTexts[i - 1].text = stageName[i - 1];
        }

        // GlobalVariablesから変数を取得する
        stageNumber = GlobalVariables.selectStageNumber;

        // 該当のStageGateから各objを修正する
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
            // ステージ番号を減算する
            if (isPushLeft)
            {
                // すでに最小番号を選択していたら、最大番号にする
                if (stageNumber == 0)
                {
                    stageNumber = stageMax - 1;
                }
                else
                {
                    stageNumber--;
                }
            }
            // ステージ番号を加算する
            else if (isPushRight)
            {
                // すでに最大番号を選択していたら、最小番号にする
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
