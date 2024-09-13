using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DeathWarpAnimationManager : MonoBehaviour
{

    [SerializeField] float flowSpeed = 1.0f;
    [SerializeField] float flowHight = 1.0f;
    [SerializeField] float alphaSpeed = 1.0f;
    [SerializeField] float alphaNum = 0.7f;
    [SerializeField] SpriteRenderer deathWarpSprite;
  

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Update()
    {
       // Debug.Log(sequence);
    }

    public void Initialize()
    {
        var sequence = DOTween.Sequence();

        //deathWarpSprite.transform.localPosition = Vector3.down * flowHight * 0.5f;
        sequence.Append(deathWarpSprite.DOFade(alphaNum, alphaSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));
        //sequence.Join(deathWarpSprite.DOFade(alphaNum, alphaSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));

        sequence.Play();
    }

    // Update is called once per frame

}
