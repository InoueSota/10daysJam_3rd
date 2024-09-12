using UnityEngine;

public class GameColorManager : MonoBehaviour
{
    // 自コンポーネント取得
    private GameManager gameManager;

    [Header("草原")]
    [SerializeField] private Color grassLandDarkColor;
    [SerializeField] private Color grassLandOriginalColor;
    [SerializeField] private Color grassLandBrightColor;

    [Header("洞窟")]
    [SerializeField] private Color caveDarkColor;
    [SerializeField] private Color caveOriginalColor;
    [SerializeField] private Color caveBrightColor;

    [Header("砂漠")]
    [SerializeField] private Color desertDarkColor;
    [SerializeField] private Color desertOriginalColor;
    [SerializeField] private Color desertBrightColor;

    [Header("墓地")]
    [SerializeField] private Color cemeteryDarkColor;
    [SerializeField] private Color cemeteryOriginalColor;
    [SerializeField] private Color cemeteryBrightColor;

    void Start()
    {
        gameManager = GetComponent<GameManager>();

        MenuColorManager menuColorManager = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuColorManager>();

        // 現在ステージがどのチャプターかをGlobalVariablesを参照し、判断する
        if (GlobalVariables.selectStageNumber >= GlobalVariables.toCemeteryNumber)
        {
            menuColorManager.SetColor(cemeteryBrightColor, cemeteryOriginalColor, cemeteryDarkColor);
            gameManager.SetRestartAnimColor(cemeteryDarkColor);
        } else
        if (GlobalVariables.selectStageNumber >= GlobalVariables.toDesertNumber)
        {
            menuColorManager.SetColor(desertBrightColor, desertOriginalColor, desertDarkColor);
            gameManager.SetRestartAnimColor(desertDarkColor);
        }
        else
        if (GlobalVariables.selectStageNumber >= GlobalVariables.toCaveNumber)
        {
            menuColorManager.SetColor(caveBrightColor, caveOriginalColor, caveDarkColor);
            gameManager.SetRestartAnimColor(caveDarkColor);
        }
        else
        {
            menuColorManager.SetColor(grassLandBrightColor, grassLandOriginalColor, grassLandDarkColor);
            gameManager.SetRestartAnimColor(grassLandDarkColor);
        }
    }
}
