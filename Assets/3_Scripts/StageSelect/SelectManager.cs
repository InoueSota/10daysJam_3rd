using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    // 自コンポーネント取得
    private SelectUiManager selectUiManager;
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;
    private bool isPushLeft;
    private bool isPushRight;
    private bool isPushUp;
    private bool isPushDown;

    // 他コンポーネント取得
    private S_Transition transition;
    private ClearData clearData;

    [Header("ステージ最大数")]
    [SerializeField] private int stageMax;
    private int stageNumber;
    private string[] stageName;

    [Header("ステージ数テキスト")]
    [SerializeField] private GameObject stageNumberPrefab;
    [SerializeField] private Transform stageNumberParent;

    [Header("ステージ名テキスト")]
    [SerializeField] private GameObject stageNamePrefab;
    [SerializeField] private Transform stageNameParent;
    [SerializeField] private string[] stageNames;

    [Header("ステージイメージ画像")]
    [SerializeField] private GameObject stageImagePrefab;
    [SerializeField] private Transform stageImageParent;
    [SerializeField] private Sprite[] stageImageSprite;

    [Header("ステージ選択間隔の時間")]
    [SerializeField] private float selectIntervalTime;
    private float selectIntervalTimer;

    [Header("ゲート")]
    [SerializeField] private float stageGateSpace;
    [SerializeField] private Transform stageGateParent;
    private StageGateManager[] stageGateManagers;

    [Header("カメラ")]
    [SerializeField] private SelectCameraManager selectCameraManager;

    [Header("テーマ")]
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

        // ステージ遷移先に関する情報の初期化
        stageName = new string[stageMax];
        stageGateManagers = new StageGateManager[stageMax];
        for (int i = 1; i < stageMax + 1; i++)
        {
            // ゲートの子オブジェクト取得
            Transform stageGateTransform = stageGateParent.GetChild(i - 1).transform;
            stageGateTransform.position = new((i - 1) * stageGateSpace, stageGateTransform.position.y, stageGateTransform.position.z);
            stageGateManagers[i - 1] = stageGateTransform.GetComponent<StageGateManager>();

            // テキストの子オブジェクト
            GameObject stageNumberText = Instantiate(stageNumberPrefab, new(stageGateTransform.position.x, stageGateTransform.position.y + 1f, stageGateTransform.position.z), Quaternion.identity);
            stageNumberText.transform.SetParent(stageNumberParent);
            GameObject stageNameText = Instantiate(stageNamePrefab, new(stageGateTransform.position.x, stageGateTransform.position.y, stageGateTransform.position.z), Quaternion.identity);
            stageNameText.transform.SetParent(stageNameParent);
            stageNameText.GetComponent<Text>().text = stageNames[i - 1];

            // イメージ画像の子オブジェクト
            GameObject stageImage = Instantiate(stageImagePrefab, new(stageGateTransform.position.x, stageGateTransform.position.y - 3.5f, stageGateTransform.position.z), Quaternion.identity);
            stageImage.transform.SetParent(stageImageParent);
            stageImage.GetComponent<Image>().sprite = stageImageSprite[i - 1];

            // ステージ名取得
            stageName[i - 1] = "Stage" + i.ToString();
            stageNumberText.GetComponent<Text>().text = stageName[i - 1];
        }

        // GlobalVariablesから変数を取得する
        stageNumber = GlobalVariables.selectStageNumber;

        // 該当のStageGateから各objを修正する
        selectCameraManager.SetPosition(stageGateManagers[stageNumber].transform.position.x);
        themeText.text = themeTitle[(int)stageGateManagers[stageNumber].GetChapter()];
        selectUiManager.Initialize((int)stageGateManagers[stageNumber].GetChapter());

        // チャプター文字列初期化
        chapterTitle = new string[themeTitle.Length];
        for (int i = 1; i < themeTitle.Length + 1; i++)
        {
            chapterTitle[i - 1] = "チャプター" + i.ToString();
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
                selectUiManager.StartTriangleRotate(true);
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
                selectUiManager.StartTriangleRotate(false);
            }
            else if (isPushUp || isPushDown)
            {
                // 現在のステージ数を参照し、チャプターを取得する
                // 「墓地」を選択
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
                // 「砂漠」を選択
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
                // 「洞窟」を選択
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
                // 「草原」を選択
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
