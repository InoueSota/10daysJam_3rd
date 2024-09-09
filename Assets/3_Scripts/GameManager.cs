using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private StageObjectManager stageObjectManager;

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
        stageObjectManager = GetComponent<StageObjectManager>();
        stageObjectManager.SetPlayerManager(playerManager);
        inputManager = GetComponent<InputManager>();

        readyTimer = 3.25f;

        // ���O���
        GlobalVariables.retryStageName = thisStageName;
        GlobalVariables.nextStageName = nextStageName;

        // �O���[�o���ϐ��̏�����
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
                // ��ʊO�̃I�u�W�F�N�g��j�󂷂�
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
                    // Ground�ȊO�̑S�ẴI�u�W�F�N�g���j�󂳂ꂽ���̔���
                    if (allObjectManager.GetObjectType() != AllObjectManager.ObjectType.ITEM && allObjectManager.GetIsActive())
                    {
                        isFinish = false;
                        break;
                    }
                    // �A�C�e������
                    else if (allObjectManager.GetObjectType() == AllObjectManager.ObjectType.ITEM)
                    {
                        // �܂��X�e�[�W�ɃA�C�e�����c���Ă���
                        if (allObjectManager.GetIsActive())
                        {
                            isFinish = false;
                            break;
                        }
                        // �A�C�e����j�󂵂Ă��܂��Ă���
                        else if (allObjectManager.GetHp() <= 0)
                        {
                            isFinish = false;
                            break;
                        }
                    }
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

            // �O���[�o���ϐ��̏�����
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
