using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICropScript : MonoBehaviour
{
    //プレイヤー
    GameObject Player;
    PlayerMoveManager moveManager;

    //スプライト
    SpriteRenderer spriteRenderer;

    //機能
    [SerializeField] float coolTime;
    [SerializeField] Ease TypeEase_In;
    [SerializeField] Ease TypeEase_Out;
    private bool isOne;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        moveManager = Player.GetComponent<PlayerMoveManager>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
            if (!isOne)
            {
                isOne = true;
                transform.DOScale(Vector3.one, 0.3f).SetEase(TypeEase_In);
            }
        }
        else
        {
            //動き始めたとき
            //一回だけ通す
            if (isOne)
            {
                isOne = false;
                transform.DOScale(Vector3.zero, 0.6f).SetEase(TypeEase_Out);
            }

        }

    }

    public void Initialize()
    {
        //動きとめる
        DOTween.KillAll();
        isOne = false;
        coolTime = 1.2f;
        transform.localScale = Vector3.one;
    }
}
