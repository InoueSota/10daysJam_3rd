using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // �t���O��
    public static bool isClear = false;

    // ���O��
    public static string retryStageName = "Stage1";
    public static string nextStageName = "Stage1";

    // ���W��
    public static Vector3 enterPosition = new(-7f, -1.5f, 0f);
    public static float enterTargetX = 0f;
}
