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
    [SerializeField] UI_Item ui_Item;
    //�h�炷�p
    private Vector3 originPos;
    private float angle;
    [SerializeField] float lenge;
    [SerializeField] float flowSpeed;
    //�󂵂��Ƃ��悤�I�u�W�F�N�g
    public GameObject fakeItem;
    //�G�t�F�N�g
    [SerializeField] GameObject GetEffect;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sitemEffectScripts = GetComponent<ItemEffectScripts>();
        ui_Item = GameObject.FindWithTag("items").GetComponent<UI_Item>();

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
        //UI�̏���������
        ui_Item.Initialize();
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
        //�Q�b�g�G�t�F�N�g�o��
        Debug.Log("�A�C�e���Q�b�g�������I�I�I");
        GameObject geteffect = Instantiate(GetEffect);
        geteffect.transform.position = gameObject.transform.position;
        fakeItem.SetActive(false);

        //UI����
        ui_Item.addList();

    }
    
}
