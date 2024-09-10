using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer effectSprite;
    [SerializeField] Ease easeIn;
    [SerializeField] Ease easeOut;
    [SerializeField] Vector3 originScale;
    [SerializeField] Vector3 easeInScale;
    [SerializeField] Vector3 easeOutScale;
    [SerializeField] float secondIn;
    [SerializeField] float secondOut;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(easeInScale, secondIn).SetEase(easeIn).OnComplete(() =>
        {
            transform.DOScale(easeOutScale, secondOut).SetEase(easeOut).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
