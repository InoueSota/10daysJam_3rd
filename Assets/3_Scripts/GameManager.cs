using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 自コンポーネント取得
    private StageObjectManager stageObjectManager;
    private MenuManager menuManager;
    private InputManager inputManager;
    private bool isTriggerCancel;
    private bool isTriggerReset;
    private bool isTriggerspecial;

    // 他コンポーネント取得
    private S_Transition transition;
    private ClearData clearData;

    // フラグ類
    private bool isStart;
    private bool isClear;

    // 時間
    private float readyTimer;

    [Header("名前")]
    [SerializeField] private string thisStageName;
    [SerializeField] private string nextStageName;

    [Header("UI")]
    [SerializeField] private GameObject groupClear;
    [SerializeField] private Text stageName;
    [SerializeField] private Text menuStageName;

    [Header("プレイヤー")]
    [SerializeField] private PlayerManager playerManager;

    [Header("リスタート演出")]
    [SerializeField] private GameObject restartPrefab;

    void Start()
    {
        stageObjectManager = GetComponent<StageObjectManager>();
        stageObjectManager.SetPlayerManager(playerManager);
        menuManager = GetComponent<MenuManager>();
        inputManager = GetComponent<InputManager>();
        clearData = new ClearData();
        clearData = clearData.LoadClearData(clearData);
        if (GameObject.FindWithTag("trans"))
        {
            transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
        }
        readyTimer = 3f;

        // 名前代入
        GlobalVariables.retryStageName = thisStageName;
        GlobalVariables.nextStageName = nextStageName;
        stageName.text = thisStageName;
        menuStageName.text = thisStageName;

        // グローバル変数の初期化
        GlobalVariables.isClear = false;
    }
    void DestroyOutOfCameraObj()
    {
        Vector2 cameraSize;
        cameraSize.x = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
        cameraSize.y = Camera.main.ScreenToWorldPoint(new(0f, Screen.height, 0f)).y;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            float xAbsValue = Mathf.Abs(obj.transform.position.x);
            float yAbsValue = Mathf.Abs(obj.transform.position.y);

            if (xAbsValue > cameraSize.x || yAbsValue > cameraSize.y)
            {
                Destroy(obj);
            }
        }
    }

    void Update()
    {
        GetInput();

        Menu();
        Ready();
        Clear();

        if (isTriggerCancel && isStart && !isClear)
        {
            if (menuManager.GetIsMenuActive())
            {
                SetPlayerAcitve(menuManager.GetIsMenuActive());
                menuManager.SetIsMenuActive();
            }
            Restart();
        }
    }

    void Menu()
    {
        // メニューを開く / 閉じる
        if (isTriggerReset && !isClear)
        {
            playerManager.SetIsActive(menuManager.GetIsMenuActive());
            if (isStart)
            {
                stageObjectManager.SetCanCheck(menuManager.GetIsMenuActive());
            }
            menuManager.SetIsMenuActive();
        }
    }
    void Ready()
    {
        if (!isStart)
        {
            readyTimer -= Time.deltaTime;
            if (readyTimer <= 0f)
            {
                // 画面外のオブジェクトを破壊する
                DestroyOutOfCameraObj();
                stageObjectManager.SetCanCheck(true);
                if (!menuManager.GetIsMenuActive())
                {
                    playerManager.SetIsActive(true);
                }
                stageObjectManager.Initialize();
                isStart = true;
            }
        }
    }
    void Clear()
    {
        if (!isClear)
        {
            bool isFinish = true;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                AllObjectManager allObjectManager = obj.GetComponent<AllObjectManager>();

                if (allObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND)
                {
                    // Ground以外の全てのオブジェクトが破壊されたかの判定
                    if (allObjectManager.GetObjectType() != AllObjectManager.ObjectType.ITEM && allObjectManager.GetIsActive())
                    {
                        isFinish = false;
                        break;
                    }
                    // アイテム判定
                    else if (allObjectManager.GetObjectType() == AllObjectManager.ObjectType.ITEM)
                    {
                        // まだステージにアイテムが残っている
                        if (allObjectManager.GetIsActive())
                        {
                            isFinish = false;
                            break;
                        }
                        // アイテムを破壊してしまっている
                        else if (allObjectManager.GetHp() <= 0)
                        {
                            isFinish = false;
                            break;
                        }
                    }
                }
            }


            // クリアフラグをtrueにする
            if (isFinish)
            {
                clearData.SetClearFlag(clearData);
                clearData.Save(clearData);
                groupClear.SetActive(true);
                GlobalVariables.isClear = true;
                isClear = true;
            }
        }
        else
        {
            if (isTriggerspecial && !transition.isTransNow)
            {
                //トランジション処理
                transition.SetTransition("SelectScene");
            }
        }
    }
    public void Restart()
    {
        // プレイヤー初期化
        playerManager.Initialize();

        // ステージオブジェクト初期化
        stageObjectManager.Initialize();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            AllObjectManager allObjectManager = obj.GetComponent<AllObjectManager>();

            switch (allObjectManager.GetObjectType())
            {
                case AllObjectManager.ObjectType.BLOCK:

                    obj.GetComponent<BlockManager>().SetIsActive(true);

                    break;
                case AllObjectManager.ObjectType.ITEM:

                    obj.GetComponent<ItemManager>().SetIsActive(true);

                    break;
                case AllObjectManager.ObjectType.GRASSPARENT:

                    obj.GetComponent<GrassParentScript>().SetIsActive(true);

                    break;
                case AllObjectManager.ObjectType.DRIPSTONE:

                    obj.GetComponent<DripStoneManager>().SetIsActive(true);

                    break;
                case AllObjectManager.ObjectType.BOMB:

                    obj.GetComponent<BombManager>().SetIsActive(true);

                    break;
                case AllObjectManager.ObjectType.SPLIT:

                    obj.GetComponent<SplitManager>().SetIsActive(true);

                    break;
                case AllObjectManager.ObjectType.CACTUS:

                    obj.GetComponent<CactusManager>().SetIsActive(true);

                    break;
                case AllObjectManager.ObjectType.ICICLE:

                    obj.GetComponent<IcicleManager>().SetIsActive(true);

                    break;
                case AllObjectManager.ObjectType.DEATHWARP:

                    obj.GetComponent<DeathWarpManager>().SetIsActive(true);

                    break;
            }
        }

        // エッフェル塔
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("effect"))
        {
            // 残ってるエフェクトを消す処理
            Destroy(obj);
        }
    }

    public void SetPlayerAcitve(bool _isActive)
    {
        playerManager.SetIsActive(_isActive);
    }

    void GetInput()
    {
        isTriggerReset = false;
        isTriggerspecial = false;
        isTriggerCancel = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.RESET))
        {
            isTriggerReset = true;
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.SPECIAL))
        {
            isTriggerspecial = true;
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.CANCEL))
        {
            isTriggerCancel = true;
        }
    }
    public bool GetIsClear()
    {
        return isClear;
    }
}
