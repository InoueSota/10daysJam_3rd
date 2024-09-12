using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // ����
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;

    // ���R���|�[�l���g�擾
    private ClearData clearData;
    private S_Transition transition;

    // �J�ڃV�[���於
    [SerializeField] private string nextScene;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        clearData = new ClearData();
        clearData = clearData.LoadClearData(clearData);
        transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
        transition.SetColor(4);
    }

    void Update()
    {
        GetInput();

        ChangeScene();
    }

    void ChangeScene()
    {
        if (isTriggerCancel)
        {
            clearData.ResetClearFlag(clearData);
            clearData.Save(clearData);
        }

        if (isTriggerJump && !transition.isTransNow)
        {
            //�g�����W�V��������
            transition.SetTransition(nextScene);
        }
    }

    void GetInput()
    {
        isTriggerJump = false;
        isTriggerCancel = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.CANCEL))
        {
            isTriggerCancel = true;
        }
    }
}
