using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private ItemEffectScripts sitemEffectScripts;
    //�h�炷�p
    private Vector3 originPos;
    private float angle;
    [SerializeField] float lenge;
    [SerializeField] float flowSpeed;
    //�󂵂��Ƃ��悤�I�u�W�F�N�g
    public GameObject fakeItem;
    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sitemEffectScripts = GetComponent<ItemEffectScripts>();
        //�����ʒu�ۑ�
        originPos = transform.position;
        angle = 0;
    }

    private void FixedUpdate()
    {
        angle += flowSpeed;
        Vector3 frowPos = new Vector3(originPos.x, originPos.y+ (MathF.Sin(angle)* lenge), originPos.z);
        transform.position = frowPos;
    }

    // ����������
    void Initialize()
    {
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();
        sitemEffectScripts.Initialized();
        fakeItem.SetActive(true);
    }

    // ���ŏ���
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
    public void SetIsGet()
    {
        fakeItem.SetActive(false);
    }
    
}
