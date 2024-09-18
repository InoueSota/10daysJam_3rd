using UnityEngine;

public class AutoActiveChanger : MonoBehaviour
{

    [SerializeField] AllObjectManager allObjectManager;
    ParticleSystem particle;
    SpriteRenderer spriteRenderer;

    bool preActive;
    bool isActive;

    [SerializeField] bool particleClear = false;
    //[SerializeField] bool dummyDestroy = false;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        preActive = isActive;
        isActive = allObjectManager.GetIsActive();

        if (preActive != isActive)
        {
            if (isActive == true)
            {
                if (particle != null)
                {
                    particle.Play();
                }
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true;
                   
                }
            }
            else
            {
                if (particle != null)
                {
                    particle.Stop();
                    if(particleClear == true)
                    {
                        particle.Clear();
                    }
                }
                if (spriteRenderer != null)
                {
                   spriteRenderer.enabled = false;
                }
            }
        }
    }
}
