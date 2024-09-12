using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffectManager : MonoBehaviour
{

    ParticleInstantiateScript particle;
    GameManager gameManager;

    bool isClear = false;

    [SerializeField] float fireWorksTime = 1, fireWorksTimeMax = 1, fireWorksTimeMin = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleInstantiateScript>();
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        isClear = gameManager.GetIsClear();

        if (isClear == true)
        {
            if (fireWorksTime <= 0)
            {
                particle.RunParticle(0, new Vector3(Random.Range(-9f, 9f), Random.Range(-4f, 4f), 0.0f));
                fireWorksTime += fireWorksTimeMax;
            }
            else
            {
                fireWorksTime -= Time.deltaTime;
            }
        }

    }
}
