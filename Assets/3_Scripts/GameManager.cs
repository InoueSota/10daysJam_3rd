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
                // 画面外のオブジェクトを破壊する
                DestroyOutOfCameraObj();
                stageObjectManager.SetCanCheck(true);
                stageObjectManager.Initialize();
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
                    case AllObjectManager.ObjectType.ICICLE:

                        obj.GetComponent<IcicleManager>().SetIsActive(true);

                        break;
                }
            }

            // グローバル変数の初期化
            GlobalVariables.isClear = false;
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
