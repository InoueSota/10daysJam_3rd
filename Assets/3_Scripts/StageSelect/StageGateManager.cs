using UnityEngine;

public class StageGateManager : MonoBehaviour
{
    public enum Chapter
    {
        GRASSLAND,
        CAVE,
        DESERT,
        CEMETERY
    }
    [Header("該当チャプター")]
    [SerializeField] private Chapter chapter;

    public Chapter GetChapter()
    {
        return chapter;
    }
}
