using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGateManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private SpriteRenderer spriteRenderer;

    [Header("�J�ڐ�X�e�[�W��")]
    [SerializeField] private string stageName;

    private enum StageTheme
    {
        GRASSLAND,
        CAVE,
        JUNGLE,
        SNOWFIELD,
        DARKNESS
    }
    [Header("�X�e�[�W�̃e�[�}")]
    [SerializeField] private StageTheme stageTheme;

    [Header("�e�[�}���Ƃ̉摜")]
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
