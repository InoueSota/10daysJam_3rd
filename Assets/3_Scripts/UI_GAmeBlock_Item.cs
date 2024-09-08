using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameBlock_Item : MonoBehaviour
{
    //スプライト＆テキスト
    private SpriteRenderer sprite_Blocks;
    private SpriteRenderer sprite_Items;
    private Text text_Blocs;
    private Text text_Items;


    //ブロック.アイテムマネージャー
    private BlockManager blockManager;
    private ItemManager itemManager;

    //UI動作関連
    [Header("UIの動き")]
    [SerializeField] Ease ui_EaseType_In;
    [SerializeField] Ease ui_EaseType_Out;
    [Header("UIのところまでの動き(ブロック)")] 
    [SerializeField] Ease moveBlock_EaseType_In;
    [SerializeField] Ease moveBlock_EaseType_Out; 
    [Header("UIのところまでの動き(アイテム)")]
    [SerializeField] Ease moveItem_EaseType_In;
    [SerializeField] Ease moveItem_EaseType_Out;
    //
    //


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
