using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // ����
    private InputManager inputManager;
    private bool isTriggerJump;

    // ���R���|�[�l���g�擾
    private S_Transition transition;

    // �J�ڃV�[���於
    [SerializeField] private string nextScene;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
    }

    void Update()
    {
        GetInput();

        ChangeScene();
    }

    void ChangeScene()
    {
        if (isTriggerJump && !transition.isTransNow)
        {
            //�g�����W�V��������
            transition.SetTransition(nextScene);
        }
    }

    void GetInput()
    {
        isTriggerJump = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
    }
}
