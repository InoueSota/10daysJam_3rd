using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Item : MonoBehaviour
{
    //�A�C�e���}�l�[�W���[
    [SerializeField] UICropScript ui_crop_script;
    //�擾�����A�C�e���i�[
    [SerializeField]public List<bool> items;
    [SerializeField]public List<bool> isOne;
    //�A�C�e���͂߂�UI�|�W�V����[0][1][2]
    [SerializeField] List<GameObject> itemPostions;
    [SerializeField] public List<GameObject> DestoryObjs;
    //���ł͂߂�v���n�u
    [SerializeField] GameObject UI_itemEffedtPrefab;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    //
    //�v���C���[
    GameObject Player;
    PlayerMoveManager moveManager;

    //�X�v���C�g
    SpriteRenderer spriteRenderer;

    //�@�\
    [SerializeField] float coolTime;
    [SerializeField] Ease TypeEase_In;
    [SerializeField] Ease TypeEase_Out;
    private bool isOne_;
    private bool isOne_Comp;
    private int ItemsCount;

    //�T�C���g
    //�h�炷�p
    private Vector3 originPos;
    [SerializeField] float angle;
    [SerializeField] float lenge;
    [SerializeField] float flowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        moveManager = Player.GetComponent<PlayerMoveManager>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        //items = new List<bool> {false,false,false };
        items.Clear();
        ItemsCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ItemsCount = items.Count(item => item);

        Debug.Log(ItemsCount);
        if (ItemsCount >= 3 && !isOne_Comp)
        {
            audioSource.PlayOneShot(clip);
                Debug.Log("�A�C�e���R���v���[�g");
            
            //�A�C�e�������ׂďW�܂�����
            transform.DOScale(Vector3.one * 2, 0.6f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            });
            isOne_Comp = true;
        }
        else if (ItemsCount < 3)
        {
            //�A�C�e�������ׂďW�܂��ĂȂ��Ƃ�
            //UI�o������
            PushUI();
        }
        ////UI�̏ꏊ�ɃA�C�e���𖄂߂鏈��
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] && !isOne[i])
            {
                //�A�C�e��i��
                isOne[i] = true;
                GameObject gameObject = Instantiate(UI_itemEffedtPrefab);
                gameObject.transform.parent = itemPostions[i].transform;
                //gameObject.transform.position = itemPostions[i].transform.position;
                gameObject.transform.localPosition = Vector3.zero;
                DestoryObjs.Add(gameObject);
            }
            
        }

        UndoUIItems();
        Debug.Log("�A�C�e���J�E���g�F"+ItemsCount);


    }

    private void FixedUpdate()
    {
        if (ItemsCount >= 3 && isOne_Comp)
        {
          
            for (int i = 0; i < items.Count; i++)
            {                
                DestoryObjs[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                angle += flowSpeed;
                Vector3 frowPos = new Vector3(itemPostions[i].transform.position.x, itemPostions[i].transform.position.y + (MathF.Sin(angle+(i*45)) * lenge), itemPostions[i].transform.position.z);
                itemPostions[i].transform.position = frowPos;
            }
        }
    }

    private void PushUI()
    {
        //UI���o������
        coolTime = Mathf.Clamp(coolTime, -1, 1);

        if (moveManager.GetIsMoving())
        {
            //��������o���Ȃ�
            coolTime -= Time.deltaTime;
        }
        else
        {
            coolTime += Time.deltaTime;
        }
        if (coolTime > 0.5f)
        {
            //�~�܂��ĂP�b��������
            //��񂾂��ʂ�
            if (!isOne_)
            {
                isOne_ = true;
                transform.DOScale(Vector3.one, 0.3f).SetEase(TypeEase_In);
            }
        }
        else
        {
            //�����n�߂��Ƃ�
            //��񂾂��ʂ�
            if (isOne_)
            {
                isOne_ = false;
                transform.DOScale(Vector3.zero, 0.6f).SetEase(TypeEase_Out);
            }

        }
    }

    public void Initialize()
    {
        //���X�g���Ȃ����B
        for (int i = 0; i < isOne.Count; i++)
        {
            isOne[i] = false;
            //Destroy(DestoryObjs[i]);
            //DestoryObjs[i] = null;
        }
        //UI
        itemPostions[0].transform.localPosition = new Vector3(-1, 0, 0);
        itemPostions[1].transform.localPosition = new Vector3(0, 0, 0);
        itemPostions[2].transform.localPosition = new Vector3(1, 0, 0);

        // List���̂��ׂĂ�UI��j�󂷂�
        foreach (GameObject obj in DestoryObjs)
        {
            Destroy(obj);
        }
        DestoryObjs.Clear();
        items.Clear();
    }
    public void addList()
    {
        items.Add(true);
        coolTime = 1.2f;
        isOne_ = false;
        isOne_Comp = false;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
    }
    public void UndoUIItems()
    {
        // Step 1: �A�C�e���̏�Ԃ��m�F���Afalse �Ȃ�I�u�W�F�N�g���폜����
        for (int i = 0; i < items.Count; i++)
        {
            if (i < DestoryObjs.Count) // �I�u�W�F�N�g�����݂��邩�m�F
            {
                // �A�C�e���� false �Ȃ�A���̃I�u�W�F�N�g���폜
                if (!items[i])
                {
                    Debug.Log($"�A�C�e�� {i} ���폜���܂�");
                    Destroy(DestoryObjs[i]);  // �I�u�W�F�N�g���폜
                    DestoryObjs[i] = null;    // ���X�g���̎Q�Ƃ��N���A
                }
            }
        }

        // Step 2: `null` �ɂȂ����I�u�W�F�N�g�����X�g����폜
        DestoryObjs.RemoveAll(obj => obj == null);

        // Step 3: �]���ȃI�u�W�F�N�g������ꍇ�A�폜����
        if (DestoryObjs.Count > items.Count)
        {
            int excessCount = DestoryObjs.Count - items.Count;
            Debug.Log($"�]���ȃI�u�W�F�N�g�� {excessCount} ����܂��B�폜���܂��B");

            // ���X�g�̌�납��]���ȃI�u�W�F�N�g���폜
            for (int i = 0; i < excessCount; i++)
            {
                int lastIndex = DestoryObjs.Count - 1; // �Ō�̗v�f�̃C���f�b�N�X
                Destroy(DestoryObjs[lastIndex]);        // �Ō�̃I�u�W�F�N�g���폜
                DestoryObjs.RemoveAt(lastIndex);        // ���X�g����폜
            }
        }

        Debug.Log("�A�C�e�����X�g�̍X�V���������܂����B");
    }
}
