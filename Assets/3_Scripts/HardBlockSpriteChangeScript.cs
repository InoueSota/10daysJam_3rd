using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBlockSpriteChangeScript : MonoBehaviour
{

    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;

    [SerializeField] Sprite[] hardBlockSprites;

    // Start is called before the first frame update
    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int hp = allObjectManager.GetHp();

        if (hp > 0)
        {
            spriteRenderer.sprite = hardBlockSprites[hp - 1];
        }
    }
}
