using DG.Tweening;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CactusManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private ParticleInstantiateScript particle;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        particle = GetComponent<ParticleInstantiateScript>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // 初期化処理
    void Initialize()
    {
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();
    }

    // 消滅処理
    void Disappear()
    {
        particle.RunParticle(1);
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
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
