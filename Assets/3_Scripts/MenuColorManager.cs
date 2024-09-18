using UnityEngine;
using UnityEngine.UI;

public class MenuColorManager : MonoBehaviour
{
    [Header("ImageŽæ“¾")]
    [SerializeField] private Image stageNumberBack;
    [SerializeField] private Image stageNumberFrame;
    [SerializeField] private Image purposeTitleFrameBack;
    [SerializeField] private Image purposeTitleFrame;
    [SerializeField] private Image purposeFrameBack;
    [SerializeField] private Image purposeFrame;
    [SerializeField] private Image tabBackGround;
    [SerializeField] private Image tabBackGroundCircle;
    [SerializeField] private Image tabFrame;
    [SerializeField] private Image returnBackGround;
    [SerializeField] private Image restartBackGround;
    [SerializeField] private Image stageSelectBackGround;

    [Header("Next - ImageŽæ“¾")]
    [SerializeField] private Image nextBack;
    [SerializeField] private Image nextFrame;
    [SerializeField] private Image nextStageNumberBack;
    [SerializeField] private Image nextStageNumberFrame;
    [SerializeField] private Image retryBack;
    [SerializeField] private Image goNextBack;
    [SerializeField] private Image goSelectBack;

    public void SetColor(Color _brightColor, Color _originalColor, Color _darkColor)
    {
        stageNumberBack.color = _darkColor;
        stageNumberFrame.color = _originalColor;
        purposeTitleFrameBack.color = _darkColor;
        purposeTitleFrame.color = _originalColor;
        purposeFrameBack.color = _darkColor;
        purposeFrame.color = _originalColor;
        tabFrame.color = _darkColor;
        tabBackGroundCircle.color = _originalColor;
        tabBackGround.color = _brightColor;
        returnBackGround.color = _darkColor;
        restartBackGround.color = _darkColor;
        stageSelectBackGround.color = _darkColor;

        nextBack.color = _darkColor;
        nextFrame.color = _originalColor;
        nextStageNumberBack.color = _darkColor;
        nextStageNumberFrame.color = _originalColor;
        retryBack.color = _darkColor;
        goNextBack.color = _darkColor;
        goSelectBack.color = _darkColor;
    }
}
