using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBlockAnimationManager : MonoBehaviour
{

    SplitManager splitManager;
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer overRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        splitManager = GetComponent<SplitManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (splitManager.GetAVoid() == true)
        {
            overRenderer.sprite = sprites[1];
        }
        else
        {
            overRenderer.sprite = sprites[0];
        }
    }
}
