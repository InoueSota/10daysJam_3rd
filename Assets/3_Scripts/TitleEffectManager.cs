using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleEffectManager : MonoBehaviour
{
    //イージングの動きかた
    [SerializeField] Ease easeIn;
    [SerializeField] Ease ease_Second;
    [SerializeField] Vector3 StartPos;
    [SerializeField] Vector3 SecondPos;
    [SerializeField] Vector3 EndPos;
    [SerializeField] float duration;
    [SerializeField] float duration_Second;

    [SerializeField] Ease easeInScale;
    [SerializeField] Vector3 easeScale;
    Vector3 originScale;
    bool isComplate;
    //スプライト
    [SerializeField] SpriteRenderer spriteRenderer;

    //サイン波
    //揺らす用
    private Vector3 originPos;
    [SerializeField] float angle;
    [SerializeField] float lenge;
    [SerializeField] float flowSpeed;
   

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // StartPos = transform.position;
        originScale = transform.localScale;
        transform.localScale = easeScale;
        transform.position = StartPos;
        //Sequenceのインスタンスを作成
        var sequence = DOTween.Sequence();

        //Appendで動作を追加していく
        sequence.Append(transform.DOMove(SecondPos, duration).SetEase(easeIn));
        sequence.Join(transform.DOScale(originScale, duration).SetEase(easeInScale));
        sequence.Append(transform.DOMove(EndPos, duration_Second).SetEase(ease_Second));

        //Playで実行
        sequence.Play().OnComplete(() =>
        {
            Debug.Log("OnComplete!");
            isComplate = true;
        }); ;

    }
    private void FixedUpdate()
    {
        if (isComplate)
        {

            angle += flowSpeed;
            Vector3 frowPos = new Vector3(EndPos.x, EndPos.y + (MathF.Sin(angle) * lenge), EndPos.z);
            transform.position = frowPos;
        }
    }

    public void Initialize()
    {
        DOTween.Kill(this);
        originScale = transform.localScale;
        transform.position = StartPos;
        isComplate = false;
        angle=0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
