using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageGateManager : MonoBehaviour
{
    // 自コンポーネント取得
    private SpriteRenderer spriteRenderer;

    [Header("遷移先ステージ名")]
    [SerializeField] private string stageName;

    [Header("ステージ名のテキスト")]
    [SerializeField] private Text stageNameText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (stageNameText)
        {
            stageNameText.transform.position = new(transform.position.x, transform.position.y + 1f, transform.position.z);
            stageNameText.text = stageName;
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
