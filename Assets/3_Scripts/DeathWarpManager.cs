using UnityEngine;

public class DeathWarpManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private ParticleInstantiateScript particle;
    private DeathWarpAnimationManager deathWarpAnimationManager;

    // 他オブジェクト取得
    private PlayerMoveManager playerMoveManager;

    void Start()
    {
        playerMoveManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveManager>();
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleInstantiateScript>();
        deathWarpAnimationManager = GetComponent<DeathWarpAnimationManager>();
    }

    // 初期化処理
    void Initialize()
    {
        deathWarpAnimationManager.Initialize();
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();
    }

    // 消滅処理
    void Disappear()
    {
        spriteRenderer.enabled = false;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        //deathWarpAnimationManager.Disappear();
        particle.RunParticle(0);
    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetIsActive() && allObjectManager.GetHp() <= 0)
        {
            playerMoveManager.SetDeathWarp(transform.position);
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
}
