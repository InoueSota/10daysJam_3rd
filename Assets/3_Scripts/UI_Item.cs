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

    public void Initialize()
    {
        //���X�g���Ȃ����B
        for (int i = 0; i < isOne.Count; i++)
        {
            isOne[i] = false;
            //Destroy(DestoryObjs[i]);
            //DestoryObjs[i] = null;
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
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
    }

}
