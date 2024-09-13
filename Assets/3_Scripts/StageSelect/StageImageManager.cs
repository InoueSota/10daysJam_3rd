using UnityEngine;
using UnityEngine.UI;

public class StageImageManager : MonoBehaviour
{
    [Header("�q�I�u�W�F�N�g - �N���A")]
    [SerializeField] private GameObject clearStamp;
    [Header("�q�I�u�W�F�N�g - ��Փx")]
    [SerializeField] private GameObject[] difficulty;
    [SerializeField] private Image frame;
    [SerializeField] private Color[] frameColors;

    public void Initialize(bool _isClear, int _difficulty, int _colorNum)
    {
        // �N���A�n
        clearStamp.SetActive(_isClear);

        // ��Փx�n
        for (int i = 0; i < _difficulty; i++)
        {
            difficulty[i].SetActive(true);
        }
        frame.color = frameColors[_colorNum];
    }
}
