using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 自コンポーネント取得
    private StageObjectManager stageObjectManager;

    // 入力
    private InputManager inputManager;
    private bool isTriggerReset;
    private bool isTriggerJump;

    // フラグ類
    private bool isStart;
    private bool isClear;

    // 時間
    private float readyTimer;

    // 名前
    [Header("名前")]
    [SerializeField] private string thisStageName;
    [SerializeField] private string nextStageName;

    // UI
    [Header("UI")]
    [SerializeField] private GameObject groupClear;

    // プレイヤー
    [Header("プレイヤー")]
    [SerializeField] private PlayerManager playerManager;

    void Start()
    {
        stageObjectManager = GetComponent<StageObjectManager>();
        stageObjectManager.SetPlayerManager(playerManager);
        inputManager = GetComponent<InputManager>();

        readyTimer = 3.25f;

        // 名前代入
        GlobalVariables.retryStageName = thisStageName;
        GlobalVariables.nextStageName = nextStageName;

        // グローバル変数の初期化
        GlobalVariables.isClear = false;
        GlobalVariables.isGetItem1 = false;
        GlobalVariables.isGetItem2 = false;
    }

    void Update()
    {
        GetInput();

        Ready();
        Clear();

        Restart();
    }

    void Ready()
    {
        if (!isStart)
        {
            readyTimer -= Time.deltaTime;
            if (readyTimer <= 0f)
            {
                stageObjectManager.SetCanCheck(true);
                playerManager.SetIsActive(true);
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

                // Ground以外の全てのオブジェクトが破壊されたかの判定
                if (allObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND && allObjectManager.GetIsActive())
                {
                    isFinish = false;
                    break;
                }
            }

            // クリアフラグをtrueにする
            if (isFinish)
            {
                groupClear.SetActive(true);
                GlobalVariables.isClear = true;
                isClear = true;
            }
        }
        else
        {
            if (isTriggerJump)
            {
                SceneManager.LoadScene("SelectScene");
            }
        }
    }
    void Restart()
    {
        if (isTriggerReset && isStart && !isClear)
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
                    case AllObjectManager.ObjectType.DRIPSTONEBLOCK:
                    case AllObjectManager.ObjectType.ICICLEBLOCK:

                        obj.GetComponent<BlockManager>().SetIsActive(true);

                        break;
                    case AllObjectManager.ObjectType.ITEM:

                        obj.GetComponent<ItemManager>().SetIsActive(true);

                        break;
                    case AllObjectManager.ObjectType.DRIPSTONE:

                        obj.GetComponent<DripStoneManager>().SetIsActive(true);

                        break;
                    case AllObjectManager.ObjectType.BOMB:

                        obj.GetComponent<BombManager>().SetIsActive(true);

                        break;
                    case AllObjectManager.ObjectType.ICICLE:

                        obj.GetComponent<IcicleManager>().SetIsActive(true);

                        break;
                            case AllObjectManager.ObjectType.GRASSPARENT:

                        obj.GetComponent<GrassParentScript>().SetIsActive(true);

                        break;
                }
            }

            // グローバル変数の初期化
            GlobalVariables.isClear = false;
            GlobalVariables.isGetItem1 = false;
            GlobalVariables.isGetItem2 = false;
        }
    }

    void GetInput()
    {
        isTriggerReset = false;
        isTriggerJump = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.RESET))
        {
            isTriggerReset = true;
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
    }
}
