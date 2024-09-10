using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // 自コンポーネント取得
    private GameManager gameManager;
    private InputManager inputManager;
    private bool isTriggerUp;
    private bool isTriggerDown;
    private bool isTriggerJump;

    // 他コンポーネント取得
    private S_Transition transition;

    public enum MenuType
    {
        RETURN,
        RESTART,
        STAGESELECT
    }
    private MenuType menuType = MenuType.RETURN;

    // メニュー画面のアクティブフラグ
    [SerializeField] private bool isMenuActive;

    [Header("メニュー画面背景")]
    [SerializeField] private Image menuBackImage;
    [SerializeField] private Color darkColor;
    private Color menuBackOriginColor;
    private Color menuBackTargetColor;

    [Header("メニュータブ")]
    [SerializeField] private RectTransform menuTabRect;
    private Vector3 menuTabOrigin;
    private Vector3 menuTabTarget;

    [Header("メニュー内のコンテンツ")]
    [SerializeField] private float menuContentsDiff;
    [SerializeField] private float chasePower;
    [SerializeField] private RectTransform returnRect;
    private Vector3 returnRectOrigin;
    private Vector3 returnRectTarget;
    [SerializeField] private RectTransform restartRect;
    private Vector3 restartRectOrigin;
    private Vector3 restartRectTarget;
    [SerializeField] private RectTransform stageSelectRect;
    private Vector3 stageSelectRectOrigin;
    private Vector3 stageSelectRectTarget;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        inputManager = GetComponent<InputManager>();
        transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();

        menuBackOriginColor = new(darkColor.r, darkColor.g, darkColor.b, 0f);
        menuBackTargetColor = menuBackOriginColor;
        menuBackImage.color = menuBackTargetColor;

        menuTabOrigin = menuTabRect.transform.localPosition;
        returnRectOrigin = returnRect.transform.localPosition;
        restartRectOrigin = restartRect.transform.localPosition;
        stageSelectRectOrigin = stageSelectRect.transform.localPosition;

        menuTabTarget = menuTabOrigin;
        returnRectTarget = returnRectOrigin;
        restartRectTarget = restartRectOrigin;
        stageSelectRectTarget = stageSelectRectOrigin;
    }

    void Update()
    {
        GetInput();

        if (isMenuActive)
        {
            ChangeMenuType();
        }
        Menu();
    }
    void ChangeMenuType()
    {
        switch (menuType)
        {
            case MenuType.RETURN:

                returnRectTarget.x = returnRectOrigin.x - menuContentsDiff;
                restartRectTarget.x = restartRectOrigin.x;
                stageSelectRectTarget.x = stageSelectRectOrigin.x;

                if (isTriggerDown)
                {
                    menuType = MenuType.RESTART;
                }

                if (isTriggerJump)
                {
                    gameManager.SetPlayerAcitve(isMenuActive);
                    SetIsMenuActive();
                }

                break;
            case MenuType.RESTART:

                returnRectTarget.x = returnRectOrigin.x;
                restartRectTarget.x = restartRectOrigin.x - menuContentsDiff;
                stageSelectRectTarget.x = stageSelectRectOrigin.x;

                if (isTriggerUp)
                {
                    menuType = MenuType.RETURN;
                }
                else if (isTriggerDown)
                {
                    menuType = MenuType.STAGESELECT;
                }

                if (isTriggerJump)
                {
                    gameManager.SetPlayerAcitve(isMenuActive);
                    SetIsMenuActive();
                    gameManager.Restart();
                }

                break;
            case MenuType.STAGESELECT:

                returnRectTarget.x = returnRectOrigin.x;
                restartRectTarget.x = restartRectOrigin.x;
                stageSelectRectTarget.x = stageSelectRectOrigin.x - menuContentsDiff;

                if (isTriggerUp)
                {
                    menuType = MenuType.RESTART;
                }

                if (isTriggerJump && !transition.isTransNow)
                {
                    SetIsMenuActive();
                    transition.SetTransition("SelectScene");
                }

                break;
        }
    }
    void Menu()
    {
        menuBackImage.color += (menuBackTargetColor - menuBackImage.color) * (chasePower * Time.deltaTime);
        menuTabRect.transform.localPosition += (menuTabTarget - menuTabRect.transform.localPosition) * (chasePower * Time.deltaTime);
        returnRect.transform.localPosition += (returnRectTarget - returnRect.transform.localPosition) * (chasePower * Time.deltaTime);
        restartRect.transform.localPosition += (restartRectTarget - restartRect.transform.localPosition) * (chasePower * Time.deltaTime);
        stageSelectRect.transform.localPosition += (stageSelectRectTarget - stageSelectRect.transform.localPosition) * (chasePower * Time.deltaTime);
    }

    // Setter
    public void SetIsMenuActive()
    {
        isMenuActive = !isMenuActive;
        if (isMenuActive)
        {
            menuBackTargetColor = darkColor;
            menuTabTarget.x = 440f;
            menuType = MenuType.RETURN;
        }
        else
        {
            menuBackTargetColor = menuBackOriginColor;
            menuTabTarget.x = menuTabOrigin.x;
        }
    }

    // Getter
    public bool GetIsMenuActive()
    {
        return isMenuActive;
    }
    void GetInput()
    {
        isTriggerUp = false;
        isTriggerDown = false;
        isTriggerJump = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.VERTICAL))
        {
            if (inputManager.ReturnInputValue(InputManager.INPUTPATTERN.VERTICAL) > 0f)
            {
                isTriggerUp = true;
            }
            else
            {
                isTriggerDown = true;
            }
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
    }
}
