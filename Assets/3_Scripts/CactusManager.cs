using DG.Tweening;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CactusManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private ParticleInstantiateScript particle;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        particle = GetComponent<ParticleInstantiateScript>();
    }

    // ����������
    void Initialize()
    {
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();
    }

    // ���ŏ���
    void Disappear()
    {
        particle.RunParticle(1);
        spriteRenderer.enabled = false;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetIsActive() && allObjectManager.GetHp() <= 0)
        {
            SetIsActive(false);
        }
    }
    public void SetIsActive(bool _isActive)
    {
        if (_isActive)
        {
            Initialize();
        }
        else
        {
            Disappear();
        }
    }
    public void SetHit()
    {
        particle.RunParticle(0);
        animator.SetTrigger("hitTrigger");
    }
}
