using UnityEngine;
using UnityEngine.UI;

public class StageImageManager : MonoBehaviour
{
    [Header("子オブジェクト - クリア")]
    [SerializeField] private GameObject clearStamp;
    [Header("子オブジェクト - 難易度")]
    [SerializeField] private GameObject[] difficulty;
    [SerializeField] private Image frame;
    [SerializeField] private Color[] frameColors;

    public void Initialize(bool _isClear, int _difficulty, int _colorNum)
    {
        // クリア系
        clearStamp.SetActive(_isClear);

        // 難易度系
        for (int i = 0; i < _difficulty; i++)
        {
            difficulty[i].SetActive(true);
        }
        frame.color = frameColors[_colorNum];
    }
}
