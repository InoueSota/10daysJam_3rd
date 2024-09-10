using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInstantiateScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ParticleSystem[] particle;
    Transform particleParent;

    void Start()
    {
        GameObject particleParentObject = GameObject.FindGameObjectWithTag("ParticleParent");

        if (particleParentObject == null)
        {
            Debug.Log("ふぇ～、パーティクルのいれものがないょぉ。プレハブの「ParticleParent」をいれてほしいのぉ。");
        }
        else
        {
            particleParent = particleParentObject.transform;
        }


        
    }
    public void RunParticle(int particleNum)
    {
        if (particle[particleNum] != null)
        {
            ParticleSystem particleObject = Instantiate(particle[particleNum], this.transform.position, Quaternion.identity);
            if (particleParent != null) {
                particleObject.gameObject.transform.parent = particleParent;
            }
        }
    }

    public void RunParticle(int particleNum, Vector3 particlePos)
    {
        if (particle[particleNum] != null)
        {
            ParticleSystem particleObject = Instantiate(particle[particleNum], particlePos, Quaternion.identity);
            if (particleParent != null)
            {
                particleObject.gameObject.transform.parent = particleParent;
            }
        }
    }

    public void RunParticle(int particleNum, Vector3 particlePos, Vector3 rot)
    {
        if (particle[particleNum] != null)
        {
            ParticleSystem particleObject = Instantiate(particle[particleNum], particlePos, Quaternion.EulerRotation(rot));
            if (particleParent != null)
            {
                particleObject.gameObject.transform.parent = particleParent;
            }
        }
    }

    public void RunParticleChild(int particleNum, Vector3 particlePos)
    {
        if (particle[particleNum] != null)
        {
            ParticleSystem particleObject = Instantiate(particle[particleNum], particlePos, Quaternion.identity);
            particleObject.gameObject.transform.parent = this.transform;
        }
    }

    
}
