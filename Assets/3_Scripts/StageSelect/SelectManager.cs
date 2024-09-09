using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    // 自コンポーネント取得
    private InputManager inputManager;
    private bool isTriggerSpecial;
    private bool isTriggerCancel;
    //他コンポーネント取得
    S_Transition transition;
    // 選択するステージ名
    private string stageName;

    [Header("カメラ")]
    [SerializeField] private SelectCameraManager selectCameraManager;

    [Header("プレイヤー")]
    [SerializeField] private PlayerManager playerManager;

    [Header("フレーム")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Color[] themeColor;
    [SerializeField] private float colorChasePower;
    private Color targetColor;

    [Header("テーマ")]
    [SerializeField] private Text themeText;
    [SerializeField] private string[] themeTitle;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerManager.SetIsActive(true);
        playerManager.transform.position = GlobalVariables.enterPosition;
        selectCameraManager.SetTargetPosition(GlobalVariables.enterTargetPosition);
        selectCameraManager.SetDepth(GlobalVariables.enterDepth);
        targetColor = GlobalVariables.enterFrameColor;
        frameImage.color = GlobalVariables.enterFrameColor;

        transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
    }

    void Update()
    {
        GetInput();

        ChangeScene();
        ChangeByDepth();
    }

    void ChangeScene()
    {
        if (stageName != null && isTriggerSpecial && !transition.isTransNow)
        {
            GlobalVariables.enterPosition = playerManager.transform.position;
            GlobalVariables.enterTargetPosition = selectCameraManager.GetTargetPosition();
            GlobalVariables.enterDepth = selectCameraManager.GetDepth();
            GlobalVariables.enterFrameColor = targetColor;
            //SceneManager.LoadScene(stageName);
            transition.SetTransition(stageName);
            //SceneManager.LoadScene(stageName);
        }
        if (isTriggerCancel&&!transition.isTransNow)
        {
            GlobalVariables.enterPosition = playerManager.transform.position;
            GlobalVariables.enterTargetPosition = selectCameraManager.GetTargetPosition();
            GlobalVariables.enterDepth = selectCameraManager.GetDepth();
            GlobalVariables.enterFrameColor = targetColor;
            //SceneManager.LoadScene("TitleScene");
            transition.SetTransition("TitleScene");
            //SceneManager.LoadScene("TitleScene");
        }
    }
    void ChangeByDepth()
    {
        themeText.text = themeTitle[selectCameraManager.GetDepth()];
        targetColor = themeColor[selectCameraManager.GetDepth()];
        frameImage.color += (targetColor - frameImage.color) * (colorChasePower * Time.deltaTime);
    }

    // Setter
    public void SetEnterStage(string _stageName)
    {
        stageName = _stageName;
    }
    public void SetLeaveStage()
    {
        stageName = null;
    }

    // Getter
    void GetInput()
    {
        isTriggerSpecial = false;
        isTriggerCancel = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.SPECIAL))
        {
            isTriggerSpecial = true;
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.CANCEL))
        {
            isTriggerCancel = true;
        }
    }
}
