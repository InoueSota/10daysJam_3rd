using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;

    //揺らす用
    private Vector3 originPos;
    private float angle;
    [SerializeField] float lenge;
    
    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //初期位置保存
        originPos = transform.position;
        angle = 0;
    }

    private void FixedUpdate()
    {
        angle += 0.01f;
        Vector3 frowPos = new Vector3(originPos.x, originPos.y+ (MathF.Sin(angle)* lenge), originPos.z);
        transform.position = frowPos;
    }

    // 初期化処理
    void Initialize()
    {
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();
    }

    // 消滅処理
    void Destruction()
    {
        spriteRenderer.enabled = false;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetIsActive() && allObjectManager.GetHp() <= 0)
        {
            SetIsActive(false);
        }
    }
    public void SetIsActive(bool _isActive)
    {
        if (_isActive)
        {
            Initialize();
        }
        else
        {
            Destruction();
        }
    }
}
