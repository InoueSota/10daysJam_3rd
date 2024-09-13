using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemParticleManager : MonoBehaviour
{

    ParticleInstantiateScript particle;

    [SerializeField] float particleTimeMax;
    float firstTime = 2;
    float particleTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleInstantiateScript>();
        particleTime = firstTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RunParticle()
    {
        if(particleTime  < 0)
        {
            particle.RunParticle(0);

            particleTime = particleTimeMax;
        }
        else
        {
            particleTime -= Time.deltaTime;
        }
    }

    public void Reset()
    {
        particleTime = firstTime;
    }
}
