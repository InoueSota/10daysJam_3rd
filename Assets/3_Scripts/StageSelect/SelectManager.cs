using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    // 自コンポーネント取得
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;
    //他コンポーネント取得
    S_Transition transition;
    // 選択するステージ名
    private string stageName;

    [Header("プレイヤー")]
    [SerializeField] private PlayerManager playerManager;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerManager.SetIsActive(true);

        transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
    }

    void Update()
    {
        GetInput();

        ChangeScene();
    }

    void ChangeScene()
    {
        if (stageName != null && isTriggerJump)
        {
            transition.SetTransition(stageName);
            //SceneManager.LoadScene(stageName);
        }
        if (isTriggerCancel)
        {
            transition.SetTransition("TitleScene");
            //SceneManager.LoadScene("TitleScene");
        }
    }

    // Setter
    public void SetEnterStage(string _stageName)
    {
        stageName = _stageName;
        playerManager.SetCanJump(false);
    }
    public void SetLeaveStage()
    {
        stageName = null;
        playerManager.SetCanJump(true);
    }

    // Getter
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
