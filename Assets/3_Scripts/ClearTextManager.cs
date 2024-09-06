using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTextManager : MonoBehaviour
{
    //テキスト用
    [SerializeField] Text text;

    //プレイヤーカラー用
    [SerializeField] GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        //テキスト取得
        text = gameObject.GetComponent<Text>();

        //プレイヤー取得
        Player = GameObject.FindWithTag("Player");
        text.color = Player.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
