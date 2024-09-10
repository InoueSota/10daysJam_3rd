using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Item : MonoBehaviour
{
    //�A�C�e���}�l�[�W���[
    [SerializeField] UICropScript ui_crop_script;
    //�擾�����A�C�e���i�[
    [SerializeField] List<bool> items;
    [SerializeField] List<bool> isOne;
    //�A�C�e���͂߂�UI�|�W�V����[0][1][2]
    [SerializeField] List<GameObject> itemPostions;
    [SerializeField] List<GameObject> DestoryObjs;
    //���ł͂߂�v���n�u
    [SerializeField] GameObject UI_itemEffedtPrefab;


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
        items.Clear();
    }

    // Update is called once per frame
    void Update()
    {


        if (items.Count >= 3 && !isOne_Comp)
        {
           
                Debug.Log("�A�C�e���R���v���[�g");
            
            //�A�C�e�������ׂďW�܂�����
            transform.DOScale(Vector3.one * 2, 0.6f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            });
            isOne_Comp = true;
        }
        else if (items.Count < 3)
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





    }

    private void FixedUpdate()
    {
        if (items.Count >= 3 && isOne_Comp)
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

        if (moveManager.IsMoving())
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
        foreach (GameObject obj in itemPostions)
        {
            obj.transform.localPosition.Set(obj.transform.localPosition.x,0.0f, obj.transform.localPosition.z);
        }

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

}
