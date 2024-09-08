using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameBlock_Item : MonoBehaviour
{
    //�X�v���C�g���e�L�X�g
    private SpriteRenderer sprite_Blocks;
    private SpriteRenderer sprite_Items;
    private Text text_Blocs;
    private Text text_Items;


    //�u���b�N.�A�C�e���}�l�[�W���[
    private BlockManager blockManager;
    private ItemManager itemManager;

    //UI����֘A
    [Header("UI�̓���")]
    [SerializeField] Ease ui_EaseType_In;
    [SerializeField] Ease ui_EaseType_Out;
    [Header("UI�̂Ƃ���܂ł̓���(�u���b�N)")] 
    [SerializeField] Ease moveBlock_EaseType_In;
    [SerializeField] Ease moveBlock_EaseType_Out; 
    [Header("UI�̂Ƃ���܂ł̓���(�A�C�e��)")]
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
