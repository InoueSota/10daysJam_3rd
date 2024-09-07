using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GrassParentManager : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    ParticleSystem particle = null;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();

        Disappear();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Disappear()
    {
        particle.Play();
        spriteRenderer.enabled = false;
    }

}
