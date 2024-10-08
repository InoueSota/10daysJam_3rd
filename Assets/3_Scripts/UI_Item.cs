using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Item : MonoBehaviour
{
    //アイテムマネージャー
    [SerializeField] UICropScript ui_crop_script;
    //取得したアイテム格納
    [SerializeField] List<bool> items;
    [SerializeField] List<bool> isOne;
    //アイテムはめるUIポジション[0][1][2]
    [SerializeField] List<GameObject> itemPostions;
    [SerializeField] List<GameObject> DestoryObjs;
    //仮ではめるプレハブ
    [SerializeField] GameObject UI_itemEffedtPrefab;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    //
    //プレイヤー
    GameObject Player;
    PlayerMoveManager moveManager;

    //スプライト
    SpriteRenderer spriteRenderer;

    //機能
    [SerializeField] float coolTime;
    [SerializeField] Ease TypeEase_In;
    [SerializeField] Ease TypeEase_Out;
    private bool isOne_;
    private bool isOne_Comp;

    //サイン波
    //揺らす用
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
            audioSource.PlayOneShot(clip);
                Debug.Log("アイテムコンプリート");
            
            //アイテムがすべて集まった時
            transform.DOScale(Vector3.one * 2, 0.6f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            });
            isOne_Comp = true;
        }
        else if (items.Count < 3)
        {
            //アイテムがすべて集まってないとき
            //UI出す処理
            PushUI();
        }
        ////UIの場所にアイテムを埋める処理
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] && !isOne[i])
            {
                //アイテムi個目
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
        //UIを出す処理
        coolTime = Mathf.Clamp(coolTime, -1, 1);

        if (moveManager.GetIsMoving())
        {
            //動いたら出さない
            coolTime -= Time.deltaTime;
        }
        else
        {
            coolTime += Time.deltaTime;
        }
        if (coolTime > 0.5f)
        {
            //止まって１秒立った時
            //一回だけ通す
            if (!isOne_)
            {
                isOne_ = true;
                transform.DOScale(Vector3.one, 0.3f).SetEase(TypeEase_In);
            }
        }
        else
        {
            //動き始めたとき
            //一回だけ通す
            if (isOne_)
            {
                isOne_ = false;
                transform.DOScale(Vector3.zero, 0.6f).SetEase(TypeEase_Out);
            }

        }
    }

    public void Initialize()
    {
        //リストをなくす。
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

        // List内のすべてのUIを破壊する
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
