using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    //[SerializeField] SpriteRenderer effectSprite;
    [SerializeField] Ease easeIn;
    [SerializeField] Ease easeOut;
    [SerializeField] Vector3 originScale;
    [SerializeField] Vector3 easeInScale;
    [SerializeField] Vector3 easeOutScale;
    [SerializeField] float secondIn;
    [SerializeField] float secondOut;
    [Header("オンにすると最後フェードアウトする")]
    [SerializeField] bool isFeed;
    [Header("オンにするとオブジェクトを保持する")]
    [SerializeField] bool isNotDestroy;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource&&audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
            transform.DOScale(easeInScale, secondIn).SetEase(easeIn).OnComplete(() =>
        {
            transform.DOScale(easeOutScale, secondOut).SetEase(easeOut).OnComplete(() =>
            {
                if (!isNotDestroy)
                {
                    Destroy(gameObject);
                }
            });
            if (isFeed)
            {
                transform.GetComponent<SpriteRenderer>().DOFade(endValue: 0, secondOut).SetEase(easeOut);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Initialize()
    {
        Destroy(gameObject);
    }

}
