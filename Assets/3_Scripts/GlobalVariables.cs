using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // ���O��
    public static string retryStageName = "Stage1";
    public static string nextStageName = "Stage1";

    // �X�e�[�W�I���̔ԍ�
    public static int selectStageNumber = 0;

    // �`���v�^�[���؂�ւ��X�e�[�W�ԍ�
    public static int toCaveNumber = 7;
    public static int toDesertNumber = 16;
    public static int toCemeteryNumber = 25;

    // ���̃X�e�[�W�֌W
    public static bool[] isClear;
    public static int[] stageDifficulty;
    public static int[] stageChapterNumber;
    public static Sprite[] stageSprite;
    public static string[] stageTitle;
}
