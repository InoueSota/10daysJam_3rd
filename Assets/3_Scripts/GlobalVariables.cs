using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // フラグ類
    public static bool isClear = false;

    // 名前類
    public static string retryStageName = "Stage1";
    public static string nextStageName = "Stage1";

    // 座標類
    public static Vector3 enterPosition = new(-7f, -1.5f, 0f);
    public static Vector3 enterTargetPosition = new(0f, 0.34375f, -10f);
    public static int enterDepth= 0;

    // 色類
    public static Color enterFrameColor = new(0.27f, 0.75f, 0f, 0.6f);
}
