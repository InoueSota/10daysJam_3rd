using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTextManager : MonoBehaviour
{
    //�e�L�X�g�p
    [SerializeField] Text text;

    //�v���C���[�J���[�p
    [SerializeField] GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        //�e�L�X�g�擾
        text = gameObject.GetComponent<Text>();

        //�v���C���[�擾
        Player = GameObject.FindWithTag("Player");
        text.color = Player.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
