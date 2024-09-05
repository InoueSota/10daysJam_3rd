using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // �����Ă���Ƃ�
        if (!spriteRenderer.enabled)
        {

        }
    }

    public void SetIsActive(bool _isActive)
    {
        if (_isActive)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            //
            // �����������ꂽ�u��
            //
            spriteRenderer.enabled = false;
        }
        allObjectManager.SetIsActive(spriteRenderer.enabled);
    }
}
