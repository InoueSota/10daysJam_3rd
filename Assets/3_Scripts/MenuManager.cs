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
    private bool isMenuActive;

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

    [Header("ステージ番号")]
    [SerializeField] private RectTransform stageNumberRect;
    private Vector3 stageNumberOrigin;
    private Vector3 stageNumberTarget;

    [Header("目的")]
    [SerializeField] private RectTransform purposeTitleRect;
    private Vector3 purposeTitleOrigin;
    private Vector3 purposeTitleTarget;
    [SerializeField] private RectTransform purposeRect;
    private Vector3 purposeOrigin;
    private Vector3 purposeTarget;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        inputManager = GetComponent<InputManager>();
        if (GameObject.FindWithTag("trans"))
        {
            transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
        }
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

        stageNumberOrigin = stageNumberRect.transform.localPosition;

        stageNumberTarget = stageNumberOrigin;

        purposeTitleOrigin = purposeTitleRect.transform.localPosition;
        purposeOrigin = purposeRect.transform.localPosition;

        purposeTitleTarget = purposeTitleOrigin;
        purposeTarget = purposeOrigin;
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
        stageNumberRect.transform.localPosition += (stageNumberTarget - stageNumberRect.transform.localPosition) * (chasePower * Time.deltaTime);
        purposeTitleRect.transform.localPosition += (purposeTitleTarget - purposeTitleRect.transform.localPosition) * (chasePower * Time.deltaTime);
        purposeRect.transform.localPosition += (purposeTarget- purposeRect.transform.localPosition) * (chasePower * Time.deltaTime);
    }

    // Setter
    public void SetIsMenuActive()
    {
        isMenuActive = !isMenuActive;
        if (isMenuActive)
        {
            menuBackTargetColor = darkColor;
            menuTabTarget.x = 440f;
            stageNumberTarget.x = -480f;
            purposeTitleTarget.x = -480f;
            purposeTarget.y = -200f;
            menuType = MenuType.RETURN;
        }
        else
        {
            menuBackTargetColor = menuBackOriginColor;
            menuTabTarget.x = menuTabOrigin.x;
            stageNumberTarget.x = stageNumberOrigin.x;
            purposeTitleTarget.x = purposeTitleOrigin.x;
            purposeTarget.y = purposeOrigin.y;
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
