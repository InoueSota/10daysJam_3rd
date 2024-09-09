using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInstantiateScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ParticleSystem[] particle;

    public void RunParticle(int particleNum)
    {
        if (particle[particleNum] != null)
        {
            Instantiate(particle[particleNum], this.transform.position, Quaternion.identity);
        }
    }

    public void RunParticle(int particleNum, Vector3 particlePos)
    {
        if (particle[particleNum] != null)
        {
            Instantiate(particle[particleNum], particlePos, Quaternion.identity);
        }
    }

    public void RunParticle(int particleNum, Vector3 particlePos, Vector3 rot)
    {
        if (particle[particleNum] != null)
        {
            Instantiate(particle[particleNum], particlePos, Quaternion.EulerRotation(rot));
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
