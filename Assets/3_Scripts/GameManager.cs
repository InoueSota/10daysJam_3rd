using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ����
    private InputManager inputManager;
    private bool isTriggerReset;
    private bool isTriggerJump;

    // �t���O��
    private bool isStart;
    private bool isClear;

    // ����
    private float readyTimer;

    // ���O
    [Header("���O")]
    [SerializeField] private string thisStageName;
    [SerializeField] private string nextStageName;

    // UI
    [Header("UI")]
    [SerializeField] private GameObject groupClear;

    // �v���C���[
    [Header("�v���C���[")]
    [SerializeField] private PlayerManager playerManager;

    void Start()
    {
        inputManager = GetComponent<InputManager>();

        readyTimer = 3.25f;

        // ���O���
        GlobalVariables.retryStageName = thisStageName;
        GlobalVariables.nextStageName = nextStageName;

        // �O���[�o���ϐ��̏�����
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

                // �S�Ẵu���b�N���j�󂳂ꂽ���̔���
                if (allObjectManager.GetObjectType() == AllObjectManager.ObjectType.BLOCK && allObjectManager.GetIsActive())
                {
                    isFinish = false;
                    break;
                }

                // �S�ẴA�C�e�����j�󂳂ꂽ���̔���
                else if (allObjectManager.GetObjectType() == AllObjectManager.ObjectType.ITEM && allObjectManager.GetIsActive())
                {
                    isFinish = false;
                    break;
                }
            }

            // �N���A�t���O��true�ɂ���
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
            // �v���C���[������
            playerManager.Initialize();

            // �X�e�[�W�I�u�W�F�N�g������
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

            // �O���[�o���ϐ��̏�����
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
