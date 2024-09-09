using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageGateManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private SpriteRenderer spriteRenderer;

    [Header("�J�ڐ�X�e�[�W��")]
    [SerializeField] private string stageName;

    [Header("�X�e�[�W���̃e�L�X�g")]
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
