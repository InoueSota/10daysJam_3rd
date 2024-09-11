using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private StageObjectManager stageObjectManager;
    private MenuManager menuManager;
    private InputManager inputManager;
    private bool isTriggerCancel;
    private bool isTriggerReset;
    private bool isTriggerspecial;

    // ���R���|�[�l���g�擾
    S_Transition transition;

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
        menuManager = GetComponent<MenuManager>();
        inputManager = GetComponent<InputManager>();
        if (GameObject.FindWithTag("trans"))
        {
            transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
        }
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

        Menu();
        Ready();
        Clear();

        if (isTriggerCancel && isStart && !isClear && !menuManager.GetIsMenuActive())
        {
            Restart();
        }
    }

    void Menu()
    {
        // ���j���[���J�� / ����
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
                // ��ʊO�̃I�u�W�F�N�g��j�󂷂�
                DestroyOutOfCameraObj();
                if (!menuManager.GetIsMenuActive())
                {
                    stageObjectManager.SetCanCheck(true);
                }
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
            if (isTriggerspecial && !transition.isTransNow)
            {
                //�g�����W�V��������
                transition.SetTransition("SelectScene");
                //SceneManager.LoadScene("SelectScene");
            }
        }
    }
    public void Restart()
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

        // �G�b�t�F����
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("effect"))
        {
            // �c���Ă�G�t�F�N�g����������
            Destroy(obj);

        }
        // �O���[�o���ϐ��̏�����
        GlobalVariables.isClear = false;
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
