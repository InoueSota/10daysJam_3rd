using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // 名前類
    public static string retryStageName = "Stage1";
    public static string nextStageName = "Stage1";

    // ステージ選択の番号
    public static int selectStageNumber = 0;

    // チャプターが切り替わるステージ番号
    public static int toCaveNumber = 7;
    public static int toDesertNumber = 16;
    public static int toCemeteryNumber = 25;

    // 次のステージ関係
    public static bool[] isClear;
    public static int[] stageDifficulty;
    public static int[] stageChapterNumber;
    public static Sprite[] stageSprite;
    public static string[] stageTitle;
}
