using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ItemManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private ItemEffectScripts sitemEffectScripts;
    private ItemParticleManager particleManager;
    [SerializeField] UI_Item ui_Item;
    //揺らす用
    private Vector3 originPos;
    private float angle;
    [SerializeField] float lenge;
    [SerializeField] float flowSpeed;
    //壊したときようオブジェクト
    public GameObject fakeItem;
    //エフェクト
    [SerializeField] GameObject GetEffect;

    // 他オブジェクト取得
    private Transform playerTransform;
    [SerializeField] CantClearManager cantClear;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sitemEffectScripts = GetComponent<ItemEffectScripts>();
        particleManager = GetComponent<ItemParticleManager>();
        ui_Item = GameObject.FindWithTag("items").GetComponent<UI_Item>();

        //初期位置保存
        originPos = transform.position;
        angle = 0;

        // プレイヤーの位置を取得
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        NoticeBreak();
    }

    private void FixedUpdate()
    {
        angle += flowSpeed;
        Vector3 frowPos = new Vector3(originPos.x, originPos.y+ (MathF.Sin(angle)* lenge), originPos.z);
        transform.position = frowPos;
    }

    // 初期化処理
    void Initialize()
    {
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();
        sitemEffectScripts.Initialized();
        fakeItem.SetActive(true);
        cantClear.SetCantClear(false);
        //UIの初期化処理
        ui_Item.Initialize();
    }

    void NoticeBreak()
    {
        if (allObjectManager.GetIsActive())
        {
            float yBetween = Mathf.Abs(transform.position.y - playerTransform.position.y + 1f);

            if (yBetween < 0.4f)
            {
                particleManager.RunParticle();
            }
            else
            {
                particleManager.Reset();
            }
        }
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
            if(cantClear != null)
            {
                cantClear.SetCantClear(true);
            }
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
    public void SetIsGet()
    {
        //ゲットエフェクト出す
        Debug.Log("アイテムゲットだっぜ！！！");
        GameObject geteffect = Instantiate(GetEffect);
        geteffect.transform.position = gameObject.transform.position;
        fakeItem.SetActive(false);

        //UI処理
        ui_Item.addList();

    }

    public void SetCantClearManager(CantClearManager cantClearManager_)
    {
        cantClear = cantClearManager_;
    }

}
