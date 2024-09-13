using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBlockAnimationManager : MonoBehaviour
{

    SplitManager splitManager;
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer overRenderer = null;
    AudioSource audio;

    bool isAvoid = false;

    // Start is called before the first frame update
    void Start()
    {
        splitManager = GetComponent<SplitManager>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool preAvoid = isAvoid;
        isAvoid = splitManager.GetAVoid();
        if (isAvoid != preAvoid)
        {
            if (isAvoid == true)
            {
                overRenderer.sprite = sprites[1];
                audio.Play();
            }
            else
            {
                overRenderer.sprite = sprites[0];
            }
        }
    }
}
