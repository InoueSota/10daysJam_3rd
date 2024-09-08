using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGateManager : MonoBehaviour
{
    // 自コンポーネント取得
    private SpriteRenderer spriteRenderer;

    [Header("遷移先ステージ名")]
    [SerializeField] private string stageName;

    private enum StageTheme
    {
        GRASSLAND,
        CAVE,
        JUNGLE,
        SNOWFIELD,
        DARKNESS
    }
    [Header("ステージのテーマ")]
    [SerializeField] private StageTheme stageTheme;

    [Header("テーマごとの画像")]
    [SerializeField] private Sprite grassLandSprite;
    [SerializeField] private Sprite caveSprite;
    [SerializeField] private Sprite jungleSprite;
    [SerializeField] private Sprite snowFieldSprite;
    [SerializeField] private Sprite darknessSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeSprite();
    }
    void ChangeSprite()
    {
        switch (stageTheme)
        {
            case StageTheme.GRASSLAND:
                spriteRenderer.sprite = grassLandSprite;
                break;
            case StageTheme.CAVE:
                spriteRenderer.sprite = caveSprite;
                break;
            case StageTheme.JUNGLE:
                spriteRenderer.sprite = jungleSprite;
                break;
            case StageTheme.SNOWFIELD:
                spriteRenderer.sprite = snowFieldSprite;
                break;
            case StageTheme.DARKNESS:
                spriteRenderer.sprite = darknessSprite;
                break;
        }
    }

    void Update()
    {
        
    }

    public string GetStageName()
    {
        return stageName;
    }
}
