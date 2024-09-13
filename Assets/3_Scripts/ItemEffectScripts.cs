using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectScripts : MonoBehaviour
{
    //itemマネージャー取得
    AllObjectManager item;

    //スプライト
    [SerializeField] GameObject front;
    [SerializeField] GameObject front_hahen;
    [SerializeField] GameObject back;
    [SerializeField] GameObject back_hahen;

    //
    [SerializeField] Vector3 originScale;

    //イージング
    [SerializeField] Ease Ease_In;
    [SerializeField] Ease Ease_Out;
    [SerializeField] Ease hahenEase_In;
    [SerializeField] Ease hahenEase_Out;
    [SerializeField] float easeScale;
    [SerializeField] float easeTime;
    [SerializeField] float easehahentime;
    bool isOne;

    //音関係

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip clip2;

    // Start is called before the first frame update
    void Start()
    {
        item=this.gameObject.GetComponent<AllObjectManager>();
        front.transform.localScale=Vector3.zero;
        front_hahen.transform.localScale = Vector3.zero;
        back.transform.localScale = Vector3.zero; ;
        back_hahen.transform.localScale = Vector3.zero; 
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!item.GetIsActive()&& !isOne && item.GetHp() <= 0)
        {
            audioSource.PlayOneShot(clip);
            audioSource2.PlayOneShot(clip2);

            Debug.Log("breakitem");
            //front
            front.transform.DOScale(easeScale, easeTime).SetEase(Ease_In).OnComplete(() =>
            {
                Debug.Log("complate");
                front.transform.DOScale(Vector3.zero, easeTime).SetEase(Ease_Out);
            });

            //back
            back.transform.DOScale(easeScale, easeTime).SetEase(Ease_In).OnComplete(() =>
            {
                back.transform.DOScale(Vector3.zero, easeTime).SetEase(Ease_Out);

            });

            //front破片
            front_hahen.transform.DOScale(easeScale, easehahentime).SetEase(hahenEase_In).OnComplete(() =>
            {
                front_hahen.transform.DOScale(Vector3.zero, easeTime).SetEase(Ease_Out);

            });

            //back破片
            back_hahen.transform.DOScale(easeScale, easehahentime).SetEase(hahenEase_In).OnComplete(() =>
            {
                back_hahen.transform.DOScale(Vector3.zero, easeTime).SetEase(Ease_Out);

            }); ;
            isOne = true;
        }
    }

    public void Initialized()
    {
        front.transform.localScale = Vector3.zero;
        front_hahen.transform.localScale = Vector3.zero;
        back.transform.localScale = Vector3.zero; ;
        back_hahen.transform.localScale = Vector3.zero;
        isOne = false;
    }
}
