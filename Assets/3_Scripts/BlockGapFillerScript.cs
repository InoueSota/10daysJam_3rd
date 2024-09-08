using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGapFillerScript : MonoBehaviour
{

    [SerializeField] Sprite[] gapFillerSprite = null;  
    SpriteRenderer SpriteRenderer = null;
    [SerializeField] Color fillColor;


    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.color = fillColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetType(int type)
    {
        if(type == -1)
        {
            SpriteRenderer.enabled = false;
        }
        else
        {
            SpriteRenderer.enabled = true;

            SpriteRenderer.sprite = gapFillerSprite[type];
        }
    }

    
}
