using UnityEngine;

public class GameColorManager : MonoBehaviour
{
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
        MenuColorManager menuColorManager = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuColorManager>();

        // ���݃X�e�[�W���ǂ̃`���v�^�[����GlobalVariables���Q�Ƃ��A���f����
        if (GlobalVariables.selectStageNumber >= GlobalVariables.toCemeteryNumber)
        {
            menuColorManager.SetColor(cemeteryBrightColor, cemeteryOriginalColor, cemeteryDarkColor);
        } else
        if (GlobalVariables.selectStageNumber >= GlobalVariables.toDesertNumber)
        {
            menuColorManager.SetColor(desertBrightColor, desertOriginalColor, desertDarkColor);
        } else
        if (GlobalVariables.selectStageNumber >= GlobalVariables.toCaveNumber)
        {
            menuColorManager.SetColor(caveBrightColor, caveOriginalColor, caveDarkColor);
        }
        else
        {
            menuColorManager.SetColor(grassLandBrightColor, grassLandOriginalColor, grassLandDarkColor);
        }
    }
}
