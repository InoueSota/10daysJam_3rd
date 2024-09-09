using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // 入力
    private InputManager inputManager;
    private bool isTriggerJump;
    //他コンポーネント取得
    S_Transition transition;
    // 遷移シーン先名
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
        if (isTriggerJump)
        {
            //トランジション処理
            transition.SetTransition(nextScene);
            //SceneManager.LoadScene(nextScene);
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
