using UnityEngine;

public class GameColorManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private GameManager gameManager;

    [Header("����")]
    [SerializeField] private Color grassLandDarkColor;
    [SerializeField] private Color grassLandOriginalColor;
    [SerializeField] private Color grassLandBrightColor;

    [Header("���A")]
    [SerializeField] private Color caveDarkColor;
    [SerializeField] private Color caveOriginalColor;
    [SerializeField] private Color caveBrightColor;

    [Header("����")]
    [SerializeField] private Color desertDarkColor;
    [SerializeField] private Color desertOriginalColor;
    [SerializeField] private Color desertBrightColor;

    [Header("��n")]
    [SerializeField] private Color cemeteryDarkColor;
    [SerializeField] private Color cemeteryOriginalColor;
    [SerializeField] private Color cemeteryBrightColor;

    void Start()
    {
        gameManager = GetComponent<GameManager>();

        MenuColorManager menuColorManager = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuColorManager>();

        // ���݃X�e�[�W���ǂ̃`���v�^�[����GlobalVariables���Q�Ƃ��A���f����
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
