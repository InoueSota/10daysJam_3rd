using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ItemManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private ItemEffectScripts sitemEffectScripts;
    private ItemParticleManager particleManager;
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

    // ���I�u�W�F�N�g�擾
    private Transform playerTransform;
    [SerializeField] CantClearManager cantClear;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sitemEffectScripts = GetComponent<ItemEffectScripts>();
        particleManager = GetComponent<ItemParticleManager>();
        ui_Item = GameObject.FindWithTag("items").GetComponent<UI_Item>();

        //�����ʒu�ۑ�
        originPos = transform.position;
        angle = 0;

        // �v���C���[�̈ʒu���擾
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

    // ����������
    void Initialize()
    {
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();
        sitemEffectScripts.Initialized();
        fakeItem.SetActive(true);
        cantClear.SetCantClear(false);
        //UI�̏���������
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
        //�Q�b�g�G�t�F�N�g�o��
        Debug.Log("�A�C�e���Q�b�g�������I�I�I");
        GameObject geteffect = Instantiate(GetEffect);
        geteffect.transform.position = gameObject.transform.position;
        fakeItem.SetActive(false);

        //UI����
        ui_Item.addList();

    }

    public void SetCantClearManager(CantClearManager cantClearManager_)
    {
        cantClear = cantClearManager_;
    }

}
