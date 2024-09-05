using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // 消えているとき
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
            // ここが消された瞬間
            //
            spriteRenderer.enabled = false;
        }
        allObjectManager.SetIsActive(spriteRenderer.enabled);
    }
}
