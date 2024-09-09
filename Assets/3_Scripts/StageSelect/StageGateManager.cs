using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGateManager : MonoBehaviour
{
    public enum Chapter
    {
        GRASSLAND,
        CAVE,
        DESERT,
        SNOWFIELD,
        CEMETERY
    }
    [Header("該当チャプター")]
    [SerializeField] private Chapter chapter;

    public Chapter GetChapter()
    {
        return chapter;
    }
}
