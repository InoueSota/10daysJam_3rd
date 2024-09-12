using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // 入力
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;

    // 他コンポーネント取得
    private ClearData clearData;
    private S_Transition transition;

    // 遷移シーン先名
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
            //トランジション処理
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
