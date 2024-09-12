using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICropScript : MonoBehaviour
{
    //�v���C���[
    GameObject Player;
    PlayerMoveManager moveManager;

    //�X�v���C�g
    SpriteRenderer spriteRenderer;

    //�@�\
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
            if (!isOne)
            {
                isOne = true;
                transform.DOScale(Vector3.one, 0.3f).SetEase(TypeEase_In);
            }
        }
        else
        {
            //�����n�߂��Ƃ�
            //��񂾂��ʂ�
            if (isOne)
            {
                isOne = false;
                transform.DOScale(Vector3.zero, 0.6f).SetEase(TypeEase_Out);
            }

        }

    }

    public void Initialize()
    {
        //�����Ƃ߂�
        DOTween.KillAll();
        isOne = false;
        coolTime = 1.2f;
        transform.localScale = Vector3.one;
    }
}
