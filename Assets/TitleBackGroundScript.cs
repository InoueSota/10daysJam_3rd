using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleBackGroundScript : MonoBehaviour
{

    [SerializeField] SpriteRenderer[] backGround = null;
    [SerializeField] Sprite[] backGroundSprites = null;

    [SerializeField] float seeTimeMax;
    float seeTime = 0.0f;

    [SerializeField] float fadeTime = 3.0f;

    int seenBackGround = 0;
    
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        backGround[0].sprite = backGroundSprites[0];
        backGround[1].sprite = backGroundSprites[1];

        seeTime = seeTimeMax;

        player.transform.DOMove(new Vector3(10f, -2.5f, 0f), 5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear); ;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(seeTime < 0)
        {

            seenBackGround++;

            if (seenBackGround >= backGroundSprites.Length)
            {
                seenBackGround = 0;
            }

            int seenBackGroundNext = seenBackGround + 1;

            if(seenBackGroundNext >= backGroundSprites.Length)
            {
                seenBackGroundNext = 0;
            }

            seeTime = seeTimeMax;

            backGround[0].DOFade(endValue: 0f, duration: fadeTime).OnComplete(() =>
            {
                backGround[0].color = Color.white;

                backGround[0].sprite = backGroundSprites[seenBackGround];
                backGround[1].sprite = backGroundSprites[seenBackGroundNext];
            });
        }
        else
        {
            seeTime -= Time.deltaTime;
        }


    }
}
