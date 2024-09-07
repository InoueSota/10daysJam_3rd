using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

                // 全てのブロックが破壊されたかの判定
                if (allObjectManager.GetObjectType() == AllObjectManager.ObjectType.BLOCK && allObjectManager.GetIsActive())
                {
                    isFinish = false;
                    break;
                }

                // 全てのアイテムが破壊されたかの判定
                else if (allObjectManager.GetObjectType() == AllObjectManager.ObjectType.ITEM && allObjectManager.GetIsActive())
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
                    case AllObjectManager.ObjectType.DRIPSTONEBLOCK:

                        obj.GetComponent<BlockManager>().SetIsActive(true);

                        break;
                    case AllObjectManager.ObjectType.DRIPSTONE:

                        obj.GetComponent<DripStoneManager>().SetIsActive(true);

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
